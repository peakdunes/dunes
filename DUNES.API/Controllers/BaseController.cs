using DUNES.Shared.Utils.Reponse;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DUNES.API.Controllers
{
    /// <summary>
    /// 1. De este controlador heredan todos los demas controladores
    /// para tener respuesta standard
    /// 2. captura excepciones en el controlador y arma la respuesta (y loguea con Serilog).
    /// 3. Cancelaciones: si el cliente corta la petición, responde 499 de forma uniforme.
    /// 4. Logging consistente: registra el error con TraceId/ruta/mensaje base.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        #region Private Helpers

        private string GetTraceId()
        {
            // Usa el X-Trace-Id si viene del middleware; si no, usa el TraceIdentifier del framework
            return Request.Headers.TryGetValue("X-Trace-Id", out var h) && !string.IsNullOrWhiteSpace(h)
                ? h.ToString()
                : HttpContext.TraceIdentifier;
        }

        private static string CleanBaseMessage(Exception ex)
        {
            var msg = ex.GetBaseException().Message ?? string.Empty;
            msg = msg.Split('\n')[0].Trim();
            return msg.Length > 500 ? msg[..500] : msg;
        }

        private void LogError(Exception ex, string origen = "[HELPER]")
        {
            var traceId = GetTraceId();
            var route = $"{Request?.Method} {Request?.Path}{Request?.QueryString}";
            var baseMsg = CleanBaseMessage(ex);

            // Serilog (archivo) – stack completo
            Log.Error(ex,
                "{Origen} TraceId: {TraceId} | Route: {Route} | {Message}",
                origen, traceId, route, baseMsg);
        }

        private IActionResult BuildErrorResponse<T>(Exception ex, int statusCode = 500, string? customMessage = null)
        {
            LogError(ex, "[HANDLE]");
            var traceId = GetTraceId();
            var baseMsg = CleanBaseMessage(ex);

            return StatusCode(statusCode, ApiResponseFactory.Fail<T>(
                baseMsg,
                customMessage ?? "❌ Internal Server Error",
                statusCode,
                traceId
            ));
        }

        private IActionResult BuildCancelResponse<T>()
        {
            return StatusCode(499, ApiResponseFactory.Fail<T>(
                "Request was cancelled",
                "⚠️ Client closed request",
                499,
                GetTraceId()
            ));
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Llama a tu servicio que ya devuelve ApiResponse y se limita a pasar el resultado; 
        /// si hay error/cancelación, construye la respuesta estándar.
     
        /// </summary>
        protected async Task<IActionResult> HandleApi<T>(
            Func<CancellationToken, Task<ApiResponse<T>>> action,
            CancellationToken ct)
        {
            try
            {
                var resp = await action(ct);
                var status = resp.StatusCode == 0 ? 200 : resp.StatusCode;
                return StatusCode(status, resp);
            }
            catch (OperationCanceledException)
            {
                return BuildCancelResponse<T>();
            }
            catch (Exception ex)
            {
                return BuildErrorResponse<T>(ex);
            }
        }

        /// <summary>
        /// (Opcional) Para servicios que devuelven T y no ApiResponse&lt;T&gt;.
        /// </summary>
        protected async Task<IActionResult> Handle<T>(
            Func<CancellationToken, Task<T>> action,
            CancellationToken ct)
        {
            try
            {
                var data = await action(ct);
                return Ok(ApiResponseFactory.Ok(data));
            }
            catch (OperationCanceledException)
            {
                return BuildCancelResponse<T>();
            }
            catch (Exception ex)
            {
                return BuildErrorResponse<T>(ex);
            }
        }

        #endregion
    }
}

using DUNES.API.ServicesWMS.Admin;
// ✅ Ajusta este using al namespace real donde vive IErrorLogService en tu solución
using DUNES.API.Utils.Logging;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DUNES.API.Controllers
{
    /// <summary>
    /// Base controller para estandarizar:
    /// <list type="bullet">
    /// <item><description>Respuestas (ApiResponse) uniformes.</description></item>
    /// <item><description>Manejo de cancelaciones (499) cuando el cliente corta la petición.</description></item>
    /// <item><description>Manejo de excepciones: log a archivo (Serilog) + persistencia a BD (IErrorLogService) para errores 5xx.</description></item>
    /// <item><description>Acceso a CompanyId / CompanyClientId exclusivamente desde el token (multi-tenant).</description></item>
    /// </list>
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        #region Tenant scope (Token)

        /// <summary>
        /// Obtiene el <c>CompanyId</c> desde el token JWT.
        /// <para>
        /// Regla: este valor nunca debe venir por body/query; solo del token.
        /// </para>
        /// </summary>
        protected int CurrentCompanyId
        {
            get
            {
                var claim = User?.FindFirst("companyId")?.Value;

                if (string.IsNullOrWhiteSpace(claim) || !int.TryParse(claim, out var value))
                    throw new UnauthorizedAccessException("Missing or invalid claim: companyId.");

                return value;
            }
        }

        /// <summary>
        /// Obtiene el <c>CompanyClientId</c> desde el token JWT.
        /// <para>
        /// Regla: este valor nunca debe venir por body/query; solo del token.
        /// </para>
        /// </summary>
        protected int CurrentCompanyClientId
        {
            get
            {
                var claim = User?.FindFirst("companyClientId")?.Value;

                if (string.IsNullOrWhiteSpace(claim) || !int.TryParse(claim, out var value))
                    throw new UnauthorizedAccessException("Missing or invalid claim: companyClientId.");

                return value;
            }
        }

        #endregion

        #region Private helpers

        /// <summary>
        /// Obtiene el TraceId/correlation id del request.
        /// <para>
        /// 1) Si existe header <c>X-Trace-Id</c> (normalmente set por middleware), lo usa.
        /// </para>
        /// <para>
        /// 2) Si no existe, usa <see cref="HttpContext.TraceIdentifier"/>.
        /// </para>
        /// </summary>
        /// <returns>TraceId del request.</returns>
        private string GetTraceId()
        {
            return Request.Headers.TryGetValue("X-Trace-Id", out var h) && !string.IsNullOrWhiteSpace(h)
                ? h.ToString()
                : HttpContext.TraceIdentifier;
        }

        /// <summary>
        /// Limpia el mensaje base de una excepción:
        /// <list type="bullet">
        /// <item><description>Usa <see cref="Exception.GetBaseException"/>.</description></item>
        /// <item><description>Toma solo la primera línea (sin saltos).</description></item>
        /// <item><description>Trunca a 500 caracteres.</description></item>
        /// </list>
        /// </summary>
        /// <param name="ex">Excepción a procesar.</param>
        /// <returns>Mensaje base limpio y corto.</returns>
        private static string CleanBaseMessage(Exception ex)
        {
            var msg = ex.GetBaseException().Message ?? string.Empty;
            msg = msg.Split('\n')[0].Trim();
            return msg.Length > 500 ? msg[..500] : msg;
        }

        /// <summary>
        /// Registra el error en archivo (Serilog).
        /// En Development incluye stack trace completo; en Production solo headline.
        /// </summary>
        private void LogError(Exception ex, string origen = "[HELPER]")
        {
            var traceId = GetTraceId();
            var route = $"{Request?.Method} {Request?.Path}{Request?.QueryString}";
            var baseMsg = CleanBaseMessage(ex);

            // Siempre headline limpio (sin stack)
            Log.Error(
                "{Origen} TraceId: {TraceId} | Route: {Route} | {Message}",
                origen, traceId, route, baseMsg);
        }

        /// <summary>
        /// Construye una respuesta estándar de error y, para errores 5xx,
        /// intenta persistir el error en BD usando <c>IErrorLogService</c>.
        /// <para>
        /// - Nunca rompe la respuesta si falla el guardado en BD.
        /// </para>
        /// </summary>
        /// <typeparam name="T">Tipo de <c>Data</c> dentro del ApiResponse.</typeparam>
        /// <param name="ex">Excepción capturada.</param>
        /// <param name="statusCode">Código HTTP (default: 500).</param>
        /// <param name="customMessage">Mensaje custom para el cliente (opcional).</param>
        /// <returns>ActionResult con ApiResponse.Fail.</returns>
        private async Task<IActionResult> BuildErrorResponseAsync<T>(Exception ex, int statusCode = 500, string? customMessage = null)
        {
            // 1) Log a archivo siempre
            LogError(ex, "[HANDLE]");

            // 2) Datos base para respuesta
            var traceId = GetTraceId();
            var baseMsg = CleanBaseMessage(ex);

            // 3) Persistencia en BD (solo errores 5xx)
            if (statusCode >= 500)
            {
                try
                {
                    // Resuelve el servicio desde el container por request (sin ensuciar el constructor)
                    var errorLogService = HttpContext.RequestServices.GetRequiredService<IErrorLogService>();

                    await errorLogService.TrySaveAsync(
                        HttpContext,
                        ex,
                        origin: "BaseController.Handle",
                        overrideMessage: baseMsg
                    );
                }
                catch (Exception logEx)
                {
                    // Fallback: si el logger/DB falla, no se debe afectar el response
                    Log.Error(logEx, "[ERRORLOG] Failed saving error to DB. TraceId={TraceId}", traceId);
                }
            }

            // 4) Respuesta estándar
            return StatusCode(statusCode, ApiResponseFactory.Fail<T>(
                baseMsg,
                customMessage ?? "❌ Something went wrong while processing your request. If the issue continues, contact support.",
                statusCode,
                traceId
            ));
        }

        /// <summary>
        /// Construye una respuesta estándar 499 para cancelaciones (cliente cortó request).
        /// </summary>
        /// <typeparam name="T">Tipo del ApiResponse.</typeparam>
        /// <returns>ActionResult 499 con ApiResponse.Fail.</returns>
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
        /// Handler estándar para acciones que ya devuelven <see cref="ApiResponse{T}"/>.
        /// <para>
        /// - Si el servicio devuelve StatusCode=0, se asume 200.
        /// </para>
        /// <para>
        /// - Si hay cancelación (<see cref="OperationCanceledException"/>), responde 499.
        /// </para>
        /// <para>
        /// - Si hay excepción, responde 500 estándar y persiste el error en BD (solo 5xx).
        /// </para>
        /// </summary>
        /// <typeparam name="T">Tipo de Data en el ApiResponse.</typeparam>
        /// <param name="action">Función que ejecuta la lógica y retorna ApiResponse.</param>
        /// <param name="ct">CancellationToken.</param>
        /// <returns>ActionResult estándar.</returns>
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
                return await BuildErrorResponseAsync<T>(ex);
            }
        }

        /// <summary>
        /// Handler alternativo para servicios que retornan un <typeparamref name="T"/> directo
        /// (sin ApiResponse).
        /// <para>
        /// - En éxito, devuelve <c>ApiResponseFactory.Ok(data)</c>.
        /// </para>
        /// <para>
        /// - Cancelación: 499.
        /// </para>
        /// <para>
        /// - Error: 500 estándar + persistencia a BD (solo 5xx).
        /// </para>
        /// </summary>
        /// <typeparam name="T">Tipo de dato retornado por el servicio.</typeparam>
        /// <param name="action">Función que retorna el dato.</param>
        /// <param name="ct">CancellationToken.</param>
        /// <returns>ActionResult estándar.</returns>
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
                return await BuildErrorResponseAsync<T>(ex);
            }
        }

        #endregion
    }
}

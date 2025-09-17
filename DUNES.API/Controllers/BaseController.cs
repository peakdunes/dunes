using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace DUNES.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        #region Private Helpers

        private void LogError(Exception ex, string origen = "[HELPER]")
        {
            Log.Error(ex,
                $"{origen} TraceId: {{TraceId}} - {{Message}}",
                HttpContext.TraceIdentifier,
                ex.GetBaseException().Message);
        }

        private IActionResult BuildErrorResponse<T>(Exception ex, int statusCode = 500, string? customMessage = null)
        {
            LogError(ex, "[HANDLE]");
            return StatusCode(statusCode, ApiResponseFactory.Fail<T>(
                ex.GetBaseException().Message,
                customMessage ?? "❌ Internal Server Error",
                statusCode,
                HttpContext.TraceIdentifier
            ));
        }

        private IActionResult BuildCancelResponse<T>()
        {
            return StatusCode(499, ApiResponseFactory.Fail<T>(
                "Request was cancelled",
                "⚠️ Client closed request",
                499,
                HttpContext.TraceIdentifier
            ));
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Use this when the service already returns ApiResponse<T>.
        /// Controller does not re-wrap; just passes through.
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

        #endregion
    }
}

using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Controllers
{
    /// <summary>
    /// Base controller that provides centralized error handling and standardized API responses.
    /// </summary>
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// PASSTHROUGH: Usa este método cuando el servicio YA devuelve ApiResponse<T>
        /// No re-empaqueta; reenvía el ApiResponse y su HTTP status.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        protected async Task<IActionResult> HandleApi<T>(Func<Task<ApiResponse<T>>> action)
        {
            var resp = await action();
            var status = resp.StatusCode == 0 ? 200 : resp.StatusCode;
            return StatusCode(status, resp);
        }

        /// <summary>
        /// Handles async operations that return a raw data result (T) and wraps it once into ApiResponse.
        /// Usa este método cuando el servicio devuelve T "puro" (no ApiResponse).
        /// </summary>
        protected async Task<IActionResult> Handle<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();

                return StatusCode(200, ApiResponseFactory.Success(result, "Successful transaction", 200, HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<T>(
                    ex.GetBaseException().Message,
                    "❌ Internal Server Error",
                    500,
                    HttpContext.TraceIdentifier
                ));
            }
        }
        /// <summary>
        /// Handles async operations that return a raw data result (T) and wraps it once into ApiResponse.
        /// Usa este método cuando el servicio devuelve T "puro" (no ApiResponse).
        /// </summary>
        protected async Task<IActionResult> Handle<T>(Func<Task<ApiResponse<T>>> action)
        {
            var resp = await action();
            var status = resp.StatusCode == 0 ? 200 : resp.StatusCode;
            return StatusCode(status, resp); // no re-empaques
        }

        /// <summary>
        /// Handles async operations that already build an IActionResult.
        /// </summary>
        protected async Task<IActionResult> Handle(Func<Task<IActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<object>(
                    ex.GetBaseException().Message,
                    "❌ Internal Server Error",
                    500,
                    HttpContext.TraceIdentifier
                ));
            }
        }

        /// <summary>
        /// Handles synchronous operations with return value; wraps once into ApiResponse.
        /// Usa este método cuando el servicio devuelve T "puro".
        /// </summary>
        protected IActionResult HandleSync<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return StatusCode(200, ApiResponseFactory.Success(result, "Successful transaction", 200, HttpContext.TraceIdentifier));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponseFactory.Fail<T>(
                    ex.GetBaseException().Message,
                    "❌ Internal Server Error",
                    500,
                    HttpContext.TraceIdentifier
                ));
            }
        }
    }
}

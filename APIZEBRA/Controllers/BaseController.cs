using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers
{

    /// <summary>
    /// Base controller that provides centralized error handling and standardized API responses.
    /// </summary>
    /// 
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        /// <summary>
        /// Handles asynchronous operations that return a data result, with automatic error handling and ApiResponse wrapping.
        /// </summary>
        /// <typeparam name="T">The type of data returned by the action.</typeparam>
        /// <param name="action">The asynchronous function to execute.</param>
        /// <returns>Standardized ApiResponse wrapped in IActionResult.</returns>
        protected async Task<IActionResult> Handle<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return StatusCode(200, ApiResponseFactory.Success(result, "✅ Successful transaction", 200, HttpContext.TraceIdentifier));
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
        /// Handles asynchronous operations that return an IActionResult directly, with centralized error handling.
        /// </summary>
        /// <param name="action">The asynchronous function to execute.</param>
        /// <returns>IActionResult, either the original result or a formatted error response.</returns>
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
        /// Handles synchronous operations with return value, wrapped in ApiResponse and with centralized error handling.
        /// </summary>
        /// <typeparam name="T">The type of data returned by the action.</typeparam>
        /// <param name="action">The synchronous function to execute.</param>
        /// <returns>Standardized ApiResponse wrapped in IActionResult.</returns>
        protected IActionResult HandleSync<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return StatusCode(200, ApiResponseFactory.Success(result, "✅ Successful transaction", 200, HttpContext.TraceIdentifier));
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
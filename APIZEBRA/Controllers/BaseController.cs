using APIZEBRA.Utils.Responses;
using Microsoft.AspNetCore.Mvc;

namespace APIZEBRA.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
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
    }
}
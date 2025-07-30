using Microsoft.AspNetCore.Mvc;

namespace DUNES.API.Utils.Responses
{
    public class Respond
    {

        public static IActionResult Ok<T>(T data) =>
        new ObjectResult(ApiResponseFactory.Success(data)) { StatusCode = 200 };

        public static IActionResult Ok<T>(T data, string mensaje) =>
            new ObjectResult(ApiResponseFactory.Success(data, mensaje, 200)) { StatusCode = 200 };

        public static IActionResult Created<T>(T data, string mensaje = "Resource created.") =>
            new ObjectResult(ApiResponseFactory.Success(data, mensaje, 201)) { StatusCode = 201 };

        public static IActionResult NoContent(string mensaje = "Request processed successfully.") =>
            new ObjectResult(ApiResponseFactory.Success<object>(null, mensaje, 204)) { StatusCode = 204 };

        public static IActionResult BadRequest<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Bad Request", 400)) { StatusCode = 400 };

        public static IActionResult Unauthorized<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Unauthorized", 401)) { StatusCode = 401 };

        public static IActionResult Forbidden<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Forbidden", 403)) { StatusCode = 403 };

        public static IActionResult NotFound<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Not Found", 404)) { StatusCode = 404 };

        public static IActionResult Conflict<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Conflict", 409)) { StatusCode = 409 };

        public static IActionResult Unprocessable<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Unprocessable Entity", 422)) { StatusCode = 422 };

        public static IActionResult InternalError<T>(string error) =>
            new ObjectResult(ApiResponseFactory.Fail<T>(error, "Internal Server Error", 500)) { StatusCode = 500 };

        public static IActionResult BadRequest(string error) =>
    BadRequest<string>(error);

        public static IActionResult Unauthorized(string error) =>
            Unauthorized<string>(error);

        public static IActionResult Forbidden(string error) =>
            Forbidden<string>(error);

        public static IActionResult NotFound(string error) =>
            NotFound<string>(error);

        public static IActionResult Conflict(string error) =>
            Conflict<string>(error);

        public static IActionResult Unprocessable(string error) =>
            Unprocessable<string>(error);

        public static IActionResult InternalError(string error) =>
            InternalError<string>(error);
    }
}

using Microsoft.AspNetCore.Http;

namespace APIZEBRA.Utils.Responses
{
    public static class ApiResponseFactory
    {
        // ----------------- SUCCESS RESPONSES -----------------

        /// <summary>
        /// Success response without TraceId
        /// </summary>
        public static ApiResponse<T> Success<T>(T data, string message = "OK", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                ProcessDate = DateTime.Now
            };
        }

        /// <summary>
        /// Success response with TraceId
        /// </summary>
        public static ApiResponse<T> Success<T>(T data, string message, int statusCode, string traceId)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = statusCode,
                TraceId = traceId,
                ProcessDate = DateTime.Now
            };
        }

        public static ApiResponse<T> Ok<T>(T data, string message = "OK") =>
            Success(data, message, 200);

        public static ApiResponse<T> Created<T>(T data, string message = "Created") =>
            Success(data, message, 201);

        public static ApiResponse<T> NoContent<T>(string message = "No Content") =>
            Success<T>(default!, message, 204);

        // ----------------- ERROR RESPONSES -----------------

        /// <summary>
        /// Fail response without TraceId
        /// </summary>
        public static ApiResponse<T> Fail<T>(string error, string message, int statusCode)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Error = error,
                StatusCode = statusCode,
                Data = default,
                ProcessDate = DateTime.Now
            };
        }

        /// <summary>
        /// Fail response with TraceId
        /// </summary>
        public static ApiResponse<T> Fail<T>(string error, string message, int statusCode, string traceId)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Error = error,
                StatusCode = statusCode,
                TraceId = traceId,
                Data = default,
                ProcessDate = DateTime.Now
            };
        }

        // ----------------- SHORTHAND FAIL ALIASES -----------------

        public static ApiResponse<T> BadRequest<T>(string error, string message = "Bad Request") =>
            Fail<T>(error, message, 400);

        public static ApiResponse<T> Unauthorized<T>(string error = "Unauthorized access") =>
            Fail<T>(error, "Unauthorized", 401);

        public static ApiResponse<T> Forbidden<T>(string error = "Access denied") =>
            Fail<T>(error, "Forbidden", 403);

        public static ApiResponse<T> NotFound<T>(string error = "Resource not found") =>
            Fail<T>(error, "Not Found", 404);

        public static ApiResponse<T> Conflict<T>(string error = "Conflict detected") =>
            Fail<T>(error, "Conflict", 409);

        public static ApiResponse<T> Unprocessable<T>(string error = "Validation failed") =>
            Fail<T>(error, "Unprocessable entity", 422);

        public static ApiResponse<T> InternalError<T>(string error = "Unexpected server error") =>
            Fail<T>(error, "Internal Server Error", 500);
    }

}

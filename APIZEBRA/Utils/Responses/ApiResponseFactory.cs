using Microsoft.AspNetCore.Http;

namespace APIZEBRA.Utils.Responses
{
    /// <summary>
    /// Factory class for building standardized API responses.
    /// </summary>
    public static class ApiResponseFactory
    {
        // ----------------- SUCCESS RESPONSES -----------------

        /// <summary>
        /// Creates a successful response without a TraceId.
        /// </summary>
        /// <typeparam name="T">Type of the data returned.</typeparam>
        /// <param name="data">Response payload.</param>
        /// <param name="message">Optional message. Defaults to "OK".</param>
        /// <param name="statusCode">HTTP status code. Defaults to 200.</param>
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
        /// Creates a successful response with a TraceId.
        /// </summary>
        /// <typeparam name="T">Type of the data returned.</typeparam>
        /// <param name="data">Response payload.</param>
        /// <param name="message">Response message.</param>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="traceId">Trace identifier for tracking.</param>
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

        /// <summary>
        /// Returns a 200 OK response.
        /// </summary>
        public static ApiResponse<T> Ok<T>(T data, string message = "OK") =>
            Success(data, message, 200);

        /// <summary>
        /// Returns a 201 Created response.
        /// </summary>
        public static ApiResponse<T> Created<T>(T data, string message = "Created") =>
            Success(data, message, 201);

        /// <summary>
        /// Returns a 204 No Content response.
        /// </summary>
        public static ApiResponse<T> NoContent<T>(string message = "No Content") =>
            Success<T>(default!, message, 204);

        // ----------------- ERROR RESPONSES -----------------

        /// <summary>
        /// Creates a failed response without a TraceId.
        /// </summary>
        /// <typeparam name="T">Type of the data returned.</typeparam>
        /// <param name="error">Detailed error message.</param>
        /// <param name="message">General response message.</param>
        /// <param name="statusCode">HTTP status code.</param>
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
        /// Creates a failed response with a TraceId.
        /// </summary>
        /// <typeparam name="T">Type of the data returned.</typeparam>
        /// <param name="error">Detailed error message.</param>
        /// <param name="message">General response message.</param>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="traceId">Trace identifier for tracking.</param>
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
        
        /// <summary>
        /// Returns a 400 Bad Request response.
        /// </summary>
        public static ApiResponse<T> BadRequest<T>(string error, string message = "Bad Request") =>
            Fail<T>(error, message, 400);

        /// <summary>
        /// Returns a 401 Unauthorized response.
        /// </summary>
        public static ApiResponse<T> Unauthorized<T>(string error = "Unauthorized access") =>
            Fail<T>(error, "Unauthorized", 401);

        /// <summary>
        /// Returns a 403 Forbidden response.
        /// </summary>
        public static ApiResponse<T> Forbidden<T>(string error = "Access denied") =>
            Fail<T>(error, "Forbidden", 403);

        /// <summary>
        /// Returns a 404 Not Found response.
        /// </summary>
        public static ApiResponse<T> NotFound<T>(string error = "Resource not found") =>
            Fail<T>(error, "Not Found", 404);

        /// <summary>
        /// Returns a 409 Conflict response.
        /// </summary>
        public static ApiResponse<T> Conflict<T>(string error = "Conflict detected") =>
            Fail<T>(error, "Conflict", 409);

        /// <summary>
        /// Returns a 422 Unprocessable Entity response.
        /// </summary>
        public static ApiResponse<T> Unprocessable<T>(string error = "Validation failed") =>
            Fail<T>(error, "Unprocessable entity", 422);

        /// <summary>
        /// Returns a 500 Internal Server Error response.
        /// </summary>
        public static ApiResponse<T> InternalError<T>(string error = "Unexpected server error") =>
            Fail<T>(error, "Internal Server Error", 500);
        /// <summary>
        /// for save error from Global exeptions (in database dbk_mvc_logs_api, and txt file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="traceId"></param>
        /// <returns></returns>
        public static ApiResponse<T> InternalError<T>(string message, string? traceId = null)
        {
            return new ApiResponse<T>
            {
                Message = message,
                Error = "Internal Server Error",
                StatusCode = 500,
                Success = false,
                TraceId = traceId
            };
        }

    }


}

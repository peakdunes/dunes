namespace APIZEBRA.Utils.Responses
{
    public class ApiResponseHelper
    {
        public static ApiResponse<T> Ok<T>(T data, string message = "✅ Transaction successful") =>
            ApiResponseFactory.Success(data, message, 200);

        public static ApiResponse<T> Created<T>(T data, string message = "✅ Resource created") =>
            ApiResponseFactory.Success(data, message, 201);

        public static ApiResponse<object> NoContent(string message = "✅ No content") =>
           ApiResponseFactory.Success<object>(null!, message, 204);

        public static ApiResponse<T> BadRequest<T>(string error, string message = "❌ Invalid request") =>
            ApiResponseFactory.Fail<T>(error, message, 400);

        public static ApiResponse<T> Unauthorized<T>(string error = "Unauthorized access") =>
            ApiResponseFactory.Fail<T>(error, "🔒 Unauthorized", 401);

        public static ApiResponse<T> Forbidden<T>(string error = "Access denied") =>
            ApiResponseFactory.Fail<T>(error, "🚫 Forbidden", 403);

        public static ApiResponse<T> NotFound<T>(string error = "Resource not found") =>
            ApiResponseFactory.Fail<T>(error, "❌ Not found", 404);

        public static ApiResponse<T> Conflict<T>(string error = "Conflict detected") =>
            ApiResponseFactory.Fail<T>(error, "⚠️ Conflict", 409);

        public static ApiResponse<T> Unprocessable<T>(string error = "Validation failed") =>
            ApiResponseFactory.Fail<T>(error, "❌ Unprocessable entity", 422);

        public static ApiResponse<T> InternalError<T>(string error = "Unexpected server error") =>
            ApiResponseFactory.Fail<T>(error, "💥 Internal server error", 500);
    }
}

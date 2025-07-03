using Microsoft.AspNetCore.Http;

namespace APIZEBRA.Utils.Responses
{
    public static class ApiResponseFactory
    {
        public static ApiResponse<T> Success<T>(T data, string mensaje = "✅ Successful transaction", int statusCode = 200, string? traceId = null)
            => new()
            {
                Mensaje = mensaje,
                Data = data,
                StatusCode = statusCode,
                TraceId = traceId,
                ProcessDate = DateTime.Now,
                Success = true,
                Warnings = new List<string>()
            };

        public static ApiResponse<T> Fail<T>(string error, string mensaje = "❌ Failed transaction", int statusCode = 400, string? traceId = null)
            => new()
            {
                Mensaje = mensaje,
                Error = error,
                Data = default,
                StatusCode = statusCode,
                TraceId = traceId,
                ProcessDate = DateTime.Now,
                Success = false,
                Warnings = new List<string>()
            };
    }
}
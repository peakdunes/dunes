using APIZEBRA.Utils.Logging;
using APIZEBRA.Utils.Responses;
using Serilog;
using System.Net;
using System.Text.Json;

namespace APIZEBRA.Utils.Middlewares
{
    /// <summary>
    /// Global exepction catcher 
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = Guid.NewGuid().ToString();

                // 🔹 Log en archivo plano .txt (Serilog)
                _logger.LogError(ex, "[MIDDLEWARE] TraceID: {TraceId} - Unhandled exception: {Message}", traceId, ex.Message);

                // 🔹 Guardar en tu tabla personalizada
                using var scope = _scopeFactory.CreateScope();
                var logHelper = scope.ServiceProvider.GetRequiredService<LogHelper>();

                await logHelper.SaveLogAsync(
                    traceId: traceId,
                    message: ex.Message,
                    exception: ex.ToString(),
                    level: "Error",
                    usuario: context.User?.Identity?.Name ?? "anonymous",
                    origen: "GlobalExceptionMiddleware",
                    ruta: context.Request.Path
                );

                // 🔹 Respuesta JSON al cliente
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = ApiResponseFactory.InternalError<object>(
                    ex.GetBaseException().Message.Split('\n')[0].Trim(),
                    traceId
                );

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}

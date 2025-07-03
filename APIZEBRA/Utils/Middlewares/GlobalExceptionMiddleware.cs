using APIZEBRA.Utils.Logging;
using Serilog;
using System.Net;
using System.Text.Json;

namespace APIZEBRA.Utils.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        //errores no atrapados en ningun TRY CATCH dentro de cualquier controlador
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public GlobalExceptionMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "🔥 [MIDDLEWARE] Unhandled exception: {Message}", ex.Message);

                // Crear un scope (tipo de servicio que dura solo lo que dura la peticion HTTP) para resolver LogHelper
                using var scope = _scopeFactory.CreateScope();
                var logHelper = scope.ServiceProvider.GetRequiredService<LogHelper>();

                await logHelper.LogErrorAsync(
                    ex,
                    origen: "GlobalExceptionMiddleware",
                    ruta: context.Request.Path,
                    usuario: context.User?.Identity?.Name ?? "anonymous"
                );

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    mensaje = "❌ Internal server error",
                    error = ex.GetBaseException().Message.Split('\n')[0].Trim(),
                    data = (object)null,
                    fechaProceso = DateTime.UtcNow,
                    statusCode = 500
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}

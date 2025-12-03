using DUNES.API.Utils.Logging;

using DUNES.Shared.Utils.Reponse;
using Serilog;
using System.Net;
using System.Text.Json;

namespace DUNES.API.Utils.Middlewares
{
    /// <summary> Global exception catcher </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;


        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="next"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="logger"></param>
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            IServiceScopeFactory scopeFactory,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        /// <summary>
        /// para el request (http) al siguiente middleware o controlador
        /// y espera al respuesta, si falla escribe el error en la bd y en el log
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = Guid.NewGuid().ToString();
                var method = context.Request?.Method ?? "N/A";
                var path = context.Request?.Path.ToString() ?? "/";
                var qs = context.Request?.QueryString.HasValue == true ? context.Request.QueryString.Value : string.Empty;
                var fullRoute = $"{method} {path}{qs}";
                var baseMsg = ex.GetBaseException().Message.Split('\n')[0].Trim();

                // 1) Log a archivo (Serilog) con stack completo
                _logger.LogError(ex, "[MIDDLEWARE] TraceID: {TraceId} - Unhandled exception en {Route}: {Message}",
                    traceId, fullRoute, baseMsg);

                // 2) Intento de guardar en DB (nunca relanzar si falla el logger)
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var logHelper = scope.ServiceProvider.GetRequiredService<LogHelper>();

                    await logHelper.SaveLogAsync(
                        traceId: traceId,
                        message: baseMsg,            // mensaje limpio
                        exception: ex.ToString(),    // si no quieres stack en DB, pon null
                        level: "Error",
                        usuario: context.User?.Identity?.Name ?? "anonymous",
                        origen: nameof(GlobalExceptionMiddleware),
                        ruta: fullRoute
                    );
                }
                catch (Exception exDb)
                {
                    // Fallback: registra que falló el guardado en DB (no relanzar)
                    Serilog.Log.Error(exDb,
                        "[LOGHELPER] Falló guardando log en DB (se ignora). TraceId={TraceId} Ruta={Ruta}",
                        traceId, fullRoute);
                }

                // 3) Respuesta al cliente (si aún no empezó)
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers["X-Trace-Id"] = traceId;


                    //crea la respuesta en formato ApiResponse
                    var response = ApiResponseFactory.InternalError<object>(baseMsg, traceId);
                    //envía ese JSON al cliente (lo “retorna” por el socket HTTP).
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response), context.RequestAborted);
                }
                // Si ya empezó la respuesta, no se puede escribir cuerpo; ya quedó logueado.
            }
        }
    }
}

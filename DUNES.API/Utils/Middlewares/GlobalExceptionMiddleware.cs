using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using DUNES.API.Utils.Logging;
using DUNES.Shared.Utils.Reponse;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Utils.Middlewares
{
    /// <summary> Global exception catcher </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// Dependency injection
        /// </summary>
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            IServiceScopeFactory scopeFactory,
            ILogger<GlobalExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Ejecuta el siguiente middleware/controlador y, si falla,
        /// escribe un log ÚTIL (archivo + BD) y responde con JSON
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // 0) Correlación y datos base del request
                var traceId = GetCorrelationId(context);
                var method = context.Request?.Method ?? "N/A";
                var path = context.Request?.Path.ToString() ?? "/";
                var qs = context.Request?.QueryString.HasValue == true ? context.Request.QueryString.Value : string.Empty;
                var route = $"{method} {path}{qs}";
                var user = context.User?.Identity?.Name ?? "anonymous";
                var ip = context.Connection?.RemoteIpAddress?.ToString() ?? "N/A";

                // 1) Clasificar excepción y extraer lo esencial
                var detail = Classify(ex); // (Kind, Code, Table, Column, Message)

                // 2) Log a archivo: 
                //    - en Dev adjunta 'ex' (stack completo)
                //    - en Prod NO adjunta 'ex' (no imprime stack → log corto)
                if (_env.IsDevelopment())
                {
                    _logger.LogError(ex,
                        "[MIDDLEWARE] Unhandled exception | TraceId={TraceId} | Route={Route} | User={User} | IP={IP} | Kind={Kind} | Code={Code} | Table={Table} | Column={Column} | Message={Message}",
                        traceId, route, user, ip, detail.Kind, detail.Code, detail.Table, detail.Column, detail.Message);
                }
                else
                {
                    _logger.LogError(
                        "[MIDDLEWARE] Unhandled exception | TraceId={TraceId} | Route={Route} | User={User} | IP={IP} | Kind={Kind} | Code={Code} | Table={Table} | Column={Column} | Message={Message}",
                        traceId, route, user, ip, detail.Kind, detail.Code, detail.Table, detail.Column, detail.Message);
                }

                // 3) Persistencia del resumen en BD (stack solo en Dev, truncado)
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var logHelper = scope.ServiceProvider.GetRequiredService<LogHelper>();

                    var exceptionText = _env.IsDevelopment()
                        ? Truncate(ex.ToString(), 4000)   // stack en Dev
                        : null;                          // en Prod no guardamos stack

                    await logHelper.SaveLogAsync(
                        traceId: traceId,
                        message: Truncate(detail.Message, 500),
                        exception: exceptionText,
                        level: "Error",
                        usuario: user,
                        origen: nameof(GlobalExceptionMiddleware),
                        ruta: route
                    );
                }
                catch (Exception exDb)
                {
                    // Fallback: no relanzar; solo informar que no se pudo guardar el log en BD
                    Serilog.Log.Error(exDb,
                        "[LOGHELPER] Falló guardando log en DB (se ignora). TraceId={TraceId} Ruta={Ruta}",
                        traceId, route);
                }

                // 4) Respuesta al cliente con TraceId (segura en Prod)
                if (!context.Response.HasStarted)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    context.Response.Headers["X-Trace-Id"] = traceId;

                    var safeMsg = _env.IsDevelopment()
                        ? detail.Message
                        : "Ocurrió un error inesperado. Usa el TraceId para seguimiento.";

                    var response = ApiResponseFactory.InternalError<object>(safeMsg, traceId);
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response), context.RequestAborted);
                }
                // Si ya empezó la respuesta: no se puede escribir cuerpo; el evento ya quedó logueado.
            }
        }

        private static string GetCorrelationId(HttpContext ctx)
        {
            // 1) Del header entrante si existe
            if (ctx.Request.Headers.TryGetValue("X-Trace-Id", out var h) && !string.IsNullOrWhiteSpace(h))
                return h.ToString();

            // 2) Del TraceIdentifier del pipeline
            if (!string.IsNullOrWhiteSpace(ctx.TraceIdentifier))
                return ctx.TraceIdentifier;

            // 3) Fallback
            return Guid.NewGuid().ToString("N");
        }

        private static (string Kind, int? Code, string? Table, string? Column, string Message) Classify(Exception ex)
        {
            // Valores por defecto (mensaje base sin saltos de línea)
            var kind = ex.GetType().Name;
            var message = ex.GetBaseException().Message.Split('\n')[0].Trim();
            int? code = null;
            string? table = null;
            string? column = null;

            // Casos EF/SQL: ejemplo tuyo => SqlException 207 (Invalid column name 'X')
            if (ex is DbUpdateException dbu && dbu.InnerException is SqlException sql)
            {
                kind = "SqlException/DbUpdateException";
                code = sql.Number; // 207 => Invalid column name

                // Extrae el nombre de la columna si aparece
                var m = Regex.Match(sql.Message, @"Invalid column name '([^']+)'", RegexOptions.IgnoreCase);
                if (m.Success) column = m.Groups[1].Value;

                // (Opcional) Si tus mensajes incluyen el nombre de la tabla, puedes extraerlo:
                // var t = Regex.Match(sql.Message, @"object '([^']+)'", RegexOptions.IgnoreCase);
                // if (t.Success) table = t.Groups[1].Value;

                message = sql.Message.Split('\n')[0].Trim();
            }
            else if (ex is DbUpdateException dbx)
            {
                kind = "DbUpdateException";
                message = dbx.GetBaseException().Message.Split('\n')[0].Trim();
            }

            return (kind, code, table, column, message);
        }

        private static string Truncate(string? s, int max)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            return s.Length <= max ? s : s.Substring(0, max);
        }
    }
}

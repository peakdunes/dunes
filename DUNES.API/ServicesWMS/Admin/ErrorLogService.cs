using DUNES.API.Utils.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DUNES.API.ServicesWMS.Admin
{
    /// <summary>
    /// save error (try - catch) in database table
    /// </summary>
    public class ErrorLogService : IErrorLogService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHostEnvironment _env;

        /// <summary>
        /// constructor(DI)
        /// </summary>
        /// <param name="scopeFactory"></param>
        /// <param name="env"></param>
        public ErrorLogService(IServiceScopeFactory scopeFactory, IHostEnvironment env)
        {
            _scopeFactory = scopeFactory;
            _env = env;
        }
        /// <summary>
        /// save catch error
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ex"></param>
        /// <param name="origin"></param>
        /// <param name="overrideMessage"></param>
        /// <returns></returns>
        public async Task TrySaveAsync(HttpContext context, Exception ex, string origin, string? overrideMessage = null)
        {
            try
            {
                var traceId = GetTraceId(context);
                var route = $"{context.Request?.Method ?? "N/A"} {context.Request?.Path}{context.Request?.QueryString}";
                var user = context.User?.Identity?.Name ?? "anonymous";

                var detail = Classify(ex);
                var msg = overrideMessage ?? detail.Message;

                using var scope = _scopeFactory.CreateScope();
                var logHelper = scope.ServiceProvider.GetRequiredService<LogHelper>();

                var exceptionText = _env.IsDevelopment()
                    ? Truncate(ex.ToString(), 4000)
                    : null;

                // Si quieres "toda la info posible", puedes meter Kind/Code/Table/Column dentro del message.
                var richMessage = $"{msg} | Kind={detail.Kind} Code={detail.Code} Table={detail.Table} Column={detail.Column}";

                await logHelper.SaveLogAsync(
                    traceId: traceId,
                    message: Truncate(richMessage, 500),
                    exception: exceptionText,
                    level: "Error",
                    usuario: user,
                    origen: origin,
                    ruta: route
                );
            }
            catch
            {
                // IMPORTANT: Nunca rompas el response por fallo de logging
                // Si quieres, aquí puedes hacer Serilog.Log.Error(exDb, ...) pero no relanzar.
            }
        }

        private static string GetTraceId(HttpContext ctx)
        {
            if (ctx.Items.TryGetValue("TraceId", out var t) && t is string s && !string.IsNullOrWhiteSpace(s))
                return s;

            if (ctx.Request.Headers.TryGetValue("X-Trace-Id", out var h) && !string.IsNullOrWhiteSpace(h))
                return h.ToString();

            return !string.IsNullOrWhiteSpace(ctx.TraceIdentifier)
                ? ctx.TraceIdentifier
                : Guid.NewGuid().ToString("N");
        }

        private static (string Kind, int? Code, string? Table, string? Column, string Message) Classify(Exception ex)
        {
            var kind = ex.GetType().Name;
            var message = ex.GetBaseException().Message.Split('\n')[0].Trim();
            int? code = null;
            string? table = null;
            string? column = null;

            if (ex is DbUpdateException dbu && dbu.InnerException is SqlException sql)
            {
                kind = "SqlException/DbUpdateException";
                code = sql.Number;

                var m = Regex.Match(sql.Message, @"Invalid column name '([^']+)'", RegexOptions.IgnoreCase);
                if (m.Success) column = m.Groups[1].Value;

                message = sql.Message.Split('\n')[0].Trim();
            }

            return (kind, code, table, column, message);
        }

        private static string Truncate(string? s, int max)
            => string.IsNullOrEmpty(s) ? string.Empty : (s.Length <= max ? s : s.Substring(0, max));
    }
}

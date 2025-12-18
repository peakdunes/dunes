using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.Interfaces.AuditContext
{


    /// <summary>
    /// Provides audit metadata for the current HTTP request (who/when/where).
    /// This context is used by EF Core interceptors (e.g., <c>AuditSaveChangesInterceptor</c>)
    /// to populate <c>AuditLog</c> fields such as <c>UserName</c>, <c>TraceId</c>, <c>IpAddress</c>,
    /// <c>AppName</c>, and a request-scoped <c>CorrelationId</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The implementation typically reads values from <see cref="Microsoft.AspNetCore.Http.HttpContext"/>:
    /// </para>
    /// <list type="bullet">
    ///   <item><description><c>UserName</c>: from the authenticated principal (claims/identity).</description></item>
    ///   <item><description><c>TraceId</c>: from <see cref="Microsoft.AspNetCore.Http.HttpContext.TraceIdentifier"/>.</description></item>
    ///   <item><description><c>IpAddress</c>: from <see cref="Microsoft.AspNetCore.Http.ConnectionInfo.RemoteIpAddress"/> (or forwarded headers if enabled).</description></item>
    ///   <item><description><c>CorrelationId</c>: stored in <c>HttpContext.Items</c> to remain stable for the entire request.</description></item>
    /// </list>
    /// <para>
    /// Note: This component does not persist logs; it only supplies metadata to be persisted by the audit pipeline.
    /// </para>
    /// </remarks>
    public sealed class AuditContext : IAuditContext
    {
        public string? UserName { get; private set; }
        public string? TraceId { get; private set; }
        public string? IpAddress { get; private set; }
        public string? AppName { get; private set; }
        public Guid CorrelationId { get; private set; }

        public AuditContext(IHttpContextAccessor http)
        {
            var ctx = http.HttpContext;

            UserName = ctx?.User?.Identity?.Name
                ?? ctx?.User?.FindFirst(ClaimTypes.Name)?.Value
                ?? "Anonymous";

            TraceId = ctx?.TraceIdentifier;

            IpAddress = ctx?.Connection?.RemoteIpAddress?.ToString();

            AppName = "DUNES.API";

            // 1 CorrelationId por request (si no existe, se crea)
            if (ctx != null)
            {
                const string key = "CorrelationId";
                if (ctx.Items.TryGetValue(key, out var val) && val is Guid g)
                    CorrelationId = g;
                else
                {
                    CorrelationId = Guid.NewGuid();
                    ctx.Items[key] = CorrelationId;
                }
            }
            else
            {
                CorrelationId = Guid.NewGuid();
            }
        }
    }
}

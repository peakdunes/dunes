using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.Interfaces.AuditContext
{
    public interface IAuditContext
    {
        string? UserName { get; }
        string? TraceId { get; }
        string? IpAddress { get; }
        string? AppName { get; }
        Guid CorrelationId { get; }
    }
}

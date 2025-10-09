using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.Interfaces.RequestInfo
{

    /// <summary>
    /// Obtains HTTP transaction information to log
    /// </summary>
    public interface IRequestInfo
    {
        string? Path { get; set; }
        string? Method { get; set; }
        string? Query { get; set; }
    }
}

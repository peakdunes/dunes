using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.Interfaces.RequestInfo
{
    public class RequestInfo : IRequestInfo
    {
        public string? Path { get; set; }
        public string? Method { get; set; }
        public string? Query { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Masters
{
    public class TzebFaultCodesDto
    {
        public string FaultCode { get; set; } = null!;

        public string? FaultDesc { get; set; }

        public string? FaultCodeDefinition { get; set; }

    }
}

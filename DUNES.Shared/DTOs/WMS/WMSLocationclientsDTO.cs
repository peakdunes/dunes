using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    public class WMSLocationclientsDTO
    {
        public int Idlocation { get; set; }

        public string? Idcompanyclient { get; set; }

        public bool Active { get; set; }
    }
}

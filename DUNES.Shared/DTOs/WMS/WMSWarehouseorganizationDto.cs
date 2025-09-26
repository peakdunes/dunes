using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.WMS
{
    /// <summary>
    /// Warehouse organization DTP
    /// </summary>

    public class WMSWarehouseorganizationDto
    {
        public int Id { get; set; }

        public int Idcompany { get; set; }

        public string? Idcompanyclient { get; set; }

        public string? Iddivision { get; set; }

        public int Idlocation { get; set; }

        public int Idrack { get; set; }

        public int Level { get; set; }

        public int Idbin { get; set; }
    }
}

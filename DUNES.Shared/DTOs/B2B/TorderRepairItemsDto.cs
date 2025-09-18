using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.B2B
{
    public class TorderRepairItemsDto
    {
        public int RefNo { get; set; }

        public string PartNo { get; set; } = null!;

        public int? Qty { get; set; }

        public string? CompanyPartNo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.B2B
{
    public class TorderRepairItemsSerialsReceivingDto
    {
        public int RefNo { get; set; }

        public string PartNo { get; set; } = null!;

        public DateTime? DateReceived { get; set; }

        public string? SerialInbound { get; set; }

        public string? SerialReceived { get; set; }

        public string? TstatusId { get; set; }
        
        public int RepairNo { get; set; }

        public int? Qty { get; set; }

        public int? QtyReceived { get; set; }
               
        public int Id { get; set; }

        public string? ProjectName { get; set; }

    }
}

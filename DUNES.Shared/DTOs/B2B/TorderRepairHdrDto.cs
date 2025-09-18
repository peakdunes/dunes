using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.B2B
{
    public class TorderRepairHdrDto
    {
        public int RefNo { get; set; }

        public DateTime? DateCreated { get; set; }

        public string TcustNo { get; set; } = null!;

        public string? CustRef { get; set; }

        public string? CustName { get; set; }

        public string? ShipToAddr { get; set; }

        public string? ShipToAddr1 { get; set; }

        public int? TcityId { get; set; }

        public string? TstateId { get; set; }

        public string? ZipCode { get; set; }

        public string? TstatusId { get; set; }
                
        public DateTime? DateInserted { get; set; }
                

    }
}

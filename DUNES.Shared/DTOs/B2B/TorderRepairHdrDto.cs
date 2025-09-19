using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.B2B
{
    public class TorderRepairHdrDto
    {
        [Display(Name ="Reference Number")]
        public int RefNo { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "Cust Number")]
        public string TcustNo { get; set; } = null!;

        [Display(Name = "Cust Reference")]
        public string? CustRef { get; set; }

        [Display(Name = "Customer Name")]
        public string? CustName { get; set; }

        [Display(Name = "Ship to Address")]
        public string? ShipToAddr { get; set; }

        [Display(Name = "Shit to Address")]
        public string? ShipToAddr1 { get; set; }

        [Display(Name = "City")]
        public int? TcityId { get; set; }
        [Display(Name = "State")]
        public string? TstateId { get; set; }
        [Display(Name = "Zip Code")]
        public string? ZipCode { get; set; }
        [Display(Name = "Status Id")]
        public string? TstatusId { get; set; }
        [Display(Name = "Date Inserted")]
        public DateTime? DateInserted { get; set; }
                

    }
}

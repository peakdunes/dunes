using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class ASNHdrDto
    {
        [Display(Name = "ASN Id")]
        public int Id { get; set; }

        [Display(Name = "Request Id")]
        public int ConsignRequestID { get; set; }

        [Display(Name = "Batch Id")]
        [MaxLength(100)]
        public string? BatchId { get; set; }

        [Display(Name = "Shipment Number")]
        [MaxLength(100)]
        public string? ShipmentNum { get; set; }

        [Display(Name = "Location")]
        public int ShipToLocationId { get; set; }

        [Display(Name = "Date Inserted")]
        public DateTime DateTimeInserted { get; set; }

        [Display(Name = "Processed")]
        public bool Processed { get; set; }
    }
}

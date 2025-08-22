using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class ASNDto
    {
        public required ASNHdr asnHdr { get; set; }

        public required List<ASNItemDetail> itemDetail { get; set; }

    }


    public class ASNHdr
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

    public class ASNItemDetail
    {
        [Display(Name ="Id")]
        public int Id { get; set; }

        [Display(Name = "Header Number")]
        public int ADNHdrId { get; set; }

        [MaxLength(50)]
        [Display(Name = "Part Number")]
        public string? ItemNumber { get; set; }

        [Display(Name = "Line ID")]
        public int LineId { get; set; }

        [Display(Name = "Quantity Shipped")]
        public int QuantityShipped { get; set; }


        [Display(Name = "Quantity Pending")]
        public int QuantityPending { get; set; }


        [MaxLength(200)]
        [Display(Name = "Item Description")]
        public string? ItemDescription { get; set; }

        [MaxLength(50)]
        [Display(Name = "Attribute 2")]
        public string? Attributte2 { get; set; }

        [Display(Name = "Quantity Received")]
        public int QuantityReceived { get; set; }

        [Display(Name = "Date Time Inserted")]
        public DateTime DateTimeInserted { get; set; }

    }
}

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
        public int Id { get; set; }

        public int ConsignRequestID { get; set; }

        [MaxLength(100)]
        public string? BatchId { get; set; }

        [MaxLength(100)]
        public string? ShipmentNum { get; set; }

        public int ShipToLocationId { get; set; }

        public DateTime DateTimeInserted { get; set; }
        
        public bool Processed { get; set; }

    }

    public class ASNItemDetail
    {

        public int Id { get; set; }

        public int ADNHdrId { get; set; }

        [MaxLength(50)]
        public string? ItemNumber { get; set; }

        public int LineId { get; set; }

        public int QuantityShipped { get; set; }

        [MaxLength(200)]
        public string? ItemDescription { get; set; }


        public int QuantityReceived { get; set; }

        public DateTime DateTimeInserted { get; set; }

    }
}

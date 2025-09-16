using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class PickProcessItemDetail
    {
        public int Id { get; set; }

        public int idPickProcessHdr { get; set; }

        [MaxLength(20)]
        public string? LindId { get; set; }

        [MaxLength(50)]
        public string? ItemNumber { get; set; }

        [MaxLength(200)]
        public string? ItemDescription { get; set; }

        [MaxLength(50)]
        public int RequestQuantity { get; set; }

        [MaxLength(50)]
        public int QuantityProcess { get; set; }

        [MaxLength(200)]
        public string? Frm3plLocatorStatus { get; set; }

        [MaxLength(200)]
        public string? PickLPN { get; set; }


        public DateTime DateTimeInserted { get; set; }

        public int QtyOnHand { get; set; }
    }
}

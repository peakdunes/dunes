using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class TzebB2bIrReceiptOutHdrDetItemInbConsReqsLogDto
    {
        [Display(Name = "Transaction Id")]
        public int Id { get; set; }

        [Display(Name ="Request Id")]
        public int ConsignDbkrequestId { get; set; }
        [Display(Name = "3PL")]
        public string OrgSystemId3pl { get; set; } = null!;
        [Display(Name = "Transaction Type")]
        public string TransactionType { get; set; } = null!;
        [Display(Name = "Shipment Number")]
        public string ShipmentNum { get; set; } = null!;
        [Display(Name = "Date Inserted")]
        public DateTime DateTimeInserted { get; set; }
    }
}

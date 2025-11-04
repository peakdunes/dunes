using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUNES.Shared.DTOs.Inventory
{
    public class TzebB2bIrReceiptLineItemTblItemInbConsReqsLogDto
    {
        public int Id { get; set; }

        public int? IrReceiptOutHdrDetItemId { get; set; }

        public float ShipmentLineId { get; set; }

        public float LineNum { get; set; }

        public float Quantity { get; set; }

        public string? UnitOfMeasure { get; set; }

        public float InventoryItemId { get; set; }

        public string ItemNumber { get; set; } = null!;

        public DateOnly ReceiptDate { get; set; }

        public DateOnly TransactionDate { get; set; }

        public DateTime DateTimeInserted { get; set; }

        public string To3plLocatorStatus { get; set; } = null!;

        public bool? IsRtvPart { get; set; }

        public bool? IsCePart { get; set; }
    }
}

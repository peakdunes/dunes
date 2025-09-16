using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory;

public partial class TzebB2bReplacementPartsInventoryLog
{
    public int Id { get; set; }

    public int PartDefinitionId { get; set; }

    public int InventoryTypeIdSource { get; set; }

    public int InventoryTypeIdDest { get; set; }

    public string? SerialNo { get; set; }

    public int Qty { get; set; }

    public string? Notes { get; set; }

    public int? RepairNo { get; set; }

    public DateTime DateInserted { get; set; }
}

using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory.Common;


/// <summary>
/// Inventory transactions logs
/// </summary>
public partial class TzebB2bReplacementPartsInventoryLog
{

    /// <summary>
    /// Record ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Part Number ID
    /// </summary>
    public int PartDefinitionId { get; set; }
    /// <summary>
    /// Inventory Type ID Source
    /// </summary>
    public int InventoryTypeIdSource { get; set; }
    /// <summary>
    /// Inventory Type ID Dest
    /// </summary>
    public int InventoryTypeIdDest { get; set; }
    /// <summary>
    /// Part Number serial id
    /// </summary>
    public string? SerialNo { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    public int Qty { get; set; }
    /// <summary>
    /// Notes
    /// </summary>
    public string? Notes { get; set; }
    /// <summary>
    /// Repair Number 
    /// </summary>
    public int? RepairNo { get; set; }
    /// <summary>
    /// Date Inserted
    /// </summary>
    public DateTime DateInserted { get; set; }
}

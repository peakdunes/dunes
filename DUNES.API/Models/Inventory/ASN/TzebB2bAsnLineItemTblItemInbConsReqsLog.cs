using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory.ASN;

/// <summary>
/// ASN Logs Process items detail
/// </summary>

public partial class TzebB2bAsnLineItemTblItemInbConsReqsLog
{
    public int Id { get; set; }

    public int? AsnOutHdrDetItemId { get; set; }

    public int? AsnLineItemTblItemId { get; set; }

    public float ShipmentLineId { get; set; }

    public string? WipJobName { get; set; }

    public string? ReconfiguredAssembly { get; set; }

    public float LineNum { get; set; }

    public float QuantityShipped { get; set; }

    public string? UnitOfMeasure { get; set; }

    public float InventoryItemId { get; set; }

    public string ItemNumber { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public string? HazardClass { get; set; }

    public string? UnNumber { get; set; }

    public string? TruckNum { get; set; }

    public string? ContainerNum { get; set; }

    public float? QuantityReceived { get; set; }

    public string OrderType { get; set; } = null!;

    public float OrderNum { get; set; }

    public float OrderLineId { get; set; }

    public string? PoShipmentNumber { get; set; }

    public string ShipmentLineStatusCode { get; set; } = null!;

    public string Comments { get; set; } = null!;

    public string? ReasonCode { get; set; }

    public string ReceiptRouting { get; set; } = null!;

    public string? OrganizationCode { get; set; }

    public string? Locator { get; set; }

    public float? TransactionId { get; set; }

    public float? TransactionQuantity { get; set; }

    public DateOnly? DateCompleted { get; set; }

    public string? SubinventoryCode { get; set; }

    public string? Attribute1 { get; set; }

    public string? Attribute2 { get; set; }

    public string? Attribute3 { get; set; }

    public string? Attribute4 { get; set; }

    public string? Attribute5 { get; set; }

    public string? Attribute6 { get; set; }

    public string? Attribute7 { get; set; }

    public string? Attribute8 { get; set; }

    public string? Attribute9 { get; set; }

    public string? Attribute10 { get; set; }

    public string? Attribute11 { get; set; }

    public string? Attribute12 { get; set; }

    public string? Attribute13 { get; set; }

    public string? Attribute14 { get; set; }

    public string? Attribute15 { get; set; }

    public string? LpnInfo { get; set; }

    public DateTime DateTimeInserted { get; set; }
}

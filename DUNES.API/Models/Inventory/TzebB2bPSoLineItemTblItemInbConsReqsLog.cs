using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory;

public partial class TzebB2bPSoLineItemTblItemInbConsReqsLog
{
    public int Id { get; set; }

    public int PSoWoHdrTblItemId { get; set; }

    public string LineId { get; set; } = null!;

    public string ItemNumber { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public string? AltLangItemDescription { get; set; }

    public string InventoryItemId { get; set; } = null!;

    public string DeliveryDetailId { get; set; } = null!;

    public string RequestedQuantity { get; set; } = null!;

    public string RequestedQuantityUom { get; set; } = null!;

    public string? OrderQuantity { get; set; }

    public string OrderingUom { get; set; } = null!;

    public string? ConversionRate { get; set; }

    public string MoveOrderNumber { get; set; } = null!;

    public string MoveOrderLineId { get; set; } = null!;

    public string? WipJobName { get; set; }

    public string SalesOrderNumber { get; set; } = null!;

    public string OrderLineNumber { get; set; } = null!;

    public string TransactionTempId { get; set; } = null!;

    public string? CustomerPoNumber { get; set; }

    public string? EndUserPoNumber { get; set; }

    public string? FairMarketValue { get; set; }

    public string? HtsCode { get; set; }

    public string? HtsDescription { get; set; }

    public string? UnCode { get; set; }

    public string? Haz { get; set; }

    public string? Sub { get; set; }

    public string? Nfmc { get; set; }

    public string? Class { get; set; }

    public string? HazardClass { get; set; }

    public string? PortOfDestination { get; set; }

    public string? PortOfLoading { get; set; }

    public string? FinalDestination { get; set; }

    public string? Incoterms { get; set; }

    public string? BlindPaperwork { get; set; }

    public string? CustomerSuppliedPaperwork { get; set; }

    public string? CooCAttribute1 { get; set; }

    public DateOnly DateScheduled { get; set; }

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

    public string Frm3plLocatorStatus { get; set; } = null!;

    public string? Frm3plLocator { get; set; }

    public string? Frm3plSubinv { get; set; }

    public string? Frm3plAttribute1 { get; set; }

    public string? Frm3plAttribute2 { get; set; }

    public string? Frm3plAttribute3 { get; set; }

    public string? Frm3plAttribute4 { get; set; }

    public string? Frm3plAttribute5 { get; set; }

    public string? Frm3plAttribute6 { get; set; }

    public string? Frm3plAttribute7 { get; set; }

    public string? Frm3plAttribute8 { get; set; }

    public DateOnly CreationDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateOnly LastUpdateDate { get; set; }

    public string LastUpdatedBy { get; set; } = null!;

    public string? ProcessStatus { get; set; }

    public string? ErrorCode { get; set; }

    public string? ErrorMessage { get; set; }

    public DateOnly SalesOrderDate { get; set; }

    public string? Requestor { get; set; }

    public string? ShipToContact { get; set; }

    public string? CustomerPartNumber { get; set; }

    public string? PtoParentItem { get; set; }

    public string? TopModelLineId { get; set; }

    public string? PtoCompUnitQty { get; set; }

    public DateTime DateTimeInserted { get; set; }

    public DateTime? DateTimeProcessed { get; set; }

    public int QtyOnHand { get; set; }

    public string? PickLpn { get; set; }
}

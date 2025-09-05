using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory;


/// <summary>
/// ASN Head process
/// </summary>
public partial class TzebB2bAsnOutHdrDetItemInbConsReqs
{
    public int Id { get; set; }

    public int ConsignRequestId { get; set; }

    public string? BatchId { get; set; }

    public string OrgSystemId3pl { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public float ShipmentHeaderId { get; set; }

    public string ShipmentNum { get; set; } = null!;

    public float? VendorId { get; set; }

    public string? WaybillAirbillNum { get; set; }

    public string? FreightCarrierCode { get; set; }

    public DateOnly ExpectedReceiptDate { get; set; }

    public string? BillOfLading { get; set; }

    public string? PackingSlip { get; set; }

    public float ShipToLocationId { get; set; }

    public float? NumOfContainers { get; set; }

    public string? Comments { get; set; }

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

    public string? CarrierMethod { get; set; }

    public string OrganizationCode { get; set; } = null!;

    public string? VendorName { get; set; }

    public string? VendorSiteCode { get; set; }

    public string? ReceiptSourceCode { get; set; }

    public DateTime DateTimeInserted { get; set; }

    public DateTime DateTimeUpdated { get; set; }

    public bool Processed { get; set; }
}

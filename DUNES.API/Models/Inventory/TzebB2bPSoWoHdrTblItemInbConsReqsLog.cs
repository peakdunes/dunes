using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory;

public partial class TzebB2bPSoWoHdrTblItemInbConsReqsLog
{
    public int Id { get; set; }

    public int ConsignRequestId { get; set; }

    public string? BatchId { get; set; }

    public string? HeaderId { get; set; }

    public string? DeliveryId { get; set; }

    public string OrgSystemId3pl { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public string Operation { get; set; } = null!;

    public string? ConsigneeName { get; set; }

    public string? FreightAccountNumber { get; set; }

    public string ShipToAddress1 { get; set; } = null!;

    public string? ShipToAddress2 { get; set; }

    public string? ShipToAddress3 { get; set; }

    public string? ShipToAddress4 { get; set; }

    public string ShipToCountry { get; set; } = null!;

    public string? ShipToCounty { get; set; }

    public string? ShipToCity { get; set; }

    public string? ShipToProvince { get; set; }

    public string? ShipToState { get; set; }

    public string? ShipToPostalCode { get; set; }

    public string CustomerName { get; set; } = null!;

    public DateOnly CreationDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateOnly LastUpdateDate { get; set; }

    public string LastUpdatedBy { get; set; } = null!;

    public string? Addressee { get; set; }

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

    public string? ProcessStatus { get; set; }

    public string? ErrorCode { get; set; }

    public string? ErrorMesagge { get; set; }

    public string Carrier { get; set; } = null!;

    public string ServiceLevel { get; set; } = null!;

    public string ModeOfTransport { get; set; } = null!;

    public string ShipMethod { get; set; } = null!;

    public string FreightTerms { get; set; } = null!;

    public string? PackSlipNumber { get; set; }

    public string CustomerNumber { get; set; } = null!;

    public string BillToAddress1 { get; set; } = null!;

    public string? BillToAddress2 { get; set; }

    public string? BillToAddress3 { get; set; }

    public string? BillToAddress4 { get; set; }

    public string BillToCountry { get; set; } = null!;

    public string? BillToCounty { get; set; }

    public string? BillToCity { get; set; }

    public string? BillToProvince { get; set; }

    public string? BillToState { get; set; }

    public string? BillToPostalCode { get; set; }

    public string? PortOfLoading { get; set; }

    public string? PortOfDestination { get; set; }

    public string? FinalDestination { get; set; }

    public string? ShippingPoint { get; set; }

    public string? IncoTerms { get; set; }

    public DateTime DateTimeInserted { get; set; }

    public DateTime? DateTimeProcessed { get; set; }

    public int? OutConsReqsId { get; set; }

    public DateTime? DateTimeConfirmed { get; set; }

    public DateTime? DateTimeOnlineOrders { get; set; }

    public DateTime? DateTimeError { get; set; }

    public string? ErrorMsg { get; set; }

    public string? OnlineOrdersErrorMsg { get; set; }

    public DateTime? DateTimeOnlineOrdersError { get; set; }

    public int? ShipOutConsReqsId { get; set; }

    public DateTime? ShipDateTimeConfirmed { get; set; }

    public string? ShipErrorMsg { get; set; }

    public DateTime? ShipDateTimeError { get; set; }
}

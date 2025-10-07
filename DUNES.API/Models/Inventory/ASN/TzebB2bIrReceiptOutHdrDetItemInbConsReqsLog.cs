using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory.ASN;

public partial class TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog
{
    public int Id { get; set; }

    public int ConsignDbkrequestId { get; set; }

    public string OrgSystemId3pl { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public string ShipmentNum { get; set; } = null!;

    public DateTime DateTimeInserted { get; set; }
}

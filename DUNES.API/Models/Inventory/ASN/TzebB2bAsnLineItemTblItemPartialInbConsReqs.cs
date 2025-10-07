using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory.ASN;

public partial class TzebB2bAsnLineItemTblItemPartialInbConsReqs
{
    public int Id { get; set; }

    public int AsnLineItemTblItemId { get; set; }

    public int QtyPartial { get; set; }

    public DateTime DateTimeSent { get; set; }

    public int? CallId { get; set; }

    public string? Username { get; set; }
}

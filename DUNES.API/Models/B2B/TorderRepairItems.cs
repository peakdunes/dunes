using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2b;

public partial class TorderRepairItems
{
    public int RefNo { get; set; }

    public string PartNo { get; set; } = null!;

    public int? Qty { get; set; }

    public string? CompanyPartNo { get; set; }

    public virtual TorderRepairHdr RefNoNavigation { get; set; } = null!;
}

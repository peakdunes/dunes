using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class TzebRepairCodesPartNo
{
    public int RepId { get; set; }

    public string PartNo { get; set; } = null!;

    public int? Qty { get; set; }

    public decimal? Cost { get; set; }
}

using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class TzebInBoundRequestsFileHoldsLog
{
    public int Id { get; set; }

    public string RowId { get; set; } = null!;

    public string HoldId { get; set; } = null!;

    public string? HoldType { get; set; }

    public string? HoldName { get; set; }

    public DateTime? DateOnHold { get; set; }

    public DateTime? DateReleased { get; set; }
}

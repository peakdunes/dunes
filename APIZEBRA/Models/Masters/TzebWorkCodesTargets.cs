using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.Masters;

public partial class TzebWorkCodesTargets
{
    public string WorkCodeTarget { get; set; } = null!;

    public string? WorkDescTarget { get; set; }

    public bool? Show { get; set; }

    public bool? ConsideredForBer { get; set; }
}

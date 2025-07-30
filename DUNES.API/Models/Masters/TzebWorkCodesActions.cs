using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

public partial class TzebWorkCodesActions
{
    public string WorkCodeAction { get; set; } = null!;

    public string? WorkDescAction { get; set; }

    public string? RepairCodeDefinition { get; set; }

    public bool? RequiresAssemblingArea { get; set; }

    public bool? Show { get; set; }

    public bool? RequiresPartsReplaced { get; set; }
}

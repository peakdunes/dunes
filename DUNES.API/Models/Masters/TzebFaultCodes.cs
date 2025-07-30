using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

public partial class TzebFaultCodes
{
    public string FaultCode { get; set; } = null!;

    public string? FaultDesc { get; set; }

    public string? FaultCodeDefinition { get; set; }

    public string? Categorization { get; set; }

    public string? FaultCodeGroup { get; set; }

    public string? ProductGroup { get; set; }

    public bool? Show { get; set; }

    public DateTime? DateInserted { get; set; }
}

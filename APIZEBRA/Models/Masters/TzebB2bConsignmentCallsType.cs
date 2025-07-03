using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.Masters;

public partial class TzebB2bConsignmentCallsType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Code { get; set; } = null!;

    public string? Attr1 { get; set; }

    public string? Attr2 { get; set; }

    public string? DocNumPrefix { get; set; }

    public bool ManualReq { get; set; }
}

using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.Masters;

public partial class TzebB2bOutBoundRequestsTypeOfCalls
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Code { get; set; }
}

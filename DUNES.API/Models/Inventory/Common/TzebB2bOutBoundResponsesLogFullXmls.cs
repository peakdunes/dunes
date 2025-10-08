using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory.Common;

public partial class TzebB2bOutBoundResponsesLogFullXmls
{
    public int Id { get; set; }

    public string FullXml { get; set; } = null!;

    public DateTime DateTimeInserted { get; set; }
}

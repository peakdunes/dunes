using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class MvcChangeAreaLogs
{
    public int Id { get; set; }

    public string? ActualArea { get; set; }

    public DateTime ChangeDate { get; set; }

    public string? UserChange { get; set; }

    public bool Lastchange { get; set; }

    public int Repairid { get; set; }
}

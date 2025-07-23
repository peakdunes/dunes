using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class MvcRepairPreflash
{
    public int Id { get; set; }

    public int Repairid { get; set; }

    public string? User { get; set; }

    public DateTime Datereceive { get; set; }

    public DateTime Dateprocess { get; set; }

    public int Techprevious { get; set; }

    public bool? TechFingerprint { get; set; }
}

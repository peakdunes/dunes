using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2B;

public partial class TzebB2bReplacedPartLabel
{
    public int Id { get; set; }

    public int RepairNo { get; set; }

    /// <summary>
    /// _TZEB_REPAIR_CODES_PART_NO - Rep_ID - To get the part associated to this one being removed
    /// </summary>
    public int? RepId { get; set; }

    public int TtechNo { get; set; }

    public string PartNo { get; set; } = null!;

    public string? SerialNo { get; set; }

    public int? Qty { get; set; }

    public string? PartRunner { get; set; }

    public string? ProblemDesc { get; set; }

    public string? RtvType { get; set; }

    public DateTime DateInserted { get; set; }

    public DateTime? DateLabelPrinted { get; set; }

    public DateTime? DateDcr1 { get; set; }
}

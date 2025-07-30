using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2b;

public partial class Trepair
{
    public int RepairNo { get; set; }

    public string? SerialNo { get; set; }

    public string? PartNo { get; set; }

    public int? RefNo { get; set; }

    public string? CompanyDsc { get; set; }

    public string? Division { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateTech { get; set; }

    public DateTime? DateRepaired { get; set; }

    public DateTime? DateCs { get; set; }

    public int? TtechNo { get; set; }

    public int? QualityEmployeeNo { get; set; }

    public float? RepairTime { get; set; }

    public int? NoRechazos { get; set; }

    public int? TrepairStateId { get; set; }

    public bool? AbuseActive { get; set; }

    public DateTime? DateLoading { get; set; }

    public DateTime? RealDateTimeCreated { get; set; }

    public DateTime? DateClose { get; set; }

    public string? WfpBin { get; set; }

    public double? InvoiceNo { get; set; }

    public string? DeviceConfigId { get; set; }

    /// <summary>
    /// In the case repair is put into a box or bin (ZEBRA)
    /// </summary>
    public string? RepairBoxNo { get; set; }

    /// <summary>
    /// Date supervisor removed work from technician
    /// </summary>
    public DateTime? DateWorkUnassigned { get; set; }
}

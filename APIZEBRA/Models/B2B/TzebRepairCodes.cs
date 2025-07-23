using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class TzebRepairCodes
{
    public int RepId { get; set; }

    public int? RepairNo { get; set; }

    public string? FaultCode { get; set; }

    public string? WorkCodeAction { get; set; }

    public string? WorkCodeTarget { get; set; }

    public int? TtechNo { get; set; }

    public DateTime? DateAdded { get; set; }

    /// <summary>
    /// Primary_Fault = TRUE means this is the main fault for device, the best describing the problem selected by customer   
    /// </summary>
    public bool? PrimaryFault { get; set; }

    public bool? Ber { get; set; }

    /// <summary>
    /// Date super tech click on SAVE AND EXIT, making request visible to part runners (if parts were requested)
    /// </summary>
    public DateTime? DateSubmitted { get; set; }
}

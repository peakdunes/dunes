using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIZEBRA.Models.B2B;

public partial class TzebRepairCodes
{
    /// <summary>
    /// primary key
    /// </summary>
    public int RepId { get; set; }
    /// <summary>
    /// Repair Number
    /// </summary>
    public int? RepairNo { get; set; }
    /// <summary>
    /// Diagnostic fault code
    /// </summary>
    public string? FaultCode { get; set; }
    /// <summary>
    /// Diagnostic action code
    /// </summary>
    public string? WorkCodeAction { get; set; }
    /// <summary>
    /// Diagnostic work activity
    /// </summary>
    public string? WorkCodeTarget { get; set; }
    /// <summary>
    /// technician id
    /// </summary>
    public int? TtechNo { get; set; }
    /// <summary>
    /// date created
    /// </summary>
    public DateTime? DateAdded { get; set; }

    /// <summary>
    /// Primary_Fault = TRUE means this is the main fault for device, the best describing the problem selected by customer   
    /// </summary>
    public bool? PrimaryFault { get; set; }

    /// <summary>
    /// true o false is BER
    /// </summary>
    public bool? Ber { get; set; }

    /// <summary>
    /// Date super tech click on SAVE AND EXIT, making request visible to part runners (if parts were requested)
    /// </summary>
    public DateTime? DateSubmitted { get; set; }


    /// <summary>
    /// Technician name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? codename { get; set; }

    /// <summary>
    /// action name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? actionname { get; set; }

    /// <summary>
    /// activity name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? workname { get; set; }
}

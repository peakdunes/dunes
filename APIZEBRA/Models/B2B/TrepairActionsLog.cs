using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIZEBRA.Models.B2B;

public partial class TrepairActionsLog
{
    /// <summary>
    /// primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// technician number
    /// </summary>
    public int? TtechNo { get; set; }
    /// <summary>
    /// repair number
    /// </summary>
    public int? RepairNo { get; set; }
    /// <summary>
    /// action code id
    /// </summary>
    public int? ActionId { get; set; }
    /// <summary>
    /// action date
    /// </summary>
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// action name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? actionName { get; set; }
}

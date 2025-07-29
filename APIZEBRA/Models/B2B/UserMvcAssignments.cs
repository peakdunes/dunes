using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace APIZEBRA.Models.B2B;
/// <summary>
/// Technician assigned to a repair
/// </summary>
public partial class UserMvcAssignments
{
    /// <summary>
    /// primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// repair number
    /// </summary>
    public int Repairid { get; set; }
    /// <summary>
    /// user asigned
    /// </summary>
    public string? Userassigned { get; set; }
    /// <summary>
    /// repair number actual area
    /// </summary>
    public string? ActualArea { get; set; }
    /// <summary>
    /// assigned date
    /// </summary>
    public DateTime Assigneddate { get; set; }
    /// <summary>
    /// who assigned
    /// </summary>
    public string? Userprocess { get; set; }
    /// <summary>
    /// true o false priority
    /// </summary>
    public bool? Bypass { get; set; }
    /// <summary>
    /// priority date
    /// </summary>
    public DateTime Bypassdate { get; set; }
    /// <summary>
    /// who change priority status
    /// </summary>
    public string? Userbypass { get; set; }
    /// <summary>
    /// Due date
    /// </summary>
    public DateTime Duedate { get; set; }
    /// <summary>
    /// true o false send alert to the technician
    /// </summary>
    public bool? Sendalert { get; set; }

    /// <summary>
    /// user received name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? username { get; set; }

}

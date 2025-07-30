using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.Models.B2B;
/// <summary>
/// User Preflahs information
/// </summary>
public partial class MvcRepairPreflash
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
    /// user who doing pre flash process
    /// </summary>
    public string? User { get; set; }
    /// <summary>
    /// date receive
    /// </summary>
    public DateTime Datereceive { get; set; }
    /// <summary>
    /// date process
    /// </summary>
    public DateTime Dateprocess { get; set; }
    /// <summary>
    /// previous preflahs user
    /// </summary>
    public int Techprevious { get; set; }
    /// <summary>
    /// finger print user
    /// </summary>
    public bool? TechFingerprint { get; set; }

    /// <summary>
    /// user received name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? username { get; set; }
}

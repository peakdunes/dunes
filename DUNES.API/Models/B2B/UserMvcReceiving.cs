using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.Models.B2B;
/// <summary>
/// receiving user process
/// </summary>
public partial class UserMvcReceiving
{
    /// <summary>
    /// primary key
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// repair number
    /// </summary>
    public int Repairno { get; set; }
    /// <summary>
    /// user who receive
    /// </summary>
    public string? User { get; set; }

    /// <summary>
    /// user received name
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? username { get; set; }
}

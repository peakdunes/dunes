using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;

/// <summary>
/// item status model
/// </summary>
public class Itemstatus
{

    /// <summary>
    /// database id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// status name
    /// </summary>
    [MaxLength(100)]
    public string? Name { get; set; }

    /// <summary>
    /// Company 
    /// </summary>
    [Required]
    [Display(Name = "Company")]
    public required int Idcompany { get; set; }


    /// <summary>
    /// observations
    /// </summary>
    /// 
    [MaxLength (1000)]
    public string? Observations { get; set; }


    /// <summary>
    /// is active
    /// </summary>
    public bool Active { get; set; }


    /// <summary>
    /// company navegation property
    /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;

  
}

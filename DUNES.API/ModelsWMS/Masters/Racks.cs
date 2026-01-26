using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;


/// <summary>
/// Racks Definition
/// </summary>
public partial class Racks
{
    /// <summary>
    /// internal DB ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// rack Name
    /// </summary>
    [Required]
    [Display(Name ="Rack Name")]
    public required string Name { get; set; }

    /// <summary>
    /// Company 
    /// </summary>
    [Required]
    [Display(Name = "Company")]
    public required int Idcompany { get; set; }

    /// <summary>
    /// location when this rack is
    /// </summary>
    [Required]
    [Display(Name = "Location")]
    public int LocationsId { get; set; }


    /// <summary>
    /// this rack is active
    /// </summary>
    [Display(Name = "Is Active")]
    public bool Active { get; set; }

  /// <summary>
  /// company navegation property
  /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;

    /// <summary>
    /// location navegation property
    /// </summary>
    public virtual Locations Locations { get; set; } = null!;

  
}

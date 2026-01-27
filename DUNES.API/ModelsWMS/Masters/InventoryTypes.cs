using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;


/// <summary>
/// inventory types
/// </summary>
public partial class InventoryTypes
{

    /// <summary>
    /// database id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// type name
    /// </summary>
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
    [MaxLength(1000)]
    public string? Observations { get; set; }

    /// <summary>
    /// it is inventory on hand?
    /// </summary>
    public bool IsOnHand { get; set; }


    /// <summary>
    /// is active
    /// </summary>
    public bool Active { get; set; }


    /// <summary>
    /// Interfaz with other system (ZEBRA)
    /// </summary>
    public int Zebrainvassociated { get; set; }


    /// <summary>
    /// company navegation property
    /// </summary>
        public virtual Company IdcompanyNavigation { get; set; } = null!;

  
}

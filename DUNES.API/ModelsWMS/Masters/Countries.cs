using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;


/// <summary>
/// WMS Countries
/// </summary>
public partial class Countries
{
    /// <summary>
    /// internal id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ISO Country Code
    /// </summary>
    [Required(ErrorMessage = "ISO Country Code is required.")]
    [MaxLength(5, ErrorMessage = "ISO Country Code cannot exceed 5 characters.")]
    [Display(Name = "ISO Country Code")]
    public string? Sigla { get; set; }

    /// <summary>
    /// country name
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    [Display(Name = "Country Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// navegacion cities
    /// </summary>
    public virtual ICollection<Cities> Cities { get; set; } = new List<Cities>();
    /// <summary>
    /// navegaction companines
    /// </summary>
    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    /// <summary>
    /// navegation locations
    /// </summary>
    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();

    /// <summary>
    /// navegation states 
    /// </summary>
    public virtual ICollection<StatesCountries> StatesCountries { get; set; } = new List<StatesCountries>();
}

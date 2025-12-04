using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// country states
/// </summary>
public partial class StatesCountries
{
    /// <summary>
    /// internal id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// country id
    /// </summary>
    /// 
    [Required(ErrorMessage = "Country is required.")]
    [Display(Name = "Country ID")]
    public int Idcountry { get; set; }

    /// <summary>
    /// state name
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    [Display(Name = "State Name")]
    public string? Name { get; set; }

    /// <summary>
    /// active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// ISO Country Code is required
    /// </summary>
    [Required(ErrorMessage = "ISO Country Code is required.")]
    [MaxLength(5, ErrorMessage = "ISO Country Code cannot exceed 5 characters.")]
    [Display(Name = "ISO Country Code")]
    public string? Sigla { get; set; }

    /// <summary>
    /// cities navegation
    /// </summary>
    public virtual ICollection<Cities> Cities { get; set; } = new List<Cities>();
    /// <summary>
    /// company navegation
    /// </summary>
    public virtual ICollection<Company> Company { get; set; } = new List<Company>();
    /// <summary>
    /// country navegation
    /// </summary>
    public virtual Countries IdcountryNavigation { get; set; } = null!;
    /// <summary>
    /// locations
    /// </summary>
    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();
}

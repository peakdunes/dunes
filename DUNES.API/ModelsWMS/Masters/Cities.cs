using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// cities
/// </summary>
public partial class Cities
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
    [Display(Name = "Country Id")]
    public int Idcountry { get; set; }

    /// <summary>
    /// state id
    /// </summary>
    [Required(ErrorMessage = "State is required.")]
    [Display(Name = "State Id")]
    public int Idstate { get; set; }

    /// <summary>
    /// city name
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters.")]
    [Display(Name = "City Name")]
    public string? Name { get; set; }

    /// <summary>
    /// active
    /// </summary>
    public bool Active { get; set; }


    /// <summary>
    /// company navegation
    /// </summary>
    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    /// <summary>
    /// country navegation
    /// </summary>
    public virtual Countries IdcountryNavigation { get; set; } = null!;

    /// <summary>
    /// state navegation
    /// </summary>
    public virtual StatesCountries IdstateNavigation { get; set; } = null!;

    /// <summary>
    /// location navegation
    /// </summary>
    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DUNES.API.ModelsWMS.Masters;


/// <summary>
/// company client information
/// </summary>
public partial class CompanyClient
{
    /// <summary>
    /// internal id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// tax company identification
    /// </summary>
    /// 
    [Display(Name ="Tax Identification")]
    public string? CompanyId { get; set; }

    /// <summary>
    /// company name
    /// </summary>
    /// 
    [Display(Name = "Company Name")]
    public string? Name { get; set; }


    /// <summary>
    /// country
    /// </summary>
     [Display(Name ="Country")]
  
    public int Idcountry { get; set; }

   

    /// <summary>
    /// state
    /// </summary>
    /// 
    [Display(Name = "State")]
    public int Idstate { get; set; }

    /// <summary>
    /// city
    /// </summary>
    /// 
    [Display(Name = "City")]
    public int Idcity { get; set; }

    /// <summary>
    /// zip code
    /// </summary>
    /// 
    [Display(Name = "Zip Code")]
    public string? Zipcode { get; set; }
    /// <summary>
    /// address
    /// </summary>
    /// 
    [Display(Name = "Address")]
    public string? Address { get; set; }

    /// <summary>
    /// phone
    /// </summary>
    /// 
    [Display(Name = "Phone")]
    public string? Phone { get; set; }

    /// <summary>
    /// web site
    /// </summary>
    /// 
    [Display(Name = "Web Site")]
    public string? Website { get; set; }

    /// <summary>
    /// active
    /// </summary>
    /// 
    [Display(Name = "Is Active")]
    public bool Active { get; set; }


    // 🔹 NAVIGATIONS con [ForeignKey]

    /// <summary>
    /// country navigation
    /// </summary>
    [ForeignKey(nameof(Idcountry))]
    public virtual Countries CountryNavegation { get; set; } = null!;

    /// <summary>
    /// state navigation
    /// </summary>
    [ForeignKey(nameof(Idstate))]
    public virtual StatesCountries StateNavegation { get; set; } = null!;

    /// <summary>
    /// city navigation
    /// </summary>
    [ForeignKey(nameof(Idcity))]
    public virtual Cities CityNavegation { get; set; } = null!;


    public virtual ICollection<Items> Items { get; set; } = new List<Items>();
}

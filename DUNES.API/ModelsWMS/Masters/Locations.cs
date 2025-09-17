using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

/// <summary>
/// WMS locations table
/// </summary>

public partial class Locations
{
    /// <summary>
    /// Location Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Location Description
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// DBK Name associated
    /// </summary>
    public string? Namedbk { get; set; }

    /// <summary>
    /// Company ID
    /// </summary>
    public int Idcompany { get; set; }
    /// <summary>
    /// Country Id
    /// </summary>
    public int Idcountry { get; set; }
    /// <summary>
    /// State Id
    /// </summary>
    public int Idstate { get; set; }
    /// <summary>
    /// City Id
    /// </summary>
    public int Idcity { get; set; }
    /// <summary>
    /// Zip code
    /// </summary>
    public string? Zipcode { get; set; }
    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }
   /// <summary>
   /// Phone
   /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// Is Active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// Cities Navegation Property
    /// </summary>
    public virtual Cities IdcityNavigation { get; set; } = null!;
    /// <summary>
    /// Company Navegation Property
    /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;
    /// <summary>
    /// Country Navegation Property
    /// </summary>
    public virtual Countries IdcountryNavigation { get; set; } = null!;
    /// <summary>
    /// States Navegation Propery
    /// </summary>
    public virtual StatesCountries IdstateNavigation { get; set; } = null!;

    /// <summary>
    /// Inventory Detail Navegacion Property
    /// </summary>
    public virtual ICollection<Inventorydetail> Inventorydetail { get; set; } = new List<Inventorydetail>();
    /// <summary>
    /// Inventory Movement Navegation Propery
    /// </summary>
    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();
    /// <summary>
    /// Transaction Detail Navegation Property
    /// </summary>
    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
}

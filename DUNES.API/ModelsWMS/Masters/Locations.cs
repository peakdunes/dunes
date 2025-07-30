using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

public partial class Locations
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Namedbk { get; set; }

    public int Idcompany { get; set; }

    public int Idcountry { get; set; }

    public int Idstate { get; set; }

    public int Idcity { get; set; }

    public string? Zipcode { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public virtual Cities IdcityNavigation { get; set; } = null!;

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual Countries IdcountryNavigation { get; set; } = null!;

    public virtual StatesCountries IdstateNavigation { get; set; } = null!;

    public virtual ICollection<Inventorydetail> Inventorydetail { get; set; } = new List<Inventorydetail>();

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
}

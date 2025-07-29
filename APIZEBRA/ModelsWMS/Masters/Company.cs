using System;
using System.Collections.Generic;

namespace APIZEBRA.ModelsWMS.Masters;

public partial class Company
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }

    public string? Name { get; set; }

    public int Idcountry { get; set; }

    public int Idstate { get; set; }

    public int Idcity { get; set; }

    public string? Zipcode { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Website { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Bines> Bines { get; set; } = new List<Bines>();

    public virtual Cities IdcityNavigation { get; set; } = null!;

    public virtual Countries IdcountryNavigation { get; set; } = null!;

    public virtual StatesCountries IdstateNavigation { get; set; } = null!;

    public virtual ICollection<InventoryTypes> InventoryTypes { get; set; } = new List<InventoryTypes>();

    public virtual ICollection<Inventorycategories> Inventorycategories { get; set; } = new List<Inventorycategories>();

    public virtual ICollection<Inventorydetail> Inventorydetail { get; set; } = new List<Inventorydetail>();

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();

    public virtual ICollection<InventorytransactionHdr> InventorytransactionHdr { get; set; } = new List<InventorytransactionHdr>();

    public virtual ICollection<Itemstatus> Itemstatus { get; set; } = new List<Itemstatus>();

    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();

    public virtual ICollection<Racks> Racks { get; set; } = new List<Racks>();

    public virtual ICollection<Transactionconcepts> Transactionconcepts { get; set; } = new List<Transactionconcepts>();

    public virtual ICollection<Transactiontypes> Transactiontypes { get; set; } = new List<Transactiontypes>();
}

using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// Company table info
/// </summary>
public partial class Company
{
    /// <summary>
    /// primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// company id
    /// </summary>
    public string? CompanyId { get; set; }
    /// <summary>
    /// company name
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// country id
    /// </summary>
    public int Idcountry { get; set; }
    /// <summary>
    /// state id
    /// </summary>
    public int Idstate { get; set; }
    /// <summary>
    /// city id
    /// </summary>
    public int Idcity { get; set; }
    /// <summary>
    /// zip code
    /// </summary>
    public string? Zipcode { get; set; }
    /// <summary>
    /// address
    /// </summary>
    public string? Address { get; set; }
    /// <summary>
    /// phone
    /// </summary>
    public string? Phone { get; set; }
    /// <summary>
    /// web site
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// is active
    /// </summary>
    public bool Active { get; set; }
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Bines details for this record.
    /// </summary>
    public virtual ICollection<Bines> Bines { get; set; } = new List<Bines>();

    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Cities details for this record.
    /// </summary>
    public virtual Cities IdcityNavigation { get; set; } = null!;
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Countries details for this record.
    /// </summary>
    public virtual Countries IdcountryNavigation { get; set; } = null!;
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full StatesCountries details for this record.
    /// </summary>
    public virtual StatesCountries IdstateNavigation { get; set; } = null!;
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full InventoryTypes details for this record.
    /// </summary>
    public virtual ICollection<InventoryTypes> InventoryTypes { get; set; } = new List<InventoryTypes>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Inventorycategories details for this record.
    /// </summary>
    public virtual ICollection<Inventorycategories> Inventorycategories { get; set; } = new List<Inventorycategories>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Inventorydetail details for this record.
    /// </summary>
    public virtual ICollection<Inventorydetail> Inventorydetail { get; set; } = new List<Inventorydetail>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Inventorymovement details for this record.
    /// </summary>
    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full InventorytransactionDetail details for this record.
    /// </summary>
    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full InventorytransactionHdr details for this record.
    /// </summary>
    public virtual ICollection<InventorytransactionHdr> InventorytransactionHdr { get; set; } = new List<InventorytransactionHdr>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Itemstatus details for this record.
    /// </summary>
    public virtual ICollection<Itemstatus> Itemstatus { get; set; } = new List<Itemstatus>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Locations details for this record.
    /// </summary>
    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Racks details for this record.
    /// </summary>
    public virtual ICollection<Racks> Racks { get; set; } = new List<Racks>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Transactionconcepts details for this record.
    /// </summary>
    public virtual ICollection<Transactionconcepts> Transactionconcepts { get; set; } = new List<Transactionconcepts>();
    /// <summary>
    /// Navigation property for the related City entity.
    /// Allows access to full Transactiontypes details for this record.
    /// </summary>
    public virtual ICollection<Transactiontypes> Transactiontypes { get; set; } = new List<Transactiontypes>();
}

using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// WMS Bins table
/// </summary>
public partial class Bines
{
    /// <summary>
    /// Bin id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Bin description
    /// </summary>
    public string? TagName { get; set; }
    /// <summary>
    /// Company ID
    /// </summary>
    public int Idcompany { get; set; }
    /// <summary>
    /// Company Client Id
    /// </summary>
    public string? Idcompanyclient { get; set; }
    /// <summary>
    /// Observations
    /// </summary>
    public string? Observations { get; set; }
    /// <summary>
    /// Bin Active?
    /// </summary>
    public bool Active { get; set; }
    /// <summary>
    /// Included in precompumption process
    /// </summary>
    public bool IncludeInConsumption { get; set; }

    /// <summary>
    /// Company navegation property
    /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;
    /// <summary>
    /// Inventory detail navegation Property
    /// </summary>
    public virtual ICollection<Inventorydetail> Inventorydetail { get; set; } = new List<Inventorydetail>();
    /// <summary>
    /// Inventory Movement Navegacion Property
    /// </summary>
    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();
    /// <summary>
    /// Inventory Detail transaction Navegation Property
    /// </summary>
    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
}

using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// Inventory Detail
/// </summary>
public partial class Inventorydetail
{
    //record id
    public int Id { get; set; }

    /// <summary>
    /// Company Id
    /// </summary>
    public int Idcompany { get; set; }

    /// <summary>
    /// Location ID
    /// </summary>
    public int Idlocation { get; set; }
    /// <summary>
    /// Inventory Tyupe ID
    /// </summary>
    public int Idtype { get; set; }

    /// <summary>
    /// Rack ID
    /// </summary>
    public int Idrack { get; set; }

    /// <summary>
    /// Level in rack
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// Part Number
    /// </summary>
    public string? Iditem { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    public int TotalQty { get; set; }

    /// <summary>
    /// Bin Id
    /// </summary>
    public int Idbin { get; set; }

    /// <summary>
    /// COmpany Client ID
    /// </summary>
    public string? Idcompanyclient { get; set; }

    /// <summary>
    /// Serial ID
    /// </summary>
    public string? Serialid { get; set; }

    /// <summary>
    /// Status id
    /// </summary>
    public int Idstatus { get; set; }

    /// <summary>
    /// Company Client Division
    /// </summary>
    public string? Iddivision { get; set; }

    /// <summary>
    /// Bins Navegation Property
    /// </summary>
    public virtual Bines IdbinNavigation { get; set; } = null!;
    /// <summary>
    /// Company Navegation Property
    /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;
    /// <summary>
    /// Location Navegation Property
    /// </summary>
    public virtual Locations IdlocationNavigation { get; set; } = null!;
    /// <summary>
    /// Rack Navegation Property
    /// </summary>
    public virtual Racks IdrackNavigation { get; set; } = null!;
    /// <summary>
    /// Item status navegation property
    /// </summary>
    public virtual Itemstatus IdstatusNavigation { get; set; } = null!;
    /// <summary>
    /// Inventory Type Navegation ID
    /// </summary>
    public virtual InventoryTypes IdtypeNavigation { get; set; } = null!;
}

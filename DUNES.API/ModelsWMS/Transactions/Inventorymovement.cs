using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;


/// <summary>
/// WMS inventory transaction movement
/// </summary>
public partial class Inventorymovement
{
    /// <summary>
    /// primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// transaction type
    /// </summary>
    public int Idtransactiontype { get; set; }

    /// <summary>
    /// location ID
    /// </summary>
    public int Idlocation { get; set; }


    /// <summary>
    /// inventory type ID
    /// </summary>
    public int Idtype { get; set; }

    /// <summary>
    /// rack Id
    /// </summary>
    public int Idrack { get; set; }
    /// <summary>
    /// rack level
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// bin Id
    /// </summary>
    public int Idbin { get; set; }


    /// <summary>
    /// item code
    /// </summary>
    public string? Iditem { get; set; }

    /// <summary>
    /// item status id
    /// </summary>
    public int Idstatus { get; set; }

    /// <summary>
    /// serial number
    /// </summary>
    public string? Serialid { get; set; }

    /// <summary>
    /// date created
    /// </summary>
    public DateTime Datecreated { get; set; }

    /// <summary>
    /// Quantity input
    /// </summary>
    public int Qtyinput { get; set; }

    /// <summary>
    /// Quantity output
    /// </summary>
    public int Qtyoutput { get; set; }

    /// <summary>
    /// Quantity Balance
    /// </summary>
    public int Qtybalance { get; set; }

    /// <summary>
    /// Company Id
    /// </summary>
    public int Idcompany { get; set; }

    /// <summary>
    /// Company Client Id
    /// </summary>
    public string? Idcompanyclient { get; set; }

    /// <summary>
    /// Id transaction Head
    /// </summary>
    public int IdtransactionHead { get; set; }

    /// <summary>
    /// Id Detail Transaction
    /// </summary>
    public int IdtransactionDetail { get; set; }

    /// <summary>
    /// division Id
    /// </summary>
    public string? Iddivision { get; set; }

    public string? Createdby { get; set; }

    public int Idtransactionconcept { get; set; }

    public virtual Bines IdbinNavigation { get; set; } = null!;

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual Locations IdlocationNavigation { get; set; } = null!;

    public virtual Racks IdrackNavigation { get; set; } = null!;

    public virtual Itemstatus IdstatusNavigation { get; set; } = null!;

    public virtual InventorytransactionDetail IdtransactionDetailNavigation { get; set; } = null!;

    public virtual InventorytransactionHdr IdtransactionHeadNavigation { get; set; } = null!;

    public virtual Transactionconcepts IdtransactionconceptNavigation { get; set; } = null!;

    public virtual Transactiontypes IdtransactiontypeNavigation { get; set; } = null!;

    public virtual InventoryTypes IdtypeNavigation { get; set; } = null!;
}

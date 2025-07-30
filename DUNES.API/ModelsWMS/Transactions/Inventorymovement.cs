using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

public partial class Inventorymovement
{
    public int Id { get; set; }

    public int Idtransactiontype { get; set; }

    public int Idlocation { get; set; }

    public int Idtype { get; set; }

    public int Idrack { get; set; }

    public int Level { get; set; }

    public int Idbin { get; set; }

    public string? Iditem { get; set; }

    public int Idstatus { get; set; }

    public string? Serialid { get; set; }

    public DateTime Datecreated { get; set; }

    public int Qtyinput { get; set; }

    public int Qtyoutput { get; set; }

    public int Qtybalance { get; set; }

    public int Idcompany { get; set; }

    public string? Idcompanyclient { get; set; }

    public int IdtransactionHead { get; set; }

    public int IdtransactionDetail { get; set; }

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

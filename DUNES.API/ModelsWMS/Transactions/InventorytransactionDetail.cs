using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

/// <summary>
/// DTO for InventorytransactionHdr
/// </summary>
public partial class InventorytransactionDetail
{
    public int Id { get; set; }

    public int Idtypetransaction { get; set; }

    public int Idlocation { get; set; }

    public int Idtype { get; set; }

    public int Idrack { get; set; }

    public int Level { get; set; }

    public string? Iditem { get; set; }

    public int TotalQty { get; set; }

    public int Idbin { get; set; }

    public int Idstatus { get; set; }

    public string? Serialid { get; set; }

    public int Idcompany { get; set; }

    public string? Idcompanyclient { get; set; }

    public string? Iddivision { get; set; }

    public int Idenctransaction { get; set; }

    public virtual Bines IdbinNavigation { get; set; } = null!;

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual InventorytransactionHdr IdenctransactionNavigation { get; set; } = null!;

    public virtual Locations IdlocationNavigation { get; set; } = null!;

    public virtual Racks IdrackNavigation { get; set; } = null!;

    public virtual Itemstatus IdstatusNavigation { get; set; } = null!;

    public virtual InventoryTypes IdtypeNavigation { get; set; } = null!;

    public virtual Transactiontypes IdtypetransactionNavigation { get; set; } = null!;

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();
}

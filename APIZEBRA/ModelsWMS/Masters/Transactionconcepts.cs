using System;
using System.Collections.Generic;

namespace APIZEBRA.ModelsWMS.Masters;

public partial class Transactionconcepts
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Idcompany { get; set; }

    public string? Idcompanyclient { get; set; }

    public bool Active { get; set; }

    public int CallType { get; set; }

    public int ZebraInventoryAssociated { get; set; }

    public bool IsInternal { get; set; }

    public bool CreateZebraCall { get; set; }

    public bool CreateZebraInvTran { get; set; }

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<InventorytransactionHdr> InventorytransactionHdr { get; set; } = new List<InventorytransactionHdr>();
}

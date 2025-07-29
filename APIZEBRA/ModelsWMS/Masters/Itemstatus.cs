using System;
using System.Collections.Generic;

namespace APIZEBRA.ModelsWMS.Masters;

public partial class Itemstatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Idcompany { get; set; }

    public string? Observations { get; set; }

    public bool Active { get; set; }

    public string? Idcompanyclient { get; set; }

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual ICollection<Inventorydetail> Inventorydetail { get; set; } = new List<Inventorydetail>();

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
}

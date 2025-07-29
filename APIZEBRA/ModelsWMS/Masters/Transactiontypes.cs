using System;
using System.Collections.Generic;

namespace APIZEBRA.ModelsWMS.Masters;

public partial class Transactiontypes
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Idcompany { get; set; }

    public bool Isinput { get; set; }

    public bool Isoutput { get; set; }

    public bool Active { get; set; }

    public string? Idcompanyclient { get; set; }

    public string? Match { get; set; }

    public bool Ispreconsumption { get; set; }

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
}

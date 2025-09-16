using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;


/// <summary>
/// Inventory Transaction Header
/// </summary>
public partial class InventorytransactionHdr
{
    public int Id { get; set; }

    public int Idcompany { get; set; }

    public int Idtransactionconcept { get; set; }

    public string? IdUser { get; set; }

    public DateTime Datecreated { get; set; }

    public bool Processed { get; set; }

    public string? IdUserprocess { get; set; }

    public string? Idcompanyclient { get; set; }

    public DateTime Dateprocessed { get; set; }

    public string? Documentreference { get; set; }

    public string? Observations { get; set; }

    public string? Iddivision { get; set; }

    public virtual Company IdcompanyNavigation { get; set; } = null!;

    public virtual Transactionconcepts IdtransactionconceptNavigation { get; set; } = null!;

    public virtual ICollection<Inventorymovement> Inventorymovement { get; set; } = new List<Inventorymovement>();

    public virtual ICollection<InventorytransactionDetail> InventorytransactionDetail { get; set; } = new List<InventorytransactionDetail>();
}

using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

public partial class Inventorycategories
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int Idcompany { get; set; }

    public string? Observations { get; set; }

    public bool Active { get; set; }

    public string? Idcompanyclient { get; set; }

    public virtual Company IdcompanyNavigation { get; set; } = null!;
}

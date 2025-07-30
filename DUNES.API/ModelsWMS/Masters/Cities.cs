using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

public partial class Cities
{
    public int Id { get; set; }

    public int Idcountry { get; set; }

    public int Idstate { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    public virtual Countries IdcountryNavigation { get; set; } = null!;

    public virtual StatesCountries IdstateNavigation { get; set; } = null!;

    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();
}

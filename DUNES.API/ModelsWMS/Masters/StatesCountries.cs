using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;

public partial class StatesCountries
{
    public int Id { get; set; }

    public int Idcountry { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public string? Sigla { get; set; }

    public virtual ICollection<Cities> Cities { get; set; } = new List<Cities>();

    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    public virtual Countries IdcountryNavigation { get; set; } = null!;

    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();
}

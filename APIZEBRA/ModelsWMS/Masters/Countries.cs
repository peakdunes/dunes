using System;
using System.Collections.Generic;

namespace APIZEBRA.ModelsWMS.Masters;

public partial class Countries
{
    public int Id { get; set; }

    public string? Sigla { get; set; }

    public string? Name { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<Cities> Cities { get; set; } = new List<Cities>();

    public virtual ICollection<Company> Company { get; set; } = new List<Company>();

    public virtual ICollection<Locations> Locations { get; set; } = new List<Locations>();

    public virtual ICollection<StatesCountries> StatesCountries { get; set; } = new List<StatesCountries>();
}

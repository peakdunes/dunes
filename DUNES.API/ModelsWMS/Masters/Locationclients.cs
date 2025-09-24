using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// Company Client By location
/// </summary>
public partial class Locationclients
{
    public int Id { get; set; }

    public int Idcompany { get; set; }

    public int Idlocation { get; set; }

    public string? Idcompanyclient { get; set; }

    public bool Active { get; set; }
}

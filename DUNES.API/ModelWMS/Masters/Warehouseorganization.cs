using System;
using System.Collections.Generic;

namespace DUNES.API.ModelWMS.Masters;
/// <summary>
/// WMS Warehouse organization
/// </summary>
public partial class Warehouseorganization
{
    public int Id { get; set; }

    public int Idcompany { get; set; }

    public string? Idcompanyclient { get; set; }

    public string? Iddivision { get; set; }

    public int Idlocation { get; set; }

    public int Idrack { get; set; }

    public int Level { get; set; }

    public int Idbin { get; set; }
}

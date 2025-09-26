using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;
/// <summary>
/// WMS Clients table
/// </summary>
public partial class WmsCompanyclient
{
    public int Id { get; set; }

    public string? CompanyId { get; set; }
}

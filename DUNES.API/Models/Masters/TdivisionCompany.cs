using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

public partial class TdivisionCompany
{
    public string DivisionDsc { get; set; } = null!;

    public string CompanyDsc { get; set; } = null!;

    public bool? CanBeOrdersPartiallyShipped { get; set; }

    public bool? Active { get; set; }
}

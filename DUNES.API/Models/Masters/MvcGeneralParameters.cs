using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

public partial class MvcGeneralParameters
{
    public int Id { get; set; }

    public int ParameterNumber { get; set; }

    public string? ParameterArea { get; set; }

    public string? ParameterDescription { get; set; }

    public string? ParameterValue { get; set; }
}

using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Configuration;

public partial class Mvcgeneralparameter
{
    public int Id { get; set; }

    public int ParameterId { get; set; }

    public string? Parametername { get; set; }

    public string? Parametervalue { get; set; }
}

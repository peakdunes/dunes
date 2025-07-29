using System;
using System.Collections.Generic;

namespace APIZEBRA.ModelsWMS.Masters;

public partial class Generalparameters
{
    public int Id { get; set; }

    public int ParameterId { get; set; }

    public string? Parametername { get; set; }

    public string? Parametervalue { get; set; }
}

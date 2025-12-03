using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

/// <summary>
/// general parameters
/// </summary>
public partial class MvcGeneralParameters
{

    /// <summary>
    /// identity id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// parameter number
    /// </summary>
    public int ParameterNumber { get; set; }
    /// <summary>
    /// parameter area
    /// </summary>
    public string? ParameterArea { get; set; }


    /// <summary>
    /// parameter description
    /// </summary>
    public string? ParameterDescription { get; set; }

    /// <summary>
    /// parameter value
    /// </summary>
    public string? ParameterValue { get; set; }
}

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;

namespace DUNES.API.ModelsWMS.Masters;

/// <summary>
/// company client division MOdel
/// </summary>
public partial class CompanyClientDivision
{

    /// <summary>
    /// internal id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Division Name
    /// </summary>
    public string? DivisionName { get; set; }

    /// <summary>
    /// Company client Id
    /// </summary>
    public int Idcompanyclient { get; set; }


    /// <summary>
    /// Active
    /// </summary>
    public bool IsActive { get; set; }

    
}



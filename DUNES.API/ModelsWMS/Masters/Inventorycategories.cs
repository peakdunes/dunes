using System;
using System.Collections.Generic;

namespace DUNES.API.ModelsWMS.Masters;
/// <summary>
/// inventory categories
/// </summary>
public class Inventorycategories
{
    /// <summary>
    /// internal database id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// category name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// company id
    /// </summary>
    public int companyId { get; set; }

    /// <summary>
    /// boservations
    /// </summary>
    public string? Observations { get; set; }

    /// <summary>
    /// is active
    /// </summary>
    public bool Active { get; set; }


    /// <summary>
    /// company navegation
    /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;




}

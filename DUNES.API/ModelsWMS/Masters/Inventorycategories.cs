using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    /// observaciones
    /// </summary>
    [MaxLength(1000)]
    public string? Observations { get; set; }


    /// <summary>
    /// cicle count frecuency
    /// </summary>
    public int CycleCountDays { get; set; }

    /// <summary>
    /// error tolerance
    /// </summary>
    /// 
    [Column(TypeName = "decimal(5,2)")]
    public decimal ErrorTolerance { get; set; }

    /// <summary>
    /// is active
    /// </summary>
    public bool Active { get; set; }





    /// <summary>
    /// company navegation
    /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;




}

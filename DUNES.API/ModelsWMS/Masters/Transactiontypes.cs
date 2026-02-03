using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Masters;

/// <summary>
/// wms transactions types
/// </summary>
public class Transactiontypes
{

    /// <summary>
    /// internal database id
    /// </summary>
    public int Id { get; set; }


    /// <summary>
    /// transaction name
    /// </summary>
    [MaxLength(200)]
    public string? Name { get; set; }

    /// <summary>
    /// company id
    /// </summary>
    public int companyId { get; set; }

    /// <summary>
    /// this type is input
    /// </summary>
    public bool Isinput { get; set; }

    /// <summary>
    /// this type is output
    /// </summary>
    public bool Isoutput { get; set; }

    /// <summary>
    /// type is active
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// code asociate for transaction type transfer
    /// </summary>
    [MaxLength(3)]
    public string? Match { get; set; }

  /// <summary>
  /// navegation property
  /// </summary>
    public virtual Company IdcompanyNavigation { get; set; } = null!;

 }

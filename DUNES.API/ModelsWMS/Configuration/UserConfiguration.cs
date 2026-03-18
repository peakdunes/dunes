using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ModelsWMS.Configuration;

/// <summary>
/// Represents the operational configuration assigned to a user.
/// This entity stores the active/default environment, company, client,
/// location, role, and other execution settings required by DUNES.
/// </summary>
public class UserConfiguration
{
    /// <summary>
    /// Database identifier of the user configuration.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identity user identifier (AspNetUsers.Id) that owns this configuration.
    /// </summary>
    [MaxLength(450)]
    public string Userid { get; set; } = string.Empty;

    /// <summary>
    /// Environment name assigned to this configuration.
    /// </summary>
    [MaxLength(100)]
    public string Enviromentname { get; set; } = string.Empty;

    /// <summary>
    /// Default company identifier for this configuration.
    /// </summary>
    public int Companydefault { get; set; }

    /// <summary>
    /// Default company client identifier for this configuration.
    /// </summary>
    public int Companyclientdefault { get; set; }

    /// <summary>
    /// Default location identifier for this configuration.
    /// </summary>
    public int Locationdefault { get; set; }

    /// <summary>
    /// Default contract identifier for this configuration.
    /// </summary>
    public int CompaniesContractId { get; set; }

    /// <summary>
    /// Default bind CR1 identifier for this configuration.
    /// </summary>
    public int Bindcr1default { get; set; }

    /// <summary>
    /// Default WMS bin identifier for this configuration.
    /// </summary>
    public int Wmsbin { get; set; }

    /// <summary>
    /// Default division identifier for this configuration.
    /// </summary>
    public int Divisiondefault { get; set; }

    /// <summary>
    /// Indicates whether this configuration is the active one for the user.
    /// </summary>
    public bool Isactive { get; set; }

    /// <summary>
    /// Distribution bins or related distribution configuration data.
    /// </summary>
    public string Binesdistribution { get; set; } = string.Empty;

    /// <summary>
    /// Default transfer concept identifier for this configuration.
    /// </summary>
    public int Concepttransferdefault { get; set; }

    /// <summary>
    /// Default transfer transaction identifier for this configuration.
    /// </summary>
    public int Transactiontransferdefault { get; set; }

    /// <summary>
    /// Indicates whether the user is allowed to change settings.
    /// </summary>
    public bool AllowChangeSettings { get; set; }

    /// <summary>
    /// Indicates whether the user can delete only their own transactions.
    /// </summary>
    public bool Deleteonlymytran { get; set; }

    /// <summary>
    /// UTC date and time when the configuration was created.
    /// </summary>
    public DateTime Datecreated { get; set; }

    /// <summary>
    /// Indicates whether the user can process only their own transactions.
    /// </summary>
    public bool Processonlymytran { get; set; }

    /// <summary>
    /// Identity role identifier associated with this configuration.
    /// </summary>
    public string Roleid { get; set; } = string.Empty;

    /// <summary>
    /// Indicates whether the configuration belongs to a depot workflow.
    /// </summary>
    public bool Isdepot { get; set; }
}

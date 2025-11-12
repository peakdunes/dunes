using System;
using System.Collections.Generic;

namespace DUNES.API.Models.WebService;
/// <summary>
/// this table save the hourly call records (ZEBRA to PEAK) 
/// </summary>

public  class MvcWebServiceHourlySummary
{

    /// <summary>
    /// year
    /// </summary>
    public int Year { get; set; }
    /// <summary>
    /// month
    /// </summary>
    public byte Month { get; set; }
    /// <summary>
    /// day
    /// </summary>
    public byte Day { get; set; }
    /// <summary>
    /// hour
    /// </summary>
    public byte Hour { get; set; }

    /// <summary>
    /// total calls
    /// </summary>
    public int TotalCalls { get; set; }

    /// <summary>
    /// total errors
    /// </summary>
    public int TotalErrors { get; set; }
    /// <summary>
    /// % total call vs total erros
    /// </summary>
    public decimal ErrorRate { get; set; }
    /// <summary>
    /// source
    /// </summary>
    public string? Source { get; set; }

    /// <summary>
    /// last updated
    /// </summary>
    public DateTime LastUpdatedUtc { get; set; }
}

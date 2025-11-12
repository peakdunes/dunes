using System;
using System.Collections.Generic;

namespace DUNES.API.Models.WebService;

/// <summary>
/// this table save the daily call records (ZEBRA to PEAK) 
/// </summary>
public class MvcWebServiceDailySummary
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
    /// total calls
    /// </summary>
    public int TotalCalls { get; set; }

    /// <summary>
    /// total error
    /// </summary>
    public int TotalErrors { get; set; }

    /// <summary>
    /// % total call vs total erros
    /// </summary>
    public decimal ErrorRate { get; set; }

    /// <summary>
    /// Actual 
    /// </summary>
    public bool Current { get; set; }


    /// <summary>
    /// last update
    /// </summary>
    public DateTime LastUpdatedUtc { get; set; }
}

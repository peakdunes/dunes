using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.Masters;

public partial class Ttech
{
    public int TtechNo { get; set; }

    public string? TtechName { get; set; }

    public bool? IsTech { get; set; }

    public string? Login { get; set; }

    public string? Password { get; set; }

    /// <summary>
    /// D: Day, N:Night
    /// </summary>
    public string? Shift { get; set; }

    /// <summary>
    /// Boolean: if Tec repairs parts (radio, cpu, etc) = 1; only Devices = 0 
    /// </summary>
    public bool? RepParts { get; set; }

    public bool? IsLoggedinInvSys { get; set; }

    public double? AdditionalTimeFromCurrDate { get; set; }

    public bool? Active { get; set; }

    public bool? Admin { get; set; }

    public bool? Engineer { get; set; }

    public bool? Supervisor { get; set; }

    public string? EmployeeCode { get; set; }

    public string? Email { get; set; }
}

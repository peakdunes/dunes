using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class UserMvcAssignments
{
    public int Id { get; set; }

    public int Repairid { get; set; }

    public string? Userassigned { get; set; }

    public string? ActualArea { get; set; }

    public DateTime Assigneddate { get; set; }

    public string? Userprocess { get; set; }

    public bool? Bypass { get; set; }

    public DateTime Bypassdate { get; set; }

    public string? Userbypass { get; set; }

    public DateTime Duedate { get; set; }

    public bool? Sendalert { get; set; }
}

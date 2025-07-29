using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.Auth;

public partial class MvcPartRunnerMenu
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Level1 { get; set; }

    public string? Level2 { get; set; }

    public string? Level3 { get; set; }

    public string? Level4 { get; set; }

    public string? Level5 { get; set; }

    public string? Roles { get; set; }

    public bool Active { get; set; }

    public string? Utility { get; set; }

    public string? Action { get; set; }

    public string? Controller { get; set; }

    public int Order { get; set; }
}

using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.Masters;

public partial class Tdbkusers
{
    public string Login { get; set; } = null!;

    public string? Password { get; set; }

    public string? Name { get; set; }

    public bool? IsAdministrator { get; set; }

    public string? WorkingForCompany { get; set; }

    public int Id { get; set; }

    public string? Email { get; set; }

    public bool? IsCustomerService { get; set; }

    public bool? IsTechnicalSupport { get; set; }

    public bool IsZebraPartRunner { get; set; }

    public bool? IsSupervisor { get; set; }

    public string? SupervisorPin { get; set; }

    public bool IsRyderPartMaintenance { get; set; }
}

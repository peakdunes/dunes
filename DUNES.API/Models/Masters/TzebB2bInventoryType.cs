using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

public partial class TzebB2bInventoryType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Comments { get; set; }

    public bool Internal { get; set; }

    public bool Adjustment { get; set; }

    public string? ShipToLocation { get; set; }

    public bool Usps { get; set; }

    /// <summary>
    /// IF NULL: INVENTORY IS NOT PART OF PRE-CONSUMPTION MODEL -- NOT NULL, INV ONLY CAN BE PRECONSUMED INTO THIS PRECON_INV_DEST VALUE
    /// </summary>
    public int? PreconInvDest { get; set; }
}

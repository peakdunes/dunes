using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class TrepairActionsLog
{
    public int Id { get; set; }

    public int? TtechNo { get; set; }

    public int? RepairNo { get; set; }

    public int? ActionId { get; set; }

    public DateTime? ActionDate { get; set; }
}

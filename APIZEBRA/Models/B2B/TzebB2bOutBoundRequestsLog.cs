using System;
using System.Collections.Generic;

namespace APIZEBRA.Models.B2B;

public partial class TzebB2bOutBoundRequestsLog
{
    public int Id { get; set; }

    public int RepairNo { get; set; }

    public int TypeOfCallId { get; set; }

    public DateTime DateTimeInserted { get; set; }

    public bool NotificationSent { get; set; }

    public DateTime? NotificationDateTime { get; set; }

    public string? NotificationXml { get; set; }

    public bool AckReceived { get; set; }

    public DateTime? AckDateTime { get; set; }

    public string? AckXml { get; set; }

    public bool InProcess { get; set; }

    public string? AckResult { get; set; }

    public bool SuccessCall { get; set; }

    public string? AdditionalInfo { get; set; }

    public string? Srlnumber { get; set; }

    public string? Failure { get; set; }
}

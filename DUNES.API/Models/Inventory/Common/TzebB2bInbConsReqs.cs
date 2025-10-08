using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Inventory.Common;


/// <summary>
/// ZEBRA to Peak calls
/// </summary>
public partial class TzebB2bInbConsReqs
{
    public int Id { get; set; }

    public string SenderCode { get; set; } = null!;

    public string ReceiverCode { get; set; } = null!;

    public string TransactionCode { get; set; } = null!;

    public string UniqueMessageIdentifier { get; set; } = null!;

    public DateTime SentTimestamp { get; set; }

    public string FileType { get; set; } = null!;

    public string? Edistandard { get; set; }

    public string ResponseLevel { get; set; } = null!;

    public string? PSuccessEmail { get; set; }

    public string? PErrorEmail { get; set; }

    public string? FullXml { get; set; }

    public DateTime DateTimeInserted { get; set; }

    public string? Error { get; set; }

    public bool Processed { get; set; }
}

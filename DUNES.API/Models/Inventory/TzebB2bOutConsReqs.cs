using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DUNES.API.Models.Inventory;

/// <summary>
/// Inventory calls Peak to ZEBRA
/// </summary>
public partial class TzebB2bOutConsReqs
{
    public int Id { get; set; }

    public int TypeOfCallId { get; set; }

    public DateTime DateTimeInserted { get; set; }

    public string? FullXmlsent { get; set; }

    public bool? AckReceived { get; set; }

    public string? ResponseXml { get; set; }

    public string? Result { get; set; }

    public string? Failure { get; set; }

    public string? AdditionalInfo { get; set; }

    public string? SenderCode { get; set; }

    public string? ReceiverCode { get; set; }

    public string? TransactionCode { get; set; }

    public string? UniqueMessageIdentifier { get; set; }

    public DateTime? SentTimestamp { get; set; }

    public string? FileType { get; set; }

    public string? Edistandard { get; set; }

    public string? ResponseLevel { get; set; }

    public DateTime? DateTimeSent { get; set; }

    public bool InProcess { get; set; }

    public string? Additional { get; set; }

    [NotMapped]
    [MaxLength(200)]
    public string? callName { get; set; }
}

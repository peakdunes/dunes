using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIZEBRA.Models.B2B;
/// <summary>
/// call record Peaktech to ZEBRA
/// </summary>
public partial class TzebB2bOutBoundRequestsLog
{

    /// <summary>
    /// primary key
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// repair number
    /// </summary>
    public int RepairNo { get; set; }
    /// <summary>
    /// type of call id
    /// </summary>
    public int TypeOfCallId { get; set; }
    /// <summary>
    /// date time inserted
    /// </summary>
    public DateTime DateTimeInserted { get; set; }
    /// <summary>
    /// was send the norification
    /// </summary>
    public bool NotificationSent { get; set; }
    /// <summary>
    /// notification date
    /// </summary>
    public DateTime? NotificationDateTime { get; set; }
    /// <summary>
    /// xml notification file
    /// </summary>
    public string? NotificationXml { get; set; }
    /// <summary>
    /// true o false ACK received
    /// </summary>
    public bool AckReceived { get; set; }
    /// <summary>
    /// ACK date
    /// </summary>
    public DateTime? AckDateTime { get; set; }
  /// <summary>
  /// ack XML file
  /// </summary>
    public string? AckXml { get; set; }
    /// <summary>
    /// true o false call processed
    /// </summary>
    public bool InProcess { get; set; }
    /// <summary>
    /// ack result
    /// </summary>
    public string? AckResult { get; set; }
    /// <summary>
    /// successfull or fail call
    /// </summary>
    public bool SuccessCall { get; set; }
    /// <summary>
    /// aditional call info
    /// </summary>
    public string? AdditionalInfo { get; set; }
    /// <summary>
    /// RMA number
    /// </summary>
    public string? Srlnumber { get; set; }
    /// <summary>
    /// failure description
    /// </summary>
    public string? Failure { get; set; }
    /// <summary>
    /// call description
    /// </summary>
    [NotMapped]
    [MaxLength(200)]
    public string? callname { get; set; }
}

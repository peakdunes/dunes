using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2b;
/// <summary>
/// _TorderRepair_ItemsSerials_Receiving model
/// </summary>
public partial class TorderRepairItemsSerialsReceiving
{
    public int RefNo { get; set; }

    public string PartNo { get; set; } = null!;

    public DateTime? DateReceived { get; set; }

    public string? SerialInbound { get; set; }

    public string? SerialReceived { get; set; }

    public string? TstatusId { get; set; }

    public float? PriceEstimated { get; set; }

    public bool? EstimatedAgree { get; set; }

    public int RepairNo { get; set; }

    public int? Qty { get; set; }

    public int? QtyReceived { get; set; }

    public int? CourierId { get; set; }

    public int Id { get; set; }

    public string? TechNotes { get; set; }

    public string? TrackingNumber { get; set; }

    public bool? InWaranty { get; set; }

    public DateTime? ConfirmationDate { get; set; }

    public DateTime? DateTimeReceived { get; set; }

    public bool? TechEvaluationDone { get; set; }

    public bool? IsObf { get; set; }

    public DateTime? ObfDateApproved { get; set; }

    public string? ObfApprovedBy { get; set; }

    /// <summary>
    /// Codifier: _TDBK_TypeOfRepair
    /// </summary>
    public int? TypeOfRepairId { get; set; }

    public DateTime? EstimatedAgreeDate { get; set; }

    public string? SparePoolId { get; set; }

    public int? KitNo { get; set; }

    public bool? RepairLogPrinted { get; set; }

    public string? ProjectName { get; set; }
}

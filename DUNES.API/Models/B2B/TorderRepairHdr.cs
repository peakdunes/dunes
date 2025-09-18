using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2b;
/// <summary>
/// _Torder_Repair_Hdr model
/// </summary>
public partial class TorderRepairHdr
{
    public int RefNo { get; set; }

    public DateTime? DateCreated { get; set; }

    public string TcustNo { get; set; } = null!;

    public string? CustRef { get; set; }

    public string? CustName { get; set; }

    public string? ShipToAddr { get; set; }

    public string? ShipToAddr1 { get; set; }

    public int? TcityId { get; set; }

    public string? TstateId { get; set; }

    public string? ZipCode { get; set; }

    public string? TstatusId { get; set; }

    public bool? EstimateRequired { get; set; }

    public string? BinLocation { get; set; }

    public int? CourierId { get; set; }

    public int? ShipViaId { get; set; }

    public string? ShippingInstr { get; set; }

    public DateTime? CanceledDate { get; set; }

    public DateTime? StopDate { get; set; }

    public DateTime? CloseDate { get; set; }

    public string? TechNotes { get; set; }

    public string? Att { get; set; }

    public DateTime? DateSaved { get; set; }

    public string? CreatedOnline { get; set; }

    public int? OriginId { get; set; }

    public string? Email { get; set; }

    public string? SonimCustType { get; set; }

    public string? SonimOperCode { get; set; }

    public string? PoRefNo { get; set; }

    public bool? EmailResponse { get; set; }

    public string? EmailResponseSource { get; set; }

    public DateTime? EmailResponseDateTime { get; set; }

    public DateTime? DateInserted { get; set; }

    public string? ReceivingCompName { get; set; }

    public string? ReceiverUser { get; set; }

    public DateTime? ReceivingStartDate { get; set; }

    public DateTime? ReceivingEndDate { get; set; }

    public virtual ICollection<TorderRepairItems> TorderRepairItems { get; set; } = new List<TorderRepairItems>();
}

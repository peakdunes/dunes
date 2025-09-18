using System;
using System.Collections.Generic;

namespace DUNES.API.Models.B2b;

/// <summary>
/// _TorderRepair_ItemsSerials_shipping
/// </summary>
public partial class TorderRepairItemsSerialsShipping
{
    public int RefNo { get; set; }

    public string PartNo { get; set; } = null!;

    public DateTime? DateShip { get; set; }

    public string? SerialShip { get; set; }

    public string? TstatusId { get; set; }

    public int RepairNo { get; set; }

    public int? Qty { get; set; }

    public int? QtyShip { get; set; }

    public int? ShipViaId { get; set; }

    public int Id { get; set; }

    public int? DaysToPickupFromShipping { get; set; }

    public string? TrackingNumber { get; set; }

    public short? ShippingGroupId { get; set; }

    public DateTime? DateTimeShip { get; set; }

    public string? UserName { get; set; }

    public bool? NeedSoftwareLoading { get; set; }

    public DateTime? DateTrackingNumber { get; set; }
}

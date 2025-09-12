using System;
using System.Collections.Generic;

namespace DUNES.API.Models.Masters;

public partial class TzebB2bMasterPartDefinition
{
    public int Id { get; set; }

    public string PartNo { get; set; } = null!;

    public string PartDsc { get; set; } = null!;

    public string? ItemLongDescription { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string Serialized { get; set; } = null!;

    public bool Repairable { get; set; }

    public string? AbcClassName { get; set; }

    public string? HtsCode { get; set; }

    public string? ProductClass { get; set; }

    public string? PrimaryUomCode { get; set; }

    public string? DangerousGoodsIndicator { get; set; }

    public string? ItemType { get; set; }

    public float? UnitLength { get; set; }

    public float? UnitWidth { get; set; }

    public float? UnitHeight { get; set; }

    public string? DimensionsUom { get; set; }

    public float? UnitWeight { get; set; }

    public string? WeightUom { get; set; }

    public float? UnitVolume { get; set; }

    public string? VolumeUom { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public string? ItemLifeCycle { get; set; }

    public string? Attribute3 { get; set; }

    public float? PalletSize { get; set; }

    public string? ItemSource { get; set; }

    public bool RepairableByDbk { get; set; }

    public float? InventoryItemId { get; set; }

    public float? LastUpdatedBy { get; set; }

    public string? ReceiptRouting { get; set; }

    public string? Eccn { get; set; }

    public bool BulkItem { get; set; }
}

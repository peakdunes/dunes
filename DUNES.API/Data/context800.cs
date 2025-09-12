using System;
using System.Collections.Generic;
using DUNES.API.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context800 : DbContext
{
    public context800()
    {
    }

    public context800(DbContextOptions<context800> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bMasterPartDefinition> TzebB2bMasterPartDefinition { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bMasterPartDefinition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_ZEBRA_Inventory_Master");

            entity.ToTable("_TZEB_B2B_Master_Part_Definition");

            entity.HasIndex(e => e.PartNo, "IX__TZEB_B2B_Master_Part_Definition");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AbcClassName)
                .HasMaxLength(255)
                .HasColumnName("ABC_CLASS_NAME");
            entity.Property(e => e.Attribute3)
                .HasMaxLength(240)
                .HasColumnName("ATTRIBUTE3");
            entity.Property(e => e.BulkItem).HasColumnName("Bulk_item");
            entity.Property(e => e.CountryOfOrigin)
                .HasMaxLength(50)
                .HasColumnName("COUNTRY_OF_ORIGIN");
            entity.Property(e => e.DangerousGoodsIndicator)
                .HasMaxLength(255)
                .HasColumnName("Dangerous_Goods_indicator");
            entity.Property(e => e.DimensionsUom)
                .HasMaxLength(3)
                .HasColumnName("DIMENSIONS_UOM");
            entity.Property(e => e.Eccn)
                .HasMaxLength(255)
                .HasColumnName("ECCN");
            entity.Property(e => e.HtsCode)
                .HasMaxLength(50)
                .HasColumnName("HTS_CODE");
            entity.Property(e => e.InventoryItemId).HasColumnName("Inventory_Item_Id");
            entity.Property(e => e.ItemLifeCycle)
                .HasMaxLength(240)
                .HasColumnName("Item_Life_cycle");
            entity.Property(e => e.ItemLongDescription)
                .HasMaxLength(500)
                .HasColumnName("Item_Long_description");
            entity.Property(e => e.ItemSource)
                .HasMaxLength(150)
                .HasColumnName("Item_source");
            entity.Property(e => e.ItemType)
                .HasMaxLength(30)
                .HasColumnName("Item_type");
            entity.Property(e => e.LastUpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Last_Update_Date");
            entity.Property(e => e.LastUpdatedBy).HasColumnName("LAST_UPDATED_BY");
            entity.Property(e => e.PalletSize).HasColumnName("PALLET_SIZE");
            entity.Property(e => e.PartDsc)
                .HasMaxLength(250)
                .IsFixedLength()
                .HasColumnName("Part_DSC");
            entity.Property(e => e.PartNo).HasMaxLength(50);
            entity.Property(e => e.PrimaryUomCode)
                .HasMaxLength(255)
                .HasColumnName("PRIMARY_UOM_CODE");
            entity.Property(e => e.ProductClass)
                .HasMaxLength(255)
                .HasColumnName("PRODUCT_CLASS");
            entity.Property(e => e.ReceiptRouting)
                .HasMaxLength(80)
                .HasColumnName("RECEIPT_ROUTING");
            entity.Property(e => e.RepairableByDbk).HasColumnName("Repairable_by_DBK");
            entity.Property(e => e.Serialized)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");
            entity.Property(e => e.UnitHeight).HasColumnName("UNIT_HEIGHT");
            entity.Property(e => e.UnitLength).HasColumnName("UNIT_LENGTH");
            entity.Property(e => e.UnitVolume).HasColumnName("UNIT_VOLUME");
            entity.Property(e => e.UnitWeight).HasColumnName("UNIT_WEIGHT");
            entity.Property(e => e.UnitWidth).HasColumnName("UNIT_WIDTH");
            entity.Property(e => e.VolumeUom)
                .HasMaxLength(3)
                .HasColumnName("VOLUME_UOM");
            entity.Property(e => e.WeightUom)
                .HasMaxLength(3)
                .HasColumnName("WEIGHT_UOM");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

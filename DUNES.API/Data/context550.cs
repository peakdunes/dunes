using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context550 : DbContext
{
    public context550()
    {
    }

    public context550(DbContextOptions<context550> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bReplacementPartsInventoryLog> TzebB2bReplacementPartsInventoryLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bReplacementPartsInventoryLog>(entity =>
        {
            entity.ToTable("_TZEB_B2B_Replacement_Parts_Inventory_Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Date_Inserted");
            entity.Property(e => e.InventoryTypeIdDest).HasColumnName("Inventory_Type_id_Dest");
            entity.Property(e => e.InventoryTypeIdSource).HasColumnName("Inventory_Type_id_Source");
            entity.Property(e => e.Notes)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PartDefinitionId).HasColumnName("Part_Definition_id");
            entity.Property(e => e.Qty).HasDefaultValue(1);
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Serial_No");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

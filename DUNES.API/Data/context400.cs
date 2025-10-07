using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory.ASN;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context400 : DbContext
{
    public context400()
    {
    }

    public context400(DbContextOptions<context400> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bIrReceiptLineItemTblItemInbConsReqsLog> TzebB2bIrReceiptLineItemTblItemInbConsReqsLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bIrReceiptLineItemTblItemInbConsReqsLog>(entity =>
        {
            entity.ToTable("_TZEB_B2B_IR_RECEIPT_LINE_ITEM_TBL_ITEM_Inb_Cons_Reqs_Log");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.InventoryItemId).HasColumnName("INVENTORY_ITEM_ID");
            entity.Property(e => e.IrReceiptOutHdrDetItemId).HasColumnName("IR_RECEIPT_OUT_HDR_DET_ITEM_ID");
            entity.Property(e => e.IsCePart).HasColumnName("Is_CE_Part");
            entity.Property(e => e.IsRtvPart).HasColumnName("Is_RTV_Part");
            entity.Property(e => e.ItemNumber)
                .HasMaxLength(40)
                .HasColumnName("ITEM_NUMBER");
            entity.Property(e => e.LineNum).HasColumnName("LINE_NUM");
            entity.Property(e => e.Quantity).HasColumnName("QUANTITY");
            entity.Property(e => e.ReceiptDate).HasColumnName("RECEIPT_DATE");
            entity.Property(e => e.ShipmentLineId).HasColumnName("SHIPMENT_LINE_ID");
            entity.Property(e => e.To3plLocatorStatus)
                .HasMaxLength(10)
                .HasColumnName("TO_3PL_LOCATOR_STATUS");
            entity.Property(e => e.TransactionDate).HasColumnName("TRANSACTION_DATE");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(25)
                .HasColumnName("UNIT_OF_MEASURE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

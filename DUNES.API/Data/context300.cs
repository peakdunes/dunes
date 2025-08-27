using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context300 : DbContext
{
    public context300()
    {
    }

    public context300(DbContextOptions<context300> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bAsnLineItemTblItemInbConsReqs> TzebB2bAsnLineItemTblItemInbConsReqs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bAsnLineItemTblItemInbConsReqs>(entity =>
        {
            entity.ToTable("_TZEB_B2B_ASN_LINE_ITEM_TBL_ITEM_Inb_Cons_Reqs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AsnOutHdrDetItemId).HasColumnName("ASN_OUT_HDR_DET_ITEM_ID");
            entity.Property(e => e.Attribute1)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE1");
            entity.Property(e => e.Attribute10)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE10");
            entity.Property(e => e.Attribute11)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE11");
            entity.Property(e => e.Attribute12)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE12");
            entity.Property(e => e.Attribute13)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE13");
            entity.Property(e => e.Attribute14)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE14");
            entity.Property(e => e.Attribute15)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE15");
            entity.Property(e => e.Attribute2)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE2");
            entity.Property(e => e.Attribute3)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE3");
            entity.Property(e => e.Attribute4)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE4");
            entity.Property(e => e.Attribute5)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE5");
            entity.Property(e => e.Attribute6)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE6");
            entity.Property(e => e.Attribute7)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE7");
            entity.Property(e => e.Attribute8)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE8");
            entity.Property(e => e.Attribute9)
                .HasMaxLength(150)
                .HasColumnName("ATTRIBUTE9");
            entity.Property(e => e.Comments)
                .HasMaxLength(4000)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.ContainerNum)
                .HasMaxLength(30)
                .HasColumnName("CONTAINER_NUM");
            entity.Property(e => e.DateCompleted).HasColumnName("DATE_COMPLETED");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.DateTimeUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Updated");
            entity.Property(e => e.HazardClass)
                .HasMaxLength(40)
                .HasColumnName("HAZARD_CLASS");
            entity.Property(e => e.InventoryItemId).HasColumnName("INVENTORY_ITEM_ID");
            entity.Property(e => e.ItemDescription)
                .HasMaxLength(240)
                .HasColumnName("ITEM_DESCRIPTION");
            entity.Property(e => e.ItemNumber)
                .HasMaxLength(40)
                .HasColumnName("ITEM_NUMBER");
            entity.Property(e => e.LineNum).HasColumnName("LINE_NUM");
            entity.Property(e => e.Locator)
                .HasMaxLength(204)
                .HasColumnName("LOCATOR");
            entity.Property(e => e.LpnInfo)
                .HasMaxLength(100)
                .HasColumnName("LPN_INFO");
            entity.Property(e => e.OrderLineId).HasColumnName("ORDER_LINE_ID");
            entity.Property(e => e.OrderNum).HasColumnName("ORDER_NUM");
            entity.Property(e => e.OrderType)
                .HasMaxLength(30)
                .HasColumnName("ORDER_TYPE");
            entity.Property(e => e.OrganizationCode)
                .HasMaxLength(3)
                .HasColumnName("ORGANIZATION_CODE");
            entity.Property(e => e.PoShipmentNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("PO_SHIPMENT_NUMBER");
            entity.Property(e => e.QuantityReceived)
                .HasDefaultValue(0f)
                .HasColumnName("QUANTITY_RECEIVED");
            entity.Property(e => e.QuantityShipped).HasColumnName("QUANTITY_SHIPPED");
            entity.Property(e => e.ReasonCode)
                .HasMaxLength(50)
                .HasColumnName("REASON_CODE");
            entity.Property(e => e.ReceiptRouting)
                .HasMaxLength(80)
                .HasColumnName("RECEIPT_ROUTING");
            entity.Property(e => e.ReconfiguredAssembly)
                .HasMaxLength(40)
                .HasColumnName("RECONFIGURED_ASSEMBLY");
            entity.Property(e => e.ShipmentLineId).HasColumnName("SHIPMENT_LINE_ID");
            entity.Property(e => e.ShipmentLineStatusCode)
                .HasMaxLength(25)
                .HasColumnName("SHIPMENT_LINE_STATUS_CODE");
            entity.Property(e => e.SubinventoryCode)
                .HasMaxLength(10)
                .HasColumnName("SUBINVENTORY_CODE");
            entity.Property(e => e.TransactionId).HasColumnName("TRANSACTION_ID");
            entity.Property(e => e.TransactionQuantity).HasColumnName("TRANSACTION_QUANTITY");
            entity.Property(e => e.TruckNum)
                .HasMaxLength(35)
                .HasColumnName("TRUCK_NUM");
            entity.Property(e => e.UnNumber)
                .HasMaxLength(25)
                .HasColumnName("UN_NUMBER");
            entity.Property(e => e.UnitOfMeasure)
                .HasMaxLength(25)
                .HasColumnName("UNIT_OF_MEASURE");
            entity.Property(e => e.WipJobName)
                .HasMaxLength(240)
                .HasColumnName("WIP_JOB_NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using APIZEBRA.Models.B2B;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context800 : DbContext
{
    public context800()
    {
    }

    public context800(DbContextOptions<context800> options)
        : base(options)
    {
    }

    public virtual DbSet<TiewRepairStatusZebraBldgReceiving> TiewRepairStatusZebraBldgReceiving { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TiewRepairStatusZebraBldgReceiving>(entity =>
        {
            entity.ToTable("_TIEW_Repair_Status_Zebra_Bldg_Receiving");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Allrepairstate).HasColumnName("ALLREPAIRSTATE");
            entity.Property(e => e.AttHdr)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Att_HDR");
            entity.Property(e => e.BinLocation)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.CanceledDate)
                .HasColumnType("datetime")
                .HasColumnName("Canceled_Date");
            entity.Property(e => e.Chd).HasColumnName("CHD");
            entity.Property(e => e.CloseDate)
                .HasColumnType("datetime")
                .HasColumnName("Close_Date");
            entity.Property(e => e.CompanyDsc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Company_DSC");
            entity.Property(e => e.CompanyPartNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Company_PartNo");
            entity.Property(e => e.CourierDsc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Courier_Dsc");
            entity.Property(e => e.CourierId).HasColumnName("Courier_ID");
            entity.Property(e => e.CurrInstructions)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Curr_instructions");
            entity.Property(e => e.CustName)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Cust_Name");
            entity.Property(e => e.CustRef)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Cust_Ref");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateLoading)
                .HasColumnType("datetime")
                .HasColumnName("Date_Loading");
            entity.Property(e => e.DateReceived)
                .HasColumnType("datetime")
                .HasColumnName("Date_Received");
            entity.Property(e => e.DateRepaired)
                .HasColumnType("datetime")
                .HasColumnName("Date_Repaired");
            entity.Property(e => e.DateShip)
                .HasColumnType("datetime")
                .HasColumnName("Date_Ship");
            entity.Property(e => e.DateTech)
                .HasColumnType("datetime")
                .HasColumnName("Date_Tech");
            entity.Property(e => e.DaysLeftToArrive).HasColumnName("DaysLeft_To_Arrive");
            entity.Property(e => e.DbkPartDsc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("DBK_Part_DSC");
            entity.Property(e => e.Division)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EstimatedAgree).HasColumnName("Estimated_Agree");
            entity.Property(e => e.InBoundpt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InboundTracking)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("Inbound_Tracking");
            entity.Property(e => e.InboundTrackingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.NeedRepair).HasColumnName("Need_Repair");
            entity.Property(e => e.NeedSerialNo).HasColumnName("Need_SerialNo");
            entity.Property(e => e.OrderOnHoldDateAdded).HasColumnName("OrderOnHold_DateAdded");
            entity.Property(e => e.OrderOnHoldReason)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("OrderOnHold_Reason");
            entity.Property(e => e.OrderStateId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Order_State_ID");
            entity.Property(e => e.OrderZipCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("Order_ZipCode");
            entity.Property(e => e.OutBoundpt)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PartDsc)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Part_DSC");
            entity.Property(e => e.PartNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Part_No");
            entity.Property(e => e.PickupStatus).HasColumnName("Pickup_Status");
            entity.Property(e => e.QtyReceived).HasColumnName("Qty_Received");
            entity.Property(e => e.QtyToOrder).HasColumnName("Qty_To_Order");
            entity.Property(e => e.QtyToReceive).HasColumnName("Qty_To_Receive");
            entity.Property(e => e.QtyToShip).HasColumnName("Qty_To_Ship");
            entity.Property(e => e.RefNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Ref_No");
            entity.Property(e => e.RepairDateClose)
                .HasColumnType("datetime")
                .HasColumnName("Repair_Date_Close");
            entity.Property(e => e.RepairNo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Repair_No");
            entity.Property(e => e.RepairNoTrepair).HasColumnName("Repair_No_TRepair");
            entity.Property(e => e.SerialInbound)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SerialINBOUND");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Serial_No");
            entity.Property(e => e.SerialReceived)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SerialRECEIVED");
            entity.Property(e => e.SerialShip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SerialSHIP");
            entity.Property(e => e.ShipToAddr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Ship_To_Addr");
            entity.Property(e => e.ShipToAddr1)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("Ship_To_Addr1");
            entity.Property(e => e.ShippSwap)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SHIPP_SWAP");
            entity.Property(e => e.SparePoolId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Spare_Pool_id");
            entity.Property(e => e.StatusForCompany)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StopDate)
                .HasColumnType("datetime")
                .HasColumnName("Stop_Date");
            entity.Property(e => e.TcityDsc)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TCity_Dsc");
            entity.Property(e => e.TcustNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("TCust_No");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TrepairStateDsc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TRepairState_DSC");
            entity.Property(e => e.TrepairStateId).HasColumnName("TRepairState_ID");
            entity.Property(e => e.TstateId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TState_ID");
            entity.Property(e => e.TstatusId)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TStatusID");
            entity.Property(e => e.UnitId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UnitID");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ZIP_Code");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

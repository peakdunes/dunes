using System;
using System.Collections.Generic;
using APIZEBRA.Models.B2b;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context200 : DbContext
{
    public context200()
    {
    }

    public context200(DbContextOptions<context200> options)
        : base(options)
    {
    }

    public virtual DbSet<TorderRepairHdr> TorderRepairHdr { get; set; }

    public virtual DbSet<TorderRepairItems> TorderRepairItems { get; set; }

    public virtual DbSet<TorderRepairItemsSerialsReceiving> TorderRepairItemsSerialsReceiving { get; set; }

    public virtual DbSet<TorderRepairItemsSerialsShipping> TorderRepairItemsSerialsShipping { get; set; }

    public virtual DbSet<Trepair> Trepair { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TorderRepairHdr>(entity =>
        {
            entity.HasKey(e => e.RefNo)
                .IsClustered(false)
                .HasFillFactor(90);

            entity.ToTable("_TOrderRepair_Hdr", tb =>
                {
                    tb.HasTrigger("Address_Updated");
                    tb.HasTrigger("OrderHdr_INSERT_RECORD");
                    tb.HasTrigger("OrderHdr_RECORD_DELETED");
                    tb.HasTrigger("OrderHdr_UPDATE_RECORD");
                    tb.HasTrigger("WhoUpdateCloseDate");
                    tb.HasTrigger("_TOrderRepair_Hdr_RECORD_UPDATED");
                });

            entity.HasIndex(e => e.DateCreated, "IX__TOrderRepair_Hdr").HasFillFactor(90);

            entity.HasIndex(e => new { e.TcityId, e.TstateId }, "IX__TOrderRepair_Hdr_1");

            entity.HasIndex(e => e.RefNo, "IX__TOrderRepair_Hdr_2");

            entity.HasIndex(e => e.StopDate, "IX__TOrderRepair_Hdr_3");

            entity.HasIndex(e => new { e.CloseDate, e.RefNo, e.TcustNo, e.DateCreated }, "IX__TOrderRepair_Hdr_4");

            entity.HasIndex(e => new { e.RefNo, e.DateCreated }, "IX__TOrderRepair_Hdr_5");

            entity.HasIndex(e => new { e.RefNo, e.TcustNo, e.DateCreated }, "IX__TOrderRepair_Hdr_6");

            entity.HasIndex(e => e.PoRefNo, "IX__TOrderRepair_Hdr_7");

            entity.HasIndex(e => e.CloseDate, "IX__TOrderRepair_Hdr_Close_Date");

            entity.HasIndex(e => new { e.CanceledDate, e.StopDate, e.CloseDate }, "Ref_No_IDX");

            entity.HasIndex(e => e.CanceledDate, "TOrderRepair_Hdr_Canceled_Date_Index>");

            entity.HasIndex(e => e.TcustNo, "_TOrderRepair_Hdr10").HasFillFactor(90);

            entity.HasIndex(e => new { e.TcustNo, e.RefNo, e.DateCreated, e.CustRef, e.CustName, e.ShipToAddr, e.CanceledDate, e.CloseDate }, "_TOrderRepair_Hdr17").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.TcustNo }, "_TOrderRepair_Hdr2")
                .IsClustered()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.TcustNo, e.CanceledDate, e.StopDate }, "_TOrderRepair_Hdr9").HasFillFactor(90);

            entity.HasIndex(e => e.CanceledDate, "_TOrderRepair_Hdr999").HasFillFactor(90);

            entity.HasIndex(e => new { e.DateCreated, e.RefNo, e.TcustNo }, "_dta_index__TOrderRepair_Hdr_7_1461684355__K2_K1_K3_4_6");

            entity.HasIndex(e => new { e.CanceledDate, e.CloseDate, e.TcustNo, e.RefNo }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K17_K19_K3_K1_4_5_11");

            entity.HasIndex(e => new { e.StopDate, e.CanceledDate }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K18_K17");

            entity.HasIndex(e => new { e.RefNo, e.CloseDate, e.StopDate, e.TcustNo }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K1_K19_K18_K3_5_13_17_21_2203");

            entity.HasIndex(e => new { e.RefNo, e.TcustNo }, "_dta_index__TOrderRepair_Hdr_8_1461684355__K1_K3_8258");

            entity.Property(e => e.RefNo)
                .ValueGeneratedNever()
                .HasColumnName("Ref_No");
            entity.Property(e => e.Att)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BinLocation)
                .HasMaxLength(8)
                .IsUnicode(false);
            entity.Property(e => e.CanceledDate)
                .HasColumnType("datetime")
                .HasColumnName("Canceled_Date");
            entity.Property(e => e.CloseDate)
                .HasColumnType("datetime")
                .HasColumnName("Close_Date");
            entity.Property(e => e.CourierId).HasColumnName("Courier_Id");
            entity.Property(e => e.CreatedOnline)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValue("N");
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
            entity.Property(e => e.DateSaved)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Date_Saved");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue("Not Provided");
            entity.Property(e => e.EmailResponseDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.EmailResponseSource).HasMaxLength(50);
            entity.Property(e => e.EstimateRequired).HasColumnName("Estimate_Required");
            entity.Property(e => e.OriginId).HasColumnName("OriginID");
            entity.Property(e => e.PoRefNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PO_RefNo");
            entity.Property(e => e.ReceiverUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReceivingCompName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReceivingEndDate).HasColumnType("datetime");
            entity.Property(e => e.ReceivingStartDate).HasColumnType("datetime");
            entity.Property(e => e.ShipToAddr)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasColumnName("Ship_To_Addr");
            entity.Property(e => e.ShipToAddr1)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasColumnName("Ship_To_Addr1");
            entity.Property(e => e.ShipViaId).HasColumnName("ShipVia_Id");
            entity.Property(e => e.ShippingInstr)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SonimCustType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SonimOperCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StopDate)
                .HasColumnType("datetime")
                .HasColumnName("Stop_Date");
            entity.Property(e => e.TcityId)
                .HasDefaultValueSql("(' ')")
                .HasColumnName("TCity_ID");
            entity.Property(e => e.TcustNo)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("TCust_No");
            entity.Property(e => e.TechNotes)
                .HasMaxLength(7000)
                .IsUnicode(false)
                .HasColumnName("Tech_Notes");
            entity.Property(e => e.TstateId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasColumnName("TState_ID");
            entity.Property(e => e.TstatusId)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TStatusID");
            entity.Property(e => e.ZipCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue(" ")
                .HasColumnName("ZIP_Code");
        });

        modelBuilder.Entity<TorderRepairItems>(entity =>
        {
            entity.HasKey(e => new { e.RefNo, e.PartNo })
                .IsClustered(false)
                .HasFillFactor(90);

            entity.ToTable("_TOrderRepair_Items");

            entity.HasIndex(e => e.CompanyPartNo, "IX__TOrderRepair_Items");

            entity.HasIndex(e => new { e.RefNo, e.PartNo, e.CompanyPartNo }, "IX__TOrderRepair_Items_1");

            entity.HasIndex(e => new { e.CompanyPartNo, e.PartNo }, "IX__TOrderRepair_Items_2");

            entity.HasIndex(e => new { e.RefNo, e.CompanyPartNo }, "IX__TOrderRepair_Items_3");

            entity.HasIndex(e => e.RefNo, "_TOrderRepair_Items3")
                .IsClustered()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.PartNo, e.RefNo }, "_dta_index__TOrderRepair_Items_8_1165963230__K2_K1_3");

            entity.Property(e => e.RefNo).HasColumnName("Ref_No");
            entity.Property(e => e.PartNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_No");
            entity.Property(e => e.CompanyPartNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Company_PartNo");

            entity.HasOne(d => d.RefNoNavigation).WithMany(p => p.TorderRepairItems)
                .HasForeignKey(d => d.RefNo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TOrderRepair_Items__TOrderRepair_Hdr");
        });

        modelBuilder.Entity<TorderRepairItemsSerialsReceiving>(entity =>
        {
            entity.HasKey(e => new { e.RefNo, e.PartNo, e.Id })
                .HasName("PK__TorderRepair_ItemsSerials_Receiving_194")
                .IsClustered(false)
                .HasFillFactor(90);

            entity.ToTable("_TorderRepair_ItemsSerials_Receiving", tb =>
                {
                    tb.HasTrigger("Receiving_INSERT_RECORD_194");
                    tb.HasTrigger("Receiving_RECORD_DELETED_194");
                    tb.HasTrigger("Receiving_UPDATE_RECORD_194");
                    tb.HasTrigger("Update_Estimated_Agree_date_194");
                });

            entity.HasIndex(e => e.DateReceived, "Date_Received_IDX_194");

            entity.HasIndex(e => e.DateReceived, "Date_Received_Ref_No_Repair_No_TrackingNumber_Idx_194");

            entity.HasIndex(e => e.RepairNo, "IX__REPAIR_194").HasFillFactor(90);

            entity.HasIndex(e => e.DateReceived, "IX__TDATE_RCV_194").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_10_194");

            entity.HasIndex(e => new { e.RepairNo, e.TrackingNumber }, "IX__TorderRepair_ItemsSerials_Receiving_11_194");

            entity.HasIndex(e => e.EstimatedAgree, "IX__TorderRepair_ItemsSerials_Receiving_12_194");

            entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.CourierId }, "IX__TorderRepair_ItemsSerials_Receiving_15_194");

            entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.TstatusId, e.EstimatedAgree, e.SerialReceived, e.DateReceived }, "IX__TorderRepair_ItemsSerials_Receiving_16_194");

            entity.HasIndex(e => e.PartNo, "IX__TorderRepair_ItemsSerials_Receiving_194").HasFillFactor(90);

            entity.HasIndex(e => e.TstatusId, "IX__TorderRepair_ItemsSerials_Receiving_1_194").HasFillFactor(90);

            entity.HasIndex(e => new { e.PartNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_2_194");

            entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_3_194");

            entity.HasIndex(e => new { e.RepairNo, e.PartNo }, "IX__TorderRepair_ItemsSerials_Receiving_4_194");

            entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_7_194");

            entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.TstatusId }, "IX__TorderRepair_ItemsSerials_Receiving_8_194");

            entity.HasIndex(e => new { e.RepairNo, e.SerialReceived }, "IX__TorderRepair_ItemsSerials_Receiving_9_194");

            entity.HasIndex(e => e.RefNo, "NonClusteredIndex-20181206-100954_194");

            entity.HasIndex(e => e.SparePoolId, "NonClusteredIndex-20191025-005759_194");

            entity.HasIndex(e => new { e.PartNo, e.RefNo, e.SerialReceived, e.Id }, "_TorderRepair_ItemsSerials_Receiving13_194").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.RepairNo }, "_TorderRepair_ItemsSerials_Receiving15_194")
                .IsClustered()
                .HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.DateReceived, e.SerialReceived, e.RepairNo }, "_TorderRepair_ItemsSerials_Receiving16_194").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.DateReceived }, "_TorderRepair_ItemsSerials_Receiving17_194").HasFillFactor(90);

            entity.HasIndex(e => e.TstatusId, "_TorderRepair_ItemsSerials_Receiving_TStatusID_Index_194");

            entity.Property(e => e.RefNo).HasColumnName("Ref_No");
            entity.Property(e => e.PartNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_No");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ConfirmationDate).HasColumnType("datetime");
            entity.Property(e => e.CourierId).HasColumnName("Courier_ID");
            entity.Property(e => e.DateReceived)
                .HasColumnType("datetime")
                .HasColumnName("Date_Received");
            entity.Property(e => e.DateTimeReceived)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Received");
            entity.Property(e => e.EstimatedAgree)
                .HasDefaultValue(false)
                .HasColumnName("Estimated_Agree");
            entity.Property(e => e.EstimatedAgreeDate)
                .HasColumnType("datetime")
                .HasColumnName("Estimated_Agree_date");
            entity.Property(e => e.InWaranty).HasDefaultValue(false);
            entity.Property(e => e.IsObf)
                .HasDefaultValue(false)
                .HasColumnName("Is_OBF");
            entity.Property(e => e.KitNo).HasColumnName("Kit_No");
            entity.Property(e => e.ObfApprovedBy)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("OBF_ApprovedBy");
            entity.Property(e => e.ObfDateApproved)
                .HasColumnType("datetime")
                .HasColumnName("OBF_DateApproved");
            entity.Property(e => e.PriceEstimated).HasColumnName("Price_Estimated");
            entity.Property(e => e.ProjectName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.QtyReceived)
                .HasDefaultValue(0)
                .HasColumnName("Qty_Received");
            entity.Property(e => e.RepairLogPrinted)
                .HasDefaultValue(false)
                .HasColumnName("RepairLog_Printed");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.SerialInbound)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SerialINBOUND");
            entity.Property(e => e.SerialReceived)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("SerialRECEIVED");
            entity.Property(e => e.SparePoolId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Spare_Pool_id");
            entity.Property(e => e.TechEvaluationDone).HasDefaultValue(false);
            entity.Property(e => e.TechNotes)
                .HasMaxLength(7000)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("Tech_Notes");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TstatusId)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("TStatusID");
            entity.Property(e => e.TypeOfRepairId)
                .HasDefaultValue(1)
                .HasComment("Codifier: _TDBK_TypeOfRepair")
                .HasColumnName("TypeOfRepair_ID");
        });

        modelBuilder.Entity<TorderRepairItemsSerialsShipping>(entity =>
        {
            entity.HasKey(e => new { e.RefNo, e.PartNo, e.Id }).HasFillFactor(90);

            entity.ToTable("_TorderRepair_ItemsSerials_Shipping", tb =>
                {
                    tb.HasTrigger("Shipping_INSERT_RECORD");
                    tb.HasTrigger("Shipping_RECORD_DELETED");
                    tb.HasTrigger("Shipping_UPDATE_RECORD");
                });

            entity.HasIndex(e => e.TstatusId, "<Name of Missing Index, sysname,>");

            entity.HasIndex(e => e.DateShip, "Date_Ship_IDX");

            entity.HasIndex(e => e.DateShip, "Date_Ship_IDX1");

            entity.HasIndex(e => new { e.DateShip, e.TrackingNumber }, "Date_Ship_TrackingNumber_IDX");

            entity.HasIndex(e => e.DateShip, "IX__TorderRepair_ItemsSerials_Shipping").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.DateShip }, "IX__TorderRepair_ItemsSerials_Shipping_1").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.SerialShip }, "IX__TorderRepair_ItemsSerials_Shipping_10").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.SerialShip, e.DateShip }, "IX__TorderRepair_ItemsSerials_Shipping_11").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.PartNo }, "IX__TorderRepair_ItemsSerials_Shipping_2").HasFillFactor(90);

            entity.HasIndex(e => new { e.SerialShip, e.DateShip }, "IX__TorderRepair_ItemsSerials_Shipping_3").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.PartNo }, "IX__TorderRepair_ItemsSerials_Shipping_4").HasFillFactor(90);

            entity.HasIndex(e => e.RepairNo, "IX__TorderRepair_ItemsSerials_Shipping_5").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.RepairNo }, "IX__TorderRepair_ItemsSerials_Shipping_6").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.TstatusId }, "IX__TorderRepair_ItemsSerials_Shipping_7").HasFillFactor(90);

            entity.HasIndex(e => new { e.RefNo, e.RepairNo, e.PartNo, e.DateShip, e.SerialShip, e.TstatusId }, "IX__TorderRepair_ItemsSerials_Shipping_8").HasFillFactor(90);

            entity.HasIndex(e => e.PartNo, "IX__TorderRepair_ItemsSerials_Shipping_9").HasFillFactor(90);

            entity.HasIndex(e => new { e.SerialShip, e.TstatusId }, "SerialSHIP_TStatusID_idx");

            entity.HasIndex(e => new { e.RefNo, e.PartNo, e.DateShip, e.RepairNo }, "_TorderRepair_ItemsSerials_Shipping27").HasFillFactor(90);

            entity.HasIndex(e => new { e.DateShip, e.TstatusId }, "_TorderRepair_ItemsSerials_Shipping_Index");

            entity.HasIndex(e => new { e.RefNo, e.DateShip, e.PartNo }, "_dta_index__TorderRepair_ItemsSerials_Shipp_8_453628709__K1_K3_K2_8").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.RefNo }, "_dta_index__TorderRepair_ItemsSerials_Shipp_8_453628709__K6_K1_2894").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.PartNo, e.RefNo, e.Id }, "_dta_index__TorderRepair_ItemsSerials_Shipp_8_453628709__K6_K2_K1_K10_3_4_5_7_8_11_12").HasFillFactor(90);

            entity.Property(e => e.RefNo).HasColumnName("Ref_No");
            entity.Property(e => e.PartNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_No");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateShip)
                .HasColumnType("datetime")
                .HasColumnName("Date_Ship");
            entity.Property(e => e.DateTimeShip)
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Ship");
            entity.Property(e => e.DateTrackingNumber).HasColumnType("datetime");
            entity.Property(e => e.DaysToPickupFromShipping).HasDefaultValue(0);
            entity.Property(e => e.NeedSoftwareLoading).HasDefaultValue(false);
            entity.Property(e => e.Qty).HasDefaultValue(0);
            entity.Property(e => e.QtyShip)
                .HasDefaultValue(0)
                .HasColumnName("Qty_Ship");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.SerialShip)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SerialSHIP");
            entity.Property(e => e.ShipViaId).HasColumnName("ShipVia_Id");
            entity.Property(e => e.ShippingGroupId).HasColumnName("Shipping_Group_ID");
            entity.Property(e => e.TrackingNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TstatusId)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasDefaultValue("")
                .HasColumnName("TStatusID");
            entity.Property(e => e.UserName)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("User_Name");
        });

        modelBuilder.Entity<Trepair>(entity =>
        {
            entity.HasKey(e => e.RepairNo).HasFillFactor(90);

            entity.ToTable("_TRepair", tb =>
                {
                    tb.HasTrigger("UpdateOrderStatus");
                    tb.HasTrigger("Update_TDBK_Repair_Status");
                    tb.HasTrigger("_TRepair_INSERT_RECORD");
                    tb.HasTrigger("_TRepair_RECORD_DELETED");
                    tb.HasTrigger("_TRepair_UPDATE_RECORD");
                });

            entity.HasIndex(e => e.Division, "IX__Division").HasFillFactor(90);

            entity.HasIndex(e => e.QualityEmployeeNo, "IX__Quality_Employee_No").HasFillFactor(90);

            entity.HasIndex(e => e.RefNo, "IX__TRefNo").HasFillFactor(90);

            entity.HasIndex(e => new { e.SerialNo, e.DateRepaired, e.Division }, "IX__TRepair").HasFillFactor(90);

            entity.HasIndex(e => e.DateRepaired, "IX__TRepair_1").HasFillFactor(90);

            entity.HasIndex(e => e.DateClose, "IX__TRepair_10").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.DateClose }, "IX__TRepair_11").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.DateCreated }, "IX__TRepair_12").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.RefNo, e.DateCreated }, "IX__TRepair_13").HasFillFactor(90);

            entity.HasIndex(e => e.TtechNo, "IX__TRepair_2").HasFillFactor(90);

            entity.HasIndex(e => e.PartNo, "IX__TRepair_3").HasFillFactor(90);

            entity.HasIndex(e => e.TrepairStateId, "IX__TRepair_4").HasFillFactor(90);

            entity.HasIndex(e => new { e.RepairNo, e.TtechNo }, "IX__TRepair_6").HasFillFactor(90);

            entity.HasIndex(e => e.DateCreated, "IX__TRepair_8").HasFillFactor(90);

            entity.HasIndex(e => new { e.Division, e.RepairNo, e.SerialNo, e.DateRepaired }, "_TRepair2").HasFillFactor(90);

            entity.HasIndex(e => new { e.Division, e.RepairNo, e.PartNo, e.DateCreated, e.DateRepaired, e.RepairTime, e.TrepairStateId }, "_TRepair5").HasFillFactor(90);

            entity.HasIndex(e => new { e.TrepairStateId, e.RepairNo }, "_dta_index__TRepair_5_1030346785__K15_K1_2_3_7_8_9_17_25_1973_4801_6980");

            entity.HasIndex(e => new { e.TrepairStateId, e.RepairNo }, "_dta_index__TRepair_5_1030346785__K15_K1_3982_3928");

            entity.HasIndex(e => e.RepairNo, "_dta_index__TRepair_5_1030346785__K1_15_4009");

            entity.HasIndex(e => new { e.CompanyDsc, e.RepairNo }, "_dta_index__TRepair_5_1030346785__K5_K1_15_3088");

            entity.Property(e => e.RepairNo)
                .ValueGeneratedNever()
                .HasColumnName("Repair_No");
            entity.Property(e => e.CompanyDsc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Company_DSC");
            entity.Property(e => e.DateClose)
                .HasColumnType("datetime")
                .HasColumnName("Date_Close");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("Date_Created");
            entity.Property(e => e.DateCs)
                .HasColumnType("datetime")
                .HasColumnName("Date_CS");
            entity.Property(e => e.DateLoading)
                .HasColumnType("datetime")
                .HasColumnName("Date_Loading");
            entity.Property(e => e.DateRepaired)
                .HasColumnType("datetime")
                .HasColumnName("Date_Repaired");
            entity.Property(e => e.DateTech)
                .HasColumnType("datetime")
                .HasColumnName("Date_Tech");
            entity.Property(e => e.DateWorkUnassigned)
                .HasComment("Date supervisor removed work from technician")
                .HasColumnType("datetime");
            entity.Property(e => e.DeviceConfigId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Device_Config_ID");
            entity.Property(e => e.Division)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.InvoiceNo).HasColumnName("Invoice_No");
            entity.Property(e => e.NoRechazos)
                .HasDefaultValue(0)
                .HasColumnName("No_Rechazos");
            entity.Property(e => e.PartNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_No");
            entity.Property(e => e.QualityEmployeeNo).HasColumnName("Quality_Employee_No");
            entity.Property(e => e.RealDateTimeCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RefNo).HasColumnName("Ref_No");
            entity.Property(e => e.RepairBoxNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasComment("In the case repair is put into a box or bin (ZEBRA)")
                .HasColumnName("Repair_Box_No");
            entity.Property(e => e.RepairTime).HasColumnName("Repair_Time");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Serial_No");
            entity.Property(e => e.TrepairStateId).HasColumnName("TRepairState_ID");
            entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
            entity.Property(e => e.WfpBin)
                .HasMaxLength(50)
                .HasColumnName("WFP_BIN");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using APIZEBRA.Models.B2B;
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

    public virtual DbSet<MvcChangeAreaLogs> MvcChangeAreaLogs { get; set; }

    public virtual DbSet<TrepairActionsLog> TrepairActionsLog { get; set; }

    public virtual DbSet<TzebB2bOutBoundRequestsLog> TzebB2bOutBoundRequestsLog { get; set; }

    public virtual DbSet<TzebB2bReplacedPartLabel> TzebB2bReplacedPartLabel { get; set; }

    public virtual DbSet<TzebRepairCodes> TzebRepairCodes { get; set; }

    public virtual DbSet<TzebRepairCodesPartNo> TzebRepairCodesPartNo { get; set; }

    public virtual DbSet<UserMvcAssignments> UserMvcAssignments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MvcChangeAreaLogs>(entity =>
        {
            entity.Property(e => e.ActualArea).HasMaxLength(200);
            entity.Property(e => e.ChangeDate).HasColumnName("changeDate");
            entity.Property(e => e.Lastchange).HasColumnName("lastchange");
            entity.Property(e => e.Repairid).HasColumnName("repairid");
            entity.Property(e => e.UserChange)
                .HasMaxLength(200)
                .HasColumnName("userChange");
        });

        modelBuilder.Entity<TrepairActionsLog>(entity =>
        {
            entity.HasKey(e => e.Id)
                .HasName("PK__TRepairs_Assigned_To_Technicians")
                .HasFillFactor(90);

            entity.ToTable("_TRepair_Actions_Log", tb => tb.HasTrigger("_TRepair_Actions_Log_INSERT_RECORD"));

            entity.HasIndex(e => e.RepairNo, "IX__TRepair_Actions_Log").HasFillFactor(90);

            entity.HasIndex(e => e.ActionDate, "IX__TRepair_Actions_Log_1")
                .IsDescending()
                .HasFillFactor(90);

            entity.HasIndex(e => e.RepairNo, "_dta_index__TRepair_Actions_Log_5_216569497__K3_4_742");

            entity.HasIndex(e => new { e.RepairNo, e.ActionDate }, "_dta_index__TRepair_Actions_Log_5_216569497__K3_K5_1973_4801");

            entity.HasIndex(e => new { e.RepairNo, e.ActionDate, e.ActionId, e.Id }, "_dta_index__TRepair_Actions_Log_5_216569497__K3_K5_K4_K1_1623");

            entity.HasIndex(e => new { e.ActionId, e.Id, e.RepairNo, e.ActionDate }, "_dta_index__TRepair_Actions_Log_5_216569497__K4_K1_K3_K5_7646");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ActionId).HasColumnName("ActionID");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
        });

        modelBuilder.Entity<TzebB2bOutBoundRequestsLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_OutBound_Request_Log");

            entity.ToTable("_TZEB_B2B_OutBound_Requests_Log", tb => tb.HasTrigger("AFTER_INSERT_RECORD"));

            entity.HasIndex(e => e.RepairNo, "IX__TZEB_B2B_OutBound_Requests_Log_Repair_No");

            entity.HasIndex(e => e.AckResult, "NonClusteredIndex-20191002-153537");

            entity.HasIndex(e => e.InProcess, "NonClusteredIndex-20191017-111002");

            entity.HasIndex(e => e.NotificationDateTime, "NonClusteredIndex-20191017-111104");

            entity.HasIndex(e => e.NotificationSent, "_TZEB_B2B_OutBound_Requests_Log_b2b");

            entity.Property(e => e.AckDateTime)
                .HasColumnType("datetime")
                .HasColumnName("Ack_Date_Time");
            entity.Property(e => e.AckReceived).HasColumnName("Ack_Received");
            entity.Property(e => e.AckResult)
                .HasMaxLength(50)
                .HasColumnName("Ack_Result");
            entity.Property(e => e.AckXml).HasColumnName("Ack_XML");
            entity.Property(e => e.AdditionalInfo).HasColumnName("Additional_info");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Date_Time_Inserted");
            entity.Property(e => e.InProcess).HasColumnName("In_Process");
            entity.Property(e => e.NotificationDateTime)
                .HasColumnType("datetime")
                .HasColumnName("Notification_Date_Time");
            entity.Property(e => e.NotificationSent).HasColumnName("Notification_Sent");
            entity.Property(e => e.NotificationXml).HasColumnName("Notification_XML");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.Srlnumber)
                .HasMaxLength(255)
                .HasColumnName("SRLNumber");
            entity.Property(e => e.SuccessCall)
                .HasDefaultValue(true)
                .HasColumnName("Success_Call");
            entity.Property(e => e.TypeOfCallId).HasColumnName("Type_Of_Call_Id");
        });

        modelBuilder.Entity<TzebB2bReplacedPartLabel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_ReplacedPartLabel");

            entity.ToTable("_TZEB_B2B_ReplacedPart_Label", tb => tb.HasTrigger("trgReplacedPartLabelDelete"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateDcr1)
                .HasColumnType("datetime")
                .HasColumnName("DateDCR1");
            entity.Property(e => e.DateInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateLabelPrinted).HasColumnType("datetime");
            entity.Property(e => e.PartNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_No");
            entity.Property(e => e.PartRunner)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Part_Runner");
            entity.Property(e => e.ProblemDesc)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("Problem_Desc");
            entity.Property(e => e.RepId)
                .HasComment("_TZEB_REPAIR_CODES_PART_NO - Rep_ID - To get the part associated to this one being removed")
                .HasColumnName("Rep_ID");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.RtvType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RTV_TYPE");
            entity.Property(e => e.SerialNo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Serial_No");
            entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
        });

        modelBuilder.Entity<TzebRepairCodes>(entity =>
        {
            entity.HasKey(e => e.RepId);

            entity.ToTable("_TZEB_REPAIR_CODES");

            entity.HasIndex(e => e.RepairNo, "RepID_TTechNo_DateAdd_J_IDX");

            entity.HasIndex(e => e.WorkCodeAction, "_TZEB_REPAIR_CODES_Work_Code_Action_Index");

            entity.Property(e => e.RepId).HasColumnName("Rep_ID");
            entity.Property(e => e.Ber)
                .HasDefaultValue(false)
                .HasColumnName("BER");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateSubmitted)
                .HasComment("Date super tech click on SAVE AND EXIT, making request visible to part runners (if parts were requested)")
                .HasColumnType("datetime")
                .HasColumnName("Date_Submitted");
            entity.Property(e => e.FaultCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Fault_Code");
            entity.Property(e => e.PrimaryFault)
                .HasDefaultValue(true)
                .HasComment("Primary_Fault = TRUE means this is the main fault for device, the best describing the problem selected by customer   ")
                .HasColumnName("Primary_Fault");
            entity.Property(e => e.RepairNo).HasColumnName("Repair_No");
            entity.Property(e => e.TtechNo).HasColumnName("TTech_No");
            entity.Property(e => e.WorkCodeAction)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Work_Code_Action");
            entity.Property(e => e.WorkCodeTarget)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Work_Code_Target");
        });

        modelBuilder.Entity<TzebRepairCodesPartNo>(entity =>
        {
            entity.HasKey(e => e.RepId).HasName("PK__TZEB_REPAIR_CODES_PART_NO_1");

            entity.ToTable("_TZEB_REPAIR_CODES_PART_NO");

            entity.HasIndex(e => e.PartNo, "<Name of Missing Index, sysname,>");

            entity.Property(e => e.RepId)
                .ValueGeneratedNever()
                .HasColumnName("Rep_ID");
            entity.Property(e => e.Cost).HasColumnType("numeric(18, 2)");
            entity.Property(e => e.PartNo)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Part_No");
        });

        modelBuilder.Entity<UserMvcAssignments>(entity =>
        {
            entity.Property(e => e.ActualArea).HasMaxLength(100);
            entity.Property(e => e.Assigneddate).HasColumnName("assigneddate");
            entity.Property(e => e.Bypass)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),0))")
                .HasColumnName("bypass");
            entity.Property(e => e.Bypassdate).HasColumnName("bypassdate");
            entity.Property(e => e.Duedate).HasColumnName("duedate");
            entity.Property(e => e.Repairid).HasColumnName("repairid");
            entity.Property(e => e.Sendalert)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),0))")
                .HasColumnName("sendalert");
            entity.Property(e => e.Userassigned)
                .HasMaxLength(100)
                .HasColumnName("userassigned");
            entity.Property(e => e.Userbypass)
                .HasMaxLength(100)
                .HasColumnName("userbypass");
            entity.Property(e => e.Userprocess)
                .HasMaxLength(100)
                .HasColumnName("userprocess");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

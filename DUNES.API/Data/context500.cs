using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context500 : DbContext
{
    public context500()
    {
    }

    public context500(DbContextOptions<context500> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bInbConsReqs> TzebB2bInbConsReqs { get; set; }

    public virtual DbSet<TzebB2bOutConsReqs> TzebB2bOutConsReqs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bInbConsReqs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_Inbound_Consignment_Requests");

            entity.ToTable("_TZEB_B2B_Inb_Cons_Reqs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Inserted");
            entity.Property(e => e.Edistandard)
                .HasMaxLength(10)
                .HasColumnName("EDIStandard");
            entity.Property(e => e.FileType).HasMaxLength(100);
            entity.Property(e => e.FullXml).HasColumnName("fullXML");
            entity.Property(e => e.PErrorEmail)
                .HasMaxLength(500)
                .HasColumnName("P_ERROR_EMAIL");
            entity.Property(e => e.PSuccessEmail)
                .HasMaxLength(500)
                .HasColumnName("P_SUCCESS_EMAIL");
            entity.Property(e => e.ReceiverCode).HasMaxLength(10);
            entity.Property(e => e.ResponseLevel).HasMaxLength(500);
            entity.Property(e => e.SenderCode).HasMaxLength(10);
            entity.Property(e => e.SentTimestamp).HasColumnType("datetime");
            entity.Property(e => e.TransactionCode).HasMaxLength(100);
            entity.Property(e => e.UniqueMessageIdentifier).HasMaxLength(500);
        });

        modelBuilder.Entity<TzebB2bOutConsReqs>(entity =>
        {
            entity.ToTable("_TZEB_B2B_Out_Cons_Reqs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AckReceived)
                .HasDefaultValue(false)
                .HasColumnName("Ack_Received");
            entity.Property(e => e.Additional).HasMaxLength(100);
            entity.Property(e => e.AdditionalInfo).HasColumnName("Additional_info");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Date_Time_Inserted");
            entity.Property(e => e.DateTimeSent)
                .HasColumnType("datetime")
                .HasColumnName("Date_Time_Sent");
            entity.Property(e => e.Edistandard)
                .HasMaxLength(10)
                .HasColumnName("EDIStandard");
            entity.Property(e => e.FileType).HasMaxLength(100);
            entity.Property(e => e.FullXmlsent).HasColumnName("fullXMLsent");
            entity.Property(e => e.InProcess)
                .HasDefaultValue(true)
                .HasColumnName("In_Process");
            entity.Property(e => e.ReceiverCode).HasMaxLength(10);
            entity.Property(e => e.ResponseLevel).HasMaxLength(500);
            entity.Property(e => e.ResponseXml).HasColumnName("ResponseXML");
            entity.Property(e => e.SenderCode).HasMaxLength(10);
            entity.Property(e => e.SentTimestamp).HasColumnType("datetime");
            entity.Property(e => e.TransactionCode).HasMaxLength(100);
            entity.Property(e => e.TypeOfCallId).HasColumnName("Type_Of_Call_id");
            entity.Property(e => e.UniqueMessageIdentifier).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

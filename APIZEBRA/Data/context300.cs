using System;
using System.Collections.Generic;
using APIZEBRA.Models.B2B;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context300 : DbContext
{
    public context300()
    {
    }

    public context300(DbContextOptions<context300> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebInBoundRequestsFileHoldsLog> TzebInBoundRequestsFileHoldsLog { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebInBoundRequestsFileHoldsLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_InBound_Requests_File_Holds_Log_1");

            entity.ToTable("_TZEB_InBound_Requests_File_Holds_Log");

            entity.HasIndex(e => e.HoldType, "NonClusteredIndex-20191017-113506");

            entity.HasIndex(e => e.RowId, "NonClusteredIndex-20191017-113642");

            entity.HasIndex(e => e.DateReleased, "_TZEB_InBound_Requests_File_Holds_Log_Index");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOnHold)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Date_On_Hold");
            entity.Property(e => e.DateReleased)
                .HasColumnType("datetime")
                .HasColumnName("Date_Released");
            entity.Property(e => e.HoldId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Hold_Id");
            entity.Property(e => e.HoldName)
                .HasMaxLength(255)
                .HasColumnName("Hold_Name");
            entity.Property(e => e.HoldType)
                .HasMaxLength(255)
                .HasColumnName("Hold_Type");
            entity.Property(e => e.RowId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROW_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

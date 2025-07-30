using System;
using System.Collections.Generic;
using DUNES.API.Models.Masters;
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

    public virtual DbSet<TzebB2bOutBoundRequestsTypeOfCalls> TzebB2bOutBoundRequestsTypeOfCalls { get; set; }

    public virtual DbSet<TzebWorkCodesActions> TzebWorkCodesActions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bOutBoundRequestsTypeOfCalls>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TZEB_B2B_OutBound_Request_Type_Of_Calls");

            entity.ToTable("_TZEB_B2B_OutBound_Requests_Type_Of_Calls");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TzebWorkCodesActions>(entity =>
        {
            entity.HasKey(e => e.WorkCodeAction);

            entity.ToTable("_TZEB_WORK_CODES_ACTIONS");

            entity.Property(e => e.WorkCodeAction)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Work_Code_Action");
            entity.Property(e => e.RepairCodeDefinition)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Repair_Code_Definition");
            entity.Property(e => e.RequiresAssemblingArea)
                .HasDefaultValue(true)
                .HasColumnName("Requires_Assembling_Area");
            entity.Property(e => e.RequiresPartsReplaced).HasColumnName("Requires_Parts_Replaced");
            entity.Property(e => e.Show).HasDefaultValue(true);
            entity.Property(e => e.WorkDescAction)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Work_Desc_Action");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

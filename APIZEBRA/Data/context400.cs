using System;
using System.Collections.Generic;
using APIZEBRA.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context400 : DbContext
{
    public context400()
    {
    }

    public context400(DbContextOptions<context400> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebWorkCodesTargets> TzebWorkCodesTargets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebWorkCodesTargets>(entity =>
        {
            entity.HasKey(e => e.WorkCodeTarget);

            entity.ToTable("_TZEB_WORK_CODES_TARGETS");

            entity.Property(e => e.WorkCodeTarget)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Work_Code_Target");
            entity.Property(e => e.ConsideredForBer)
                .HasDefaultValue(false)
                .HasColumnName("Considered_For_BER");
            entity.Property(e => e.Show).HasDefaultValue(true);
            entity.Property(e => e.WorkDescTarget)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Work_Desc_Target");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

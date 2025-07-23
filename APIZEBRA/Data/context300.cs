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

    public virtual DbSet<MvcRepairPreflash> MvcRepairPreflash { get; set; }

    public virtual DbSet<UserMvcReceiving> UserMvcReceiving { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MvcRepairPreflash>(entity =>
        {
            entity.ToTable("mvcRepairPreflash");

            entity.Property(e => e.Dateprocess).HasColumnName("dateprocess");
            entity.Property(e => e.Datereceive).HasColumnName("datereceive");
            entity.Property(e => e.Repairid).HasColumnName("repairid");
            entity.Property(e => e.TechFingerprint)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0),0))")
                .HasColumnName("techFingerprint");
            entity.Property(e => e.Techprevious).HasColumnName("techprevious");
            entity.Property(e => e.User).HasColumnName("user");
        });

        modelBuilder.Entity<UserMvcReceiving>(entity =>
        {
            entity.ToTable("userMvcReceiving");

            entity.Property(e => e.Repairno).HasColumnName("repairno");
            entity.Property(e => e.User)
                .HasMaxLength(100)
                .HasColumnName("user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

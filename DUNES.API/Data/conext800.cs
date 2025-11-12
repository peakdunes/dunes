using System;
using System.Collections.Generic;
using DUNES.API.Models.WebService;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class conext800 : DbContext
{
    public conext800()
    {
    }

    public conext800(DbContextOptions<conext800> options)
        : base(options)
    {
    }

    public virtual DbSet<MvcWebServiceDailySummary> MvcWebServiceDailySummary { get; set; }

    public virtual DbSet<MvcWebServiceHourlySummary> MvcWebServiceHourlySummary { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.235;database=DBK;TrustServerCertificate=true; persist security info=True;user id=sa;password=buxodortipinup");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MvcWebServiceDailySummary>(entity =>
        {
            entity.HasKey(e => new { e.Year, e.Month, e.Day }).HasName("PK_WebServiceDailySummary");

            entity.ToTable("mvcWebServiceDailySummary");

            entity.HasIndex(e => new { e.Year, e.Month, e.Day }, "IX_WebServiceDailySummary_YMD");

            entity.Property(e => e.ErrorRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LastUpdatedUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<MvcWebServiceHourlySummary>(entity =>
        {
            entity.HasKey(e => new { e.Year, e.Month, e.Day, e.Hour }).HasName("PK_WebServiceHourlySummary");

            entity.ToTable("mvcWebServiceHourlySummary");

            entity.HasIndex(e => new { e.Year, e.Month, e.Day }, "IX_WebServiceHourlySummary_YMD");

            entity.Property(e => e.ErrorRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LastUpdatedUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Source)
                .HasMaxLength(64)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using DUNES.API.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context700 : DbContext
{
    public context700()
    {
    }

    public context700(DbContextOptions<context700> options)
        : base(options)
    {
    }

    public virtual DbSet<MvcGeneralParameters> MvcGeneralParameters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MvcGeneralParameters>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__mvcGener__3213E83FA1C83C08");

            entity.ToTable("mvcGeneralParameters");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ParameterArea)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("parameterArea");
            entity.Property(e => e.ParameterDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("parameterDescription");
            entity.Property(e => e.ParameterNumber).HasColumnName("parameterNumber");
            entity.Property(e => e.ParameterValue)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("parameterValue");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

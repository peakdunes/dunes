using System;
using System.Collections.Generic;
using DUNES.API.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context200 : DbContext
{
    public context200()
    {
    }

    public context200(DbContextOptions<context200> options)
        : base(options)
    {
    }

    public virtual DbSet<WmsCompanyclient> WmsCompanyclient { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WmsCompanyclient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__wms_comp__3213E83FCFA8AF88");

            entity.ToTable("wms_companyclient");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("company_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

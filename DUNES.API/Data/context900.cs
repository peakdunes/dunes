using System;
using System.Collections.Generic;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context900 : DbContext
{
    public context900()
    {
    }

    public context900(DbContextOptions<context900> options)
        : base(options)
    {
    }

    public virtual DbSet<CompanyClient> CompanyClient { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.233;Database=DBKWMSDUNES;TrustServerCertificate=true;persist security info=True;user id=sa;password=jDqEuXNDOAm6FY7");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyClient>(entity =>
        {
            entity.ToTable("companyClient");

            entity.HasIndex(e => e.Idcity, "IX_companyClient_Idcity");

            entity.HasIndex(e => e.Idcountry, "IX_companyClient_Idcountry");

            entity.HasIndex(e => e.Idstate, "IX_companyClient_Idstate");

            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.CompanyId)
                .HasMaxLength(100)
                .HasColumnName("companyId");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(100)
                .HasColumnName("phone");
            entity.Property(e => e.Website)
                .HasMaxLength(200)
                .HasColumnName("website");
            entity.Property(e => e.Zipcode)
                .HasMaxLength(20)
                .HasColumnName("zipcode");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

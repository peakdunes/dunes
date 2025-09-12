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

    public virtual DbSet<TdivisionCompany> TdivisionCompany { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TdivisionCompany>(entity =>
        {
            entity.HasKey(e => new { e.DivisionDsc, e.CompanyDsc })
                .IsClustered(false)
                .HasFillFactor(90);

            entity.ToTable("_TDivision_Company");

            entity.HasIndex(e => e.CanBeOrdersPartiallyShipped, "IX__TDivision_Company").HasFillFactor(90);

            entity.HasIndex(e => new { e.DivisionDsc, e.CanBeOrdersPartiallyShipped }, "IX__TDivision_Company_1").HasFillFactor(90);

            entity.HasIndex(e => new { e.DivisionDsc, e.CompanyDsc, e.CanBeOrdersPartiallyShipped }, "IX__TDivision_Company_2").HasFillFactor(90);

            entity.HasIndex(e => e.DivisionDsc, "_dta_index__TDivision_Company_8_116247519__K1_9987").HasFillFactor(90);

            entity.HasIndex(e => e.CompanyDsc, "_dta_index__TDivision_Company_8_116247519__K2").HasFillFactor(90);

            entity.HasIndex(e => e.CompanyDsc, "_dta_index__TDivision_Company_c_8_116247519__K2")
                .IsClustered()
                .HasFillFactor(90);

            entity.Property(e => e.DivisionDsc)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("Division_Dsc");
            entity.Property(e => e.CompanyDsc)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Company_Dsc");
            entity.Property(e => e.Active).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

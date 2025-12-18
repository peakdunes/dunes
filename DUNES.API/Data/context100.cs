using System;
using System.Collections.Generic;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context100 : DbContext
{
    public context100()
    {
    }

    public context100(DbContextOptions<context100> options)
        : base(options)
    {
    }

    public virtual DbSet<CompanyClientDivision> CompanyClientDivision { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.233;Database=DBKWMSDUNES;TrustServerCertificate=true;persist security info=True;user id=sa;password=jDqEuXNDOAm6FY7");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompanyClientDivision>(entity =>
        {
            entity.ToTable("companyClientDivision");

            entity.HasIndex(e => e.Idcompanyclient, "IX_companyClientDivision_Idcompanyclient");

            entity.Property(e => e.DivisionName).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

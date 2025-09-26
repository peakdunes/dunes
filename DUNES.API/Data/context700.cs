using System;
using System.Collections.Generic;
using DUNES.API.ModelWMS.Masters;
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

    public virtual DbSet<Warehouseorganization> Warehouseorganization { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.233;Database=DBKWMS;TrustServerCertificate=true;persist security info=True;user id=sa;password=jDqEuXNDOAm6FY7");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Warehouseorganization>(entity =>
        {
            entity.ToTable("warehouseorganization");

            entity.HasIndex(e => e.Idbin, "IX_warehouseorganization_Idbin");

            entity.HasIndex(e => e.Idcompany, "IX_warehouseorganization_Idcompany");

            entity.HasIndex(e => e.Idlocation, "IX_warehouseorganization_Idlocation");

            entity.HasIndex(e => e.Idrack, "IX_warehouseorganization_Idrack");

            entity.Property(e => e.Idcompanyclient).HasMaxLength(200);
            entity.Property(e => e.Iddivision).HasMaxLength(200);
            entity.Property(e => e.Level).HasColumnName("level");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

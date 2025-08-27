using System;
using System.Collections.Generic;
using DUNES.API.ModelsWMS.Transactions;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context400 : DbContext
{
    public context400()
    {
    }

    public context400(DbContextOptions<context400> options)
        : base(options)
    {
    }

    public virtual DbSet<Itemsbybin> Itemsbybin { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.233;Database=DBKWMS;TrustServerCertificate=true;persist security info=True;user id=sa;password=jDqEuXNDOAm6FY7");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Itemsbybin>(entity =>
        {
            entity.ToTable("itemsbybin");

            entity.HasIndex(e => e.BinesId, "IX_itemsbybin_binesId");

            entity.HasIndex(e => e.CompanyId, "IX_itemsbybin_companyId");

            entity.Property(e => e.BinesId).HasColumnName("binesId");
            entity.Property(e => e.CompanyId).HasColumnName("companyId");
            entity.Property(e => e.Idcompanyclient)
                .HasMaxLength(200)
                .HasDefaultValue("");
            entity.Property(e => e.Itemid)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("itemid");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

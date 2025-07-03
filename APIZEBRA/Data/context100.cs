using System;
using System.Collections.Generic;
using APIZEBRA.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context100 : DbContext
{
    public context100()
    {
    }

    public context100(DbContextOptions<context100> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bInventoryType> TzebB2bInventoryType { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bInventoryType>(entity =>
        {
            entity.ToTable("_TZEB_B2B_Inventory_Type");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.PreconInvDest)
                .HasComment("IF NULL: INVENTORY IS NOT PART OF PRE-CONSUMPTION MODEL -- NOT NULL, INV ONLY CAN BE PRECONSUMED INTO THIS PRECON_INV_DEST VALUE")
                .HasColumnName("PRECON_INV_DEST");
            entity.Property(e => e.ShipToLocation)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Usps).HasColumnName("USPS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

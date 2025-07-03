using System;
using System.Collections.Generic;
using APIZEBRA.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context200 : DbContext
{
    public context200()
    {
    }

    public context200(DbContextOptions<context200> options)
        : base(options)
    {
    }

    public virtual DbSet<TrepairActionsCodes> TrepairActionsCodes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TrepairActionsCodes>(entity =>
        {
            entity.HasKey(e => e.ActionId).HasFillFactor(90);

            entity.ToTable("_TRepair_Actions_Codes");

            entity.Property(e => e.ActionId)
                .ValueGeneratedNever()
                .HasColumnName("ActionID");
            entity.Property(e => e.ActionDesc)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using APIZEBRA.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context900 : DbContext
{
    public context900()
    {
    }

    public context900(DbContextOptions<context900> options)
        : base(options)
    {
    }

    public virtual DbSet<MvcPartRunnerMenu> MvcPartRunnerMenu { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MvcPartRunnerMenu>(entity =>
        {
            entity.ToTable("mvcPartRunnerMenu");

            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .HasColumnName("code");
            entity.Property(e => e.Controller)
                .HasMaxLength(100)
                .HasColumnName("controller");
            entity.Property(e => e.Level1)
                .HasMaxLength(100)
                .HasColumnName("level1");
            entity.Property(e => e.Level2)
                .HasMaxLength(100)
                .HasColumnName("level2");
            entity.Property(e => e.Level3)
                .HasMaxLength(100)
                .HasColumnName("level3");
            entity.Property(e => e.Level4)
                .HasMaxLength(100)
                .HasColumnName("level4");
            entity.Property(e => e.Level5)
                .HasMaxLength(100)
                .HasColumnName("level5");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Roles)
                .HasMaxLength(500)
                .HasColumnName("roles");
            entity.Property(e => e.Utility).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

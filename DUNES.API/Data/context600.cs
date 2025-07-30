using System;
using System.Collections.Generic;
using DUNES.API.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context600 : DbContext
{
    public context600()
    {
    }

    public context600(DbContextOptions<context600> options)
        : base(options)
    {
    }

    public virtual DbSet<Ttech> Ttech { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ttech>(entity =>
        {
            entity.HasKey(e => e.TtechNo)
                .IsClustered(false)
                .HasFillFactor(90);

            entity.ToTable("_TTech");

            entity.HasIndex(e => e.TtechName, "IX__TTech");

            entity.HasIndex(e => e.Password, "IX__TTech_1");

            entity.HasIndex(e => e.Login, "IX__TTech_2");

            entity.HasIndex(e => e.IsTech, "IX__TTech_3");

            entity.Property(e => e.TtechNo)
                .ValueGeneratedNever()
                .HasColumnName("TTech_No");
            entity.Property(e => e.Active).HasDefaultValue(true);
            entity.Property(e => e.AdditionalTimeFromCurrDate)
                .HasDefaultValue(0.0)
                .HasColumnName("Additional_time_from _curr_date");
            entity.Property(e => e.Admin).HasDefaultValue(false);
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Engineer).HasDefaultValue(false);
            entity.Property(e => e.IsTech).HasColumnName("Is_Tech");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RepParts).HasComment("Boolean: if Tec repairs parts (radio, cpu, etc) = 1; only Devices = 0 ");
            entity.Property(e => e.Shift)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasComment("D: Day, N:Night");
            entity.Property(e => e.Supervisor).HasDefaultValue(false);
            entity.Property(e => e.TtechName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TTech_Name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

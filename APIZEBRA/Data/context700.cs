using System;
using System.Collections.Generic;
using APIZEBRA.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data;

public partial class context700 : DbContext
{
    public context700()
    {
    }

    public context700(DbContextOptions<context700> options)
        : base(options)
    {
    }

    public virtual DbSet<Tdbkusers> Tdbkusers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=DBK;TrustServerCertificate=true;persist security info=True;user id=radeon;password=ghee8PHED-lism1cich");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tdbkusers>(entity =>
        {
            entity.HasKey(e => e.Login)
                .IsClustered(false)
                .HasFillFactor(90);

            entity.ToTable("TDBKUsers");

            entity.Property(e => e.Login)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");
            entity.Property(e => e.IsCustomerService)
                .HasDefaultValue(false)
                .HasColumnName("Is_Customer_Service");
            entity.Property(e => e.IsTechnicalSupport)
                .HasDefaultValue(false)
                .HasColumnName("Is_Technical_Support");
            entity.Property(e => e.IsZebraPartRunner).HasColumnName("Is_Zebra_PartRunner");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.SupervisorPin)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.WorkingForCompany)
                .HasMaxLength(100)
                .HasDefaultValue("DBK")
                .HasColumnName("Working_for_company");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

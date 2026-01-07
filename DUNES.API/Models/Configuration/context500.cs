using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Models.Configuration;

public partial class context500 : DbContext
{
    public context500()
    {
    }

    public context500(DbContextOptions<context500> options)
        : base(options)
    {
    }

    public virtual DbSet<UserConfiguration> UserConfiguration { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=192.168.1.233;Database=DBKWMSDUNES;TrustServerCertificate=true;persist security info=True;user id=sa;password=jDqEuXNDOAm6FY7");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserConfiguration>(entity =>
        {
            entity.ToTable("userConfiguration");

            entity.HasIndex(e => e.Companyclientdefault, "IX_userConfiguration_companyclientdefault");

            entity.HasIndex(e => e.Companydefault, "IX_userConfiguration_companydefault");

            entity.Property(e => e.AllowChangeSettings).HasColumnName("allowChangeSettings");
            entity.Property(e => e.Bindcr1default).HasColumnName("bindcr1default");
            entity.Property(e => e.Binesdistribution)
                .HasMaxLength(1000)
                .HasColumnName("binesdistribution");
            entity.Property(e => e.Companyclientdefault).HasColumnName("companyclientdefault");
            entity.Property(e => e.Companydefault).HasColumnName("companydefault");
            entity.Property(e => e.Concepttransferdefault).HasColumnName("concepttransferdefault");
            entity.Property(e => e.Datecreated).HasColumnName("datecreated");
            entity.Property(e => e.Deleteonlymytran).HasColumnName("deleteonlymytran");
            entity.Property(e => e.Divisiondefault)
                .HasMaxLength(100)
                .HasColumnName("divisiondefault");
            entity.Property(e => e.Enviromentname)
                .HasMaxLength(100)
                .HasColumnName("enviromentname");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Isdepot).HasColumnName("isdepot");
            entity.Property(e => e.Locationdefault).HasColumnName("locationdefault");
            entity.Property(e => e.Processonlymytran).HasColumnName("processonlymytran");
            entity.Property(e => e.Roleid)
                .HasMaxLength(450)
                .HasColumnName("roleid");
            entity.Property(e => e.Transactiontransferdefault).HasColumnName("transactiontransferdefault");
            entity.Property(e => e.Userid)
                .HasMaxLength(256)
                .HasColumnName("userid");
            entity.Property(e => e.Wmsbin).HasColumnName("wmsbin");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

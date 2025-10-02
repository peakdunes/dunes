using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context800 : DbContext
{
    public context800()
    {
    }

    public context800(DbContextOptions<context800> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bOutBoundResponsesLogFullXmls> TzebB2bOutBoundResponsesLogFullXmls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bOutBoundResponsesLogFullXmls>(entity =>
        {
            entity.ToTable("_TZEB_B2B_OutBound_Responses_Log_fullXMLs");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.DateTimeInserted)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullXml).HasColumnName("fullXML");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

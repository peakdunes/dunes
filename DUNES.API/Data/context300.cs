using System;
using System.Collections.Generic;
using DUNES.API.Models.Inventory.ASN;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data;

public partial class context300 : DbContext
{
    public context300()
    {
    }

    public context300(DbContextOptions<context300> options)
        : base(options)
    {
    }

    public virtual DbSet<TzebB2bAsnLineItemTblItemPartialInbConsReqs> TzebB2bAsnLineItemTblItemPartialInbConsReqs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=10.101.10.241;Database=ZebraUAT;TrustServerCertificate=true;persist security info=True;user id=radeonuat;password=Mrhojt53");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TzebB2bAsnLineItemTblItemPartialInbConsReqs>(entity =>
        {
            entity.ToTable("_TZEB_B2B_ASN_LINE_ITEM_TBL_ITEM_PARTIAL_Inb_Cons_Reqs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AsnLineItemTblItemId).HasColumnName("ASN_LINE_ITEM_TBL_ITEM_ID");
            entity.Property(e => e.CallId).HasColumnName("Call_ID");
            entity.Property(e => e.DateTimeSent)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("DateTime_Sent");
            entity.Property(e => e.QtyPartial).HasColumnName("Qty_Partial");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

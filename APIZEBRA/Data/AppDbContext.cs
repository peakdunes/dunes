using APIZEBRA.Models;
using APIZEBRA.Models.Masters;
using Microsoft.EntityFrameworkCore;

namespace APIZEBRA.Data
{
    public class AppDbContext: DbContext
    {
      

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }


        public DbSet<DbkMvcLogApi> DbkMvcLogApi { get; set; }
        public virtual DbSet<TzebB2bConsignmentCallsType> TzebB2bConsignmentCallsType { get; set; }
        public virtual DbSet<TzebB2bInventoryType> TzebB2bInventoryType { get; set; }
        public virtual DbSet<TrepairActionsCodes> TrepairActionsCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbkMvcLogApi>()
                .ToTable("dbk_mvc_logs_api");

            modelBuilder.Entity<TzebB2bConsignmentCallsType>(entity =>
            {
                entity.ToTable("_TZEB_B2B_Consignment_Calls_Type");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.Attr1).HasMaxLength(100);
                entity.Property(e => e.Attr2).HasMaxLength(100);
                entity.Property(e => e.Code).HasMaxLength(15);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.DocNumPrefix)
                    .HasMaxLength(10)
                    .HasColumnName("Doc_Num_Prefix");
                entity.Property(e => e.ManualReq).HasColumnName("Manual_Req");
                entity.Property(e => e.Name).HasMaxLength(100);
            });

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
        }
    }
}

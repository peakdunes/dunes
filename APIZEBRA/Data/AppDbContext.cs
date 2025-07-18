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
        public virtual DbSet<TzebFaultCodes> TzebFaultCodes { get; set; }
        public virtual DbSet<TzebWorkCodesTargets> TzebWorkCodesTargets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DbkMvcLogApi>()
                .ToTable("dbk_mvc_logs_api");

            modelBuilder.Entity<TzebFaultCodes>(entity =>
            {
                entity.HasKey(e => e.FaultCode);

                entity.ToTable("_TZEB_FAULT_CODES");

                entity.Property(e => e.FaultCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code");
                entity.Property(e => e.Categorization)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.DateInserted)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.FaultCodeDefinition)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code_Definition");
                entity.Property(e => e.FaultCodeGroup)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Code_Group");
                entity.Property(e => e.FaultDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Fault_Desc");
                entity.Property(e => e.ProductGroup)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Product_Group");
                entity.Property(e => e.Show).HasDefaultValue(true);
            });

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

            modelBuilder.Entity<TzebWorkCodesTargets>(entity =>
            {
                entity.HasKey(e => e.WorkCodeTarget);

                entity.ToTable("_TZEB_WORK_CODES_TARGETS");

                entity.Property(e => e.WorkCodeTarget)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Work_Code_Target");
                entity.Property(e => e.ConsideredForBer)
                    .HasDefaultValue(false)
                    .HasColumnName("Considered_For_BER");
                entity.Property(e => e.Show).HasDefaultValue(true);
                entity.Property(e => e.WorkDescTarget)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("Work_Desc_Target");
            });
        }
    }
}

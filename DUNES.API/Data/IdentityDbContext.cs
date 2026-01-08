using DUNES.API.Models.Configuration;
using DUNES.API.ModelsWMS.Admin;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data
{
    /// <summary>
    /// Identity DB Context
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// userConfiguration table
        /// </summary>
        public DbSet<UserConfiguration> UserConfiguration { get; set; } = default!;

        /// <summary>
        /// Menu access options
        /// </summary>
        public virtual DbSet<Menu> Menu { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("code");

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

                entity.Property(e => e.Roles)
                    .HasMaxLength(500)
                    .HasColumnName("roles");

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.Property(e => e.Utility)
                    .HasMaxLength(500)
                    .HasColumnName("Utility"); // OJO: en tu diseño aparece con U mayúscula

                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .HasColumnName("action");

                entity.Property(e => e.Controller)
                    .HasMaxLength(100)
                    .HasColumnName("controller");

                entity.Property(e => e.Order)
                    .HasColumnName("order"); // reserved word ok via mapping

                entity.Property(e => e.Title)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("title");
            });


            modelBuilder.Entity<UserConfiguration>(entity =>
            {
                entity.ToTable("userConfiguration");

                // PK
                entity.HasKey(e => e.Id);

                // Existing indexes from scaffold (keep them)
                entity.HasIndex(e => e.Companyclientdefault, "IX_userConfiguration_companyclientdefault");
                entity.HasIndex(e => e.Companydefault, "IX_userConfiguration_companydefault");

                // Column mappings (keep your DB column names)
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

                // IMPORTANT: must match AspNetUsers.Id (nvarchar(450))
                entity.Property(e => e.Userid)
                    .HasMaxLength(450)
                    .HasColumnName("userid");

                entity.Property(e => e.Wmsbin).HasColumnName("wmsbin");

                // ----------------------------
                // UNIQUE RULES
                // ----------------------------

                // Only one active config per user
                entity.HasIndex(e => e.Userid)
                      .IsUnique()
                      .HasFilter("[isactive] = 1")
                      .HasDatabaseName("UX_userConfiguration_OneActivePerUser");

                // (Recommended) EnvironmentName unique per user
                entity.HasIndex(e => new { e.Userid, e.Enviromentname })
                      .IsUnique()
                      .HasDatabaseName("UX_userConfiguration_User_EnvName");

                // ----------------------------
                // FOREIGN KEYS (Identity only here)
                // ----------------------------

                // FK -> AspNetUsers
                entity.HasOne<IdentityUser>()
                      .WithMany()
                      .HasForeignKey(e => e.Userid)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_userConfiguration_AspNetUsers_userid");

                // FK -> AspNetRoles
                entity.HasOne<IdentityRole>()
                      .WithMany()
                      .HasForeignKey(e => e.Roleid)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_userConfiguration_AspNetRoles_roleid");

                // NOTE:
                // FKs to Company and companyClient will be created in the migration
                // using migrationBuilder.AddForeignKey(principalTable: "Company"/"companyClient")
                // because those entities are not part of this DbContext.
            });
        }
    }
}

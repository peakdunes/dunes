using DUNES.API.ModelsWMS.Auth;
using DUNES.API.ModelsWMS.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Data
{
    /// <summary>
    /// Identity DB Context
    /// </summary>
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// DI
        /// </summary>
        /// <param name="options"></param>
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
        public virtual DbSet<Menu> Menu { get; set; } = null!;

        /// <summary>
        /// Permissions catalog (stable keys like: WMS.LOCATIONS.ACCESS, ADMIN.USERS.EDIT).
        /// </summary>
        public virtual DbSet<AuthPermission> AuthPermissions { get; set; } = null!;

        /// <summary>
        /// Role → Permission mapping (many-to-many).
        /// Defines the base permissions granted by each Identity role.
        /// </summary>
        public virtual DbSet<AuthRolePermission> AuthRolePermissions { get; set; } = null!;

        /// <summary>
        /// User → Permission mapping (grants only for now).
        /// Used for user-specific exceptions without creating new roles.
        /// </summary>
        public virtual DbSet<AuthUserPermission> AuthUserPermissions { get; set; } = null!;

        /// <summary>
        /// Menu → Permission mapping (usually ACCESS permissions).
        /// Controls which menu options are visible/available based on permissions.
        /// </summary>
        public virtual DbSet<MenuPermission> MenuPermissions { get; set; } = null!;

        /// <summary>
        /// Fluent API mappings for this DbContext.
        /// Here we configure table names, column types/lengths, keys (including composite keys),
        /// indexes/unique constraints, and foreign-key relationships for Identity + Auth + Menu
        /// entities without relying only on data annotations.
        /// </summary>
        /// <param name="modelBuilder">EF Core model builder used to configure the entity model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // AspNetUsers extra fields
            // =========================
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("AspNetUsers");

                entity.Property(x => x.FullName)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true)
                    .IsRequired();

                entity.Property(x => x.MustChangePassword)
                    .HasDefaultValue(true)
                    .IsRequired();

                entity.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("sysutcdatetime()")
                    .IsRequired();

                entity.Property(x => x.CreatedBy)
                    .HasMaxLength(450);

                entity.Property(x => x.UpdatedAt)
                    .HasColumnType("datetime2");

                entity.Property(x => x.UpdatedBy)
                    .HasMaxLength(450);
            });

            // =========================
            // AuthPermissions
            // =========================
            modelBuilder.Entity<AuthPermission>(entity =>
            {
                entity.ToTable("AuthPermissions");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.PermissionKey)
                    .HasMaxLength(150)
                    .IsRequired();

                entity.HasIndex(x => x.PermissionKey)
                    .IsUnique()
                    .HasDatabaseName("UX_AuthPermissions_PermissionKey");

                entity.Property(x => x.Description)
                    .HasMaxLength(300);

                entity.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                entity.Property(x => x.CreatedAt)
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("sysutcdatetime()");
            });

            // =========================
            // AuthRolePermissions (RoleId, PermissionId)
            // =========================
            modelBuilder.Entity<AuthRolePermission>(entity =>
            {
                entity.ToTable("AuthRolePermissions");

                entity.HasKey(x => new { x.RoleId, x.PermissionId });

                entity.Property(x => x.RoleId)
                    .HasMaxLength(450)
                    .IsRequired();

                entity.Property(x => x.PermissionId)
                    .IsRequired();

                // FK -> AspNetRoles
                entity.HasOne<IdentityRole>()
                    .WithMany()
                    .HasForeignKey(x => x.RoleId)
                    .HasConstraintName("FK_AuthRolePermissions_AspNetRoles_RoleId")
                    .OnDelete(DeleteBehavior.Cascade);

                // FK -> AuthPermissions
                entity.HasOne<AuthPermission>()
                    .WithMany()
                    .HasForeignKey(x => x.PermissionId)
                    .HasConstraintName("FK_AuthRolePermissions_AuthPermissions_PermissionId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => x.PermissionId)
                    .HasDatabaseName("IX_AuthRolePermissions_PermissionId");
            });

            // =========================
            // AuthUserPermissions (UserId, PermissionId)
            // =========================
            modelBuilder.Entity<AuthUserPermission>(entity =>
            {
                entity.ToTable("AuthUserPermissions");

                entity.HasKey(x => new { x.UserId, x.PermissionId });

                entity.Property(x => x.UserId)
                    .HasMaxLength(450)
                    .IsRequired();

                entity.Property(x => x.PermissionId)
                    .IsRequired();

                // FK -> AspNetUsers
                entity.HasOne<ApplicationUser>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .HasConstraintName("FK_AuthUserPermissions_AspNetUsers_UserId")
                    .OnDelete(DeleteBehavior.Cascade);

                // FK -> AuthPermissions
                entity.HasOne<AuthPermission>()
                    .WithMany()
                    .HasForeignKey(x => x.PermissionId)
                    .HasConstraintName("FK_AuthUserPermissions_AuthPermissions_PermissionId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => x.PermissionId)
                    .HasDatabaseName("IX_AuthUserPermissions_PermissionId");
            });

            // =========================
            // MenuPermissions (MenuId, PermissionId)
            // =========================
            modelBuilder.Entity<MenuPermission>(entity =>
            {
                entity.ToTable("MenuPermissions");

                entity.HasKey(x => new { x.MenuId, x.PermissionId });

                entity.Property(x => x.MenuId).IsRequired();
                entity.Property(x => x.PermissionId).IsRequired();

                // FK -> Menu
                entity.HasOne<Menu>()
                    .WithMany()
                    .HasForeignKey(x => x.MenuId)
                    .HasConstraintName("FK_MenuPermissions_Menu_MenuId")
                    .OnDelete(DeleteBehavior.Cascade);

                // FK -> AuthPermissions
                entity.HasOne<AuthPermission>()
                    .WithMany()
                    .HasForeignKey(x => x.PermissionId)
                    .HasConstraintName("FK_MenuPermissions_AuthPermissions_PermissionId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(x => x.PermissionId)
                    .HasDatabaseName("IX_MenuPermissions_PermissionId");
            });

            // =========================
            // Menu
            // =========================
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

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.Property(e => e.Utility)
                    .HasMaxLength(500)
                    .HasColumnName("Utility");

                entity.Property(e => e.Action)
                    .HasMaxLength(100)
                    .HasColumnName("action");

                entity.Property(e => e.Controller)
                    .HasMaxLength(100)
                    .HasColumnName("controller");

                entity.Property(e => e.Order)
                    .HasColumnName("order");

                entity.Property(e => e.Title)
                    .HasColumnType("varchar(200)")
                    .HasColumnName("title");
            });

            // =========================
            // UserConfiguration
            // =========================
            modelBuilder.Entity<UserConfiguration>(entity =>
            {
                entity.ToTable("userConfiguration");

                entity.HasKey(e => e.Id);

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
                    .HasMaxLength(450)
                    .HasColumnName("userid");

                entity.Property(e => e.Wmsbin).HasColumnName("wmsbin");

                // Only one active config per user
                entity.HasIndex(e => e.Userid)
                    .IsUnique()
                    .HasFilter("[isactive] = 1")
                    .HasDatabaseName("UX_userConfiguration_OneActivePerUser");

                // Recommended: EnvironmentName unique per user
                entity.HasIndex(e => new { e.Userid, e.Enviromentname })
                    .IsUnique()
                    .HasDatabaseName("UX_userConfiguration_User_EnvName");

                // FK -> AspNetUsers
                entity.HasOne<ApplicationUser>()
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
            });
        }
    }
}
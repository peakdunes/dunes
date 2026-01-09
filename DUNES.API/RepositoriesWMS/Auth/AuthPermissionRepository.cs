using DUNES.API.Data;
using DUNES.API.RepositoriesWMS.Auth;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Auth

{
    /// <summary>
    /// Auth permissions repository implementation.
    /// </summary>
    public class AuthPermissionRepository : IAuthPermissionRepository
    {
        private readonly IdentityDbContext _db;

        /// <summary>
        /// Constructor (DI).
        /// </summary>
        public AuthPermissionRepository(IdentityDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns effective permission keys for user:
        /// - Role permissions (AspNetUserRoles -> AuthRolePermissions -> AuthPermissions)
        /// - User grants (AuthUserPermissions -> AuthPermissions)
        /// </summary>
        public async Task<List<string>> GetEffectivePermissionKeysAsync(string userId, CancellationToken ct)
        {
            // Role-based permissions
            var rolePermsQuery =
                from ur in _db.UserRoles
                join rp in _db.AuthRolePermissions on ur.RoleId equals rp.RoleId
                join p in _db.AuthPermissions on rp.PermissionId equals p.Id
                where ur.UserId == userId && p.IsActive
                select p.PermissionKey;

            // User grants (only grants for now)
            var userPermsQuery =
                from up in _db.AuthUserPermissions
                join p in _db.AuthPermissions on up.PermissionId equals p.Id
                where up.UserId == userId && p.IsActive
                select p.PermissionKey;

            // Union + Distinct
            var keys = await rolePermsQuery
                .Union(userPermsQuery)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync(ct);

            return keys;
        }
    }
}

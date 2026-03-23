using DUNES.API.Data;
using DUNES.API.ModelsWMS.Auth;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Repository implementation for role-permission assignments.
    /// </summary>
    public class AuthRolePermissionRepository : IAuthRolePermissionRepository
    {
        private readonly IdentityDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRolePermissionRepository"/> class.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public AuthRolePermissionRepository(IdentityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all permission ids currently assigned to the given role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission ids.</returns>
        public async Task<List<int>> GetPermissionIdsByRoleAsync(string roleId, CancellationToken ct)
        {
            return await _context.AuthRolePermissions
                .Where(x => x.RoleId == roleId)
                .Select(x => x.PermissionId)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Removes all permission assignments for the given role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        public async Task RemoveAllByRoleAsync(string roleId, CancellationToken ct)
        {
            var entities = await _context.AuthRolePermissions
                .Where(x => x.RoleId == roleId)
                .ToListAsync(ct);

            _context.AuthRolePermissions.RemoveRange(entities);
            await _context.SaveChangesAsync(ct);
        }

        /// <summary>
        /// Adds role-permission assignments in bulk.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        /// <param name="ct">Cancellation token.</param>
        public async Task AddRangeAsync(List<AuthRolePermission> entities, CancellationToken ct)
        {
            if (entities.Count == 0)
                return;

            _context.AuthRolePermissions.AddRange(entities);
            await _context.SaveChangesAsync(ct);
        }
    }
}

using DUNES.API.Data;
using DUNES.API.ModelsWMS.Auth;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Repository for direct user-permission assignments.
    /// </summary>
    public class AuthUserPermissionRepository : IAuthUserPermissionRepository
    {
        private readonly IdentityDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthUserPermissionRepository"/> class.
        /// </summary>
        /// <param name="context">Identity database context.</param>
        public AuthUserPermissionRepository(IdentityDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<int>> GetPermissionIdsByUserAsync(string userId, CancellationToken ct)
        {
            return await _context.AuthUserPermissions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.PermissionId)
                .ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task DeleteByUserIdAsync(string userId, CancellationToken ct)
        {
            var entities = await _context.AuthUserPermissions
                .Where(x => x.UserId == userId)
                .ToListAsync(ct);

            if (!entities.Any())
                return;

            _context.AuthUserPermissions.RemoveRange(entities);
            await _context.SaveChangesAsync(ct);
        }

        /// <inheritdoc />
        public async Task AddRangeAsync(List<AuthUserPermission> entities, CancellationToken ct)
        {
            if (entities.Count == 0)
                return;

            _context.AuthUserPermissions.AddRange(entities);
            await _context.SaveChangesAsync(ct);
        }

     

        /// <inheritdoc />
        public async Task<List<int>> GetValidIdsAsync(IEnumerable<int> ids, CancellationToken ct)
        {
            var idList = ids?
                .Distinct()
                .ToList() ?? new List<int>();

            if (idList.Count == 0)
                return new List<int>();

            return await _context.AuthPermissions
                .AsNoTracking()
                .Where(x => idList.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(ct);
        }
    }
}

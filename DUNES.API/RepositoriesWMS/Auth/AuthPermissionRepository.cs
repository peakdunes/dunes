using DUNES.API.Data;
using DUNES.API.ModelsWMS.Auth;
using DUNES.API.RepositoriesWMS.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace DUNES.API.RepositoriesWMS.Auth

{
    /// <summary>
    /// Repository implementation for permission catalog operations.
    /// </summary>
    public class AuthPermissionRepository : IAuthPermissionRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPermissionRepository"/> class.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public AuthPermissionRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all permissions ordered by permission key.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions.</returns>
        public async Task<List<AuthPermission>> GetAllAsync(CancellationToken ct)
        {
            return await _context.AuthPermissions
                .OrderBy(x => x.PermissionKey)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a permission by its database identifier.
        /// </summary>
        /// <param name="id">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The permission if found; otherwise null.</returns>
        public async Task<AuthPermission?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.AuthPermissions
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        /// <summary>
        /// Retrieves a permission by its unique permission key.
        /// </summary>
        /// <param name="permissionKey">Permission key.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The permission if found; otherwise null.</returns>
        public async Task<AuthPermission?> GetByKeyAsync(string permissionKey, CancellationToken ct)
        {
            return await _context.AuthPermissions
                .FirstOrDefaultAsync(x => x.PermissionKey == permissionKey, ct);
        }

        /// <summary>
        /// Creates a new permission record.
        /// </summary>
        /// <param name="entity">Permission entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission entity.</returns>
        public async Task<AuthPermission> CreateAsync(AuthPermission entity, CancellationToken ct)
        {
            _context.AuthPermissions.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

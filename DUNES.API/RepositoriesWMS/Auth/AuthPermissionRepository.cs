using DUNES.API.Data;
using DUNES.API.ModelsWMS.Auth;
using Microsoft.EntityFrameworkCore;

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
                .AsNoTracking()
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
                .AsNoTracking()
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
                .AsNoTracking()
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

        /// <inheritdoc />
        public async Task<List<AuthPermission>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct)
        {
            var idList = ids?
                .Distinct()
                .ToList() ?? new List<int>();

            if (idList.Count == 0)
                return new List<AuthPermission>();

            return await _context.AuthPermissions
                .AsNoTracking()
                .Where(x => idList.Contains(x.Id))
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves all permissions that belong to a specific functional group and module.
        /// This method returns the complete catalog for the requested module.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions for the requested module.</returns>
        public async Task<List<AuthPermission>> GetByModuleAsync(string groupName, string moduleName, CancellationToken ct)
        {
            return await _context.AuthPermissions
                .AsNoTracking()
                .Where(x => x.GroupName == groupName && x.ModuleName == moduleName)
                .OrderBy(x => x.DisplayOrder)
                .ThenBy(x => x.PermissionKey)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as row-level actions in index tables.
        /// Example: Edit, Delete, ResetPassword, Deactivate.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of row-action permissions for the requested module.</returns>
        public async Task<List<AuthPermission>> GetRowActionsByModuleAsync(string groupName, string moduleName, CancellationToken ct)
        {
            return await _context.AuthPermissions
                .AsNoTracking()
                .Where(x =>
                    x.GroupName == groupName &&
                    x.ModuleName == moduleName &&
                    x.IsActive &&
                    x.ShowAsRowAction)
                .OrderBy(x => x.ButtonOrder)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.PermissionKey)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as toolbar or header actions in index views.
        /// Example: Create, Export, Process.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of toolbar-action permissions for the requested module.</returns>
        public async Task<List<AuthPermission>> GetToolbarActionsByModuleAsync(string groupName, string moduleName, CancellationToken ct)
        {
            return await _context.AuthPermissions
                .AsNoTracking()
                .Where(x =>
                    x.GroupName == groupName &&
                    x.ModuleName == moduleName &&
                    x.IsActive &&
                    x.ShowAsToolbarAction)
                .OrderBy(x => x.ButtonOrder)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.PermissionKey)
                .ToListAsync(ct);
        }
    }
}
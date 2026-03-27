using DUNES.API.ModelsWMS.Auth;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Repository contract for managing permission catalog records.
    /// </summary>
    public interface IAuthPermissionRepository
    {
        /// <summary>
        /// Retrieves all permissions ordered by permission key.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions.</returns>
        Task<List<AuthPermission>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Retrieves a permission by its database identifier.
        /// </summary>
        /// <param name="id">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The permission if found; otherwise null.</returns>
        Task<AuthPermission?> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Retrieves a permission by its unique permission key.
        /// </summary>
        /// <param name="permissionKey">Permission key.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The permission if found; otherwise null.</returns>
        Task<AuthPermission?> GetByKeyAsync(string permissionKey, CancellationToken ct);

        /// <summary>
        /// Creates a new permission record.
        /// </summary>
        /// <param name="entity">Permission entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission entity.</returns>
        Task<AuthPermission> CreateAsync(AuthPermission entity, CancellationToken ct);

        /// <summary>
        /// Gets permissions by a collection of identifiers.
        /// </summary>
        /// <param name="ids">Permission identifiers.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Matching permissions.</returns>
        Task<List<AuthPermission>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct);

        /// <summary>
        /// Retrieves all permissions that belong to a specific functional group and module.
        /// This method returns the complete catalog for the requested module.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions for the requested module.</returns>
        Task<List<AuthPermission>> GetByModuleAsync(string groupName, string moduleName, CancellationToken ct);

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as row-level actions in index tables.
        /// Example: Edit, Delete, ResetPassword, Deactivate.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of row-action permissions for the requested module.</returns>
        Task<List<AuthPermission>> GetRowActionsByModuleAsync(string groupName, string moduleName, CancellationToken ct);

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as toolbar or header actions in index views.
        /// Example: Create, Export, Process.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of toolbar-action permissions for the requested module.</returns>
        Task<List<AuthPermission>> GetToolbarActionsByModuleAsync(string groupName, string moduleName, CancellationToken ct);
    }
}
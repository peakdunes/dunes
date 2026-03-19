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
    }
}

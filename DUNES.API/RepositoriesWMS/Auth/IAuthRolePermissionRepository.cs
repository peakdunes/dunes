using DUNES.API.ModelsWMS.Auth;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Repository contract for role-permission assignments.
    /// </summary>
    public interface IAuthRolePermissionRepository
    {
        /// <summary>
        /// Retrieves all permission ids currently assigned to the given role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission ids.</returns>
        Task<List<int>> GetPermissionIdsByRoleAsync(string roleId, CancellationToken ct);

        /// <summary>
        /// Removes all permission assignments for the given role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        Task RemoveAllByRoleAsync(string roleId, CancellationToken ct);

        /// <summary>
        /// Adds role-permission assignments in bulk.
        /// </summary>
        /// <param name="entities">Entities to add.</param>
        /// <param name="ct">Cancellation token.</param>
        Task AddRangeAsync(List<AuthRolePermission> entities, CancellationToken ct);
    }
}

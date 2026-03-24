using DUNES.API.ModelsWMS.Auth;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Repository contract for direct user-permission assignments.
    /// </summary>
    public interface IAuthUserPermissionRepository
    {
        /// <summary>
        /// Gets all direct permission ids assigned to the specified user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission ids.</returns>
        Task<List<int>> GetPermissionIdsByUserAsync(string userId, CancellationToken ct);

        /// <summary>
        /// Deletes all direct permission assignments for the specified user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Task.</returns>
        Task DeleteByUserIdAsync(string userId, CancellationToken ct);

        /// <summary>
        /// Adds multiple direct user-permission assignments.
        /// </summary>
        /// <param name="entities">Entities to insert.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Task.</returns>
        Task AddRangeAsync(List<AuthUserPermission> entities, CancellationToken ct);
    }
}

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Auth.DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service contract for direct user-permission management.
    /// </summary>
    public interface IAuthUserPermissionService
    {
        /// <summary>
        /// Gets inherited, direct, and effective permissions for a user.
        /// </summary>
        /// <param name="userId">User identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>User permission bundle.</returns>
        Task<ApiResponse<UserPermissionBundleDTO>> GetByUserAsync(string userId, CancellationToken ct);

        /// <summary>
        /// Saves direct permissions for a user.
        /// </summary>
        /// <param name="request">Save request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> SaveByUserAsync(SaveUserPermissionsDTO request, CancellationToken ct);
    }

}

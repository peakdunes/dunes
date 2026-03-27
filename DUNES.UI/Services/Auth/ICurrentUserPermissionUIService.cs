using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Provides UI operations related to the authenticated user's effective permissions.
    /// </summary>
    public interface ICurrentUserPermissionUIService
    {
        /// <summary>
        /// Gets the effective permissions of the authenticated user.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Authenticated user effective permissions.</returns>
        Task<ApiResponse<CurrentUserPermissionsDTO>> GetMyPermissionsAsync(string token, CancellationToken ct);
    }
}

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    public interface IAuthPermissionUIService
    {
        /// <summary>
        /// Retrieves all permissions from the catalog.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetAllAsync(string token, CancellationToken ct);

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="dto">Permission creation DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission record.</returns>
        Task<ApiResponse<AuthPermissionReadDTO>> CreateAsync(string token, AuthPermissionCreateDTO dto, CancellationToken ct);

    }
}

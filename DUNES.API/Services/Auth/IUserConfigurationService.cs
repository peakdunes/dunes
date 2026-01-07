using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// User configuration service interface
    /// </summary>
    public interface IUserConfigurationService
    {
        /// <summary>
        /// Get active user configuration (Read DTO)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<UserConfigurationReadDto?>> GetActiveAsync(string userId, CancellationToken ct);

        /// <summary>
        /// Get configurations by user (Read DTO list)
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<List<UserConfigurationReadDto>>> GetByUserAsync(string userId, CancellationToken ct);

        /// <summary>
        /// Get configuration by id (Read DTO)
        /// (Validates ownership by userId)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<UserConfigurationReadDto?>> GetByIdAsync(int id, string userId, CancellationToken ct);

        /// <summary>
        /// Create new user configuration
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId">
        /// The logged-in user id (recommended to set ownership server-side).
        /// If you want admin to create for another user, extend signature later.
        /// </param>
        /// <param name="ct"></param>
        Task<ApiResponse<UserConfigurationReadDto>> CreateAsync(UserConfigurationCreateDto dto, string userId, CancellationToken ct);

        /// <summary>
        /// Update user configuration
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<bool>> UpdateAsync(UserConfigurationUpdateDto dto, string userId, CancellationToken ct);

        /// <summary>
        /// Delete user configuration
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<bool>> DeleteAsync(int id, string userId, CancellationToken ct);

        /// <summary>
        /// Activate a user configuration (deactivates any other active config for that user)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<bool>> SetActiveAsync(int id, string userId, CancellationToken ct);

        /// <summary>
        /// Exists environment name for user (optional endpoint to support UI validations)
        /// </summary>
        /// <param name="envName"></param>
        /// <param name="excludeId"></param>
        /// <param name="userId"></param>
        /// <param name="ct"></param>
        Task<ApiResponse<bool>> ExistsEnvNameAsync(string envName, int? excludeId, string userId, CancellationToken ct);
    }
}

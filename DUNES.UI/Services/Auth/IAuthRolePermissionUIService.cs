using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Service contract for role-permission assignment operations from the MVC UI.
    /// </summary>
    public interface IAuthRolePermissionUIService
    {
        /// <summary>
        /// Retrieves all permissions and indicates which are assigned to the specified role.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of role permission items.</returns>
        Task<ApiResponse<List<RolePermissionItemDTO>>> GetByRoleAsync(string token, string roleId, CancellationToken ct);

        /// <summary>
        /// Saves the full permission set for the specified role.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="dto">Save request DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> SaveByRoleAsync(string token, SaveRolePermissionsDTO dto, CancellationToken ct);

        /// <summary>
        /// Retrieves available roles for dropdown selection.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of roles.</returns>
        Task<ApiResponse<List<RoleOptionDTO>>> GetRolesAsync(string token, CancellationToken ct);
    }
}
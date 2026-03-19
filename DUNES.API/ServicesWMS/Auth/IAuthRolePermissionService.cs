using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service contract for managing role-permission assignments.
    /// </summary>
    public interface IAuthRolePermissionService
    {
        /// <summary>
        /// Retrieves all permissions and marks which ones are assigned to the given role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission items for the selected role.</returns>
        Task<ApiResponse<List<RolePermissionItemDTO>>> GetByRoleAsync(string roleId, CancellationToken ct);

        /// <summary>
        /// Replaces the full permission set for a role.
        /// </summary>
        /// <param name="dto">Save request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        Task<ApiResponse<bool>> SaveByRoleAsync(SaveRolePermissionsDTO dto, CancellationToken ct);
    }
}

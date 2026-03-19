using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Service implementation for role-permission assignment operations from the MVC UI.
    /// </summary>
    public class AuthRolePermissionUIService : UIApiServiceBase, IAuthRolePermissionUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRolePermissionUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public AuthRolePermissionUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Retrieves all permissions and indicates which are assigned to the specified role.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of role permission items.</returns>
        public async Task<ApiResponse<List<RolePermissionItemDTO>>> GetByRoleAsync(string token, string roleId, CancellationToken ct)
        {
            return await GetApiAsync<List<RolePermissionItemDTO>>(
                $"/api/AuthRolePermission/GetByRole/{roleId}",
                token,
                ct
            );
        }

        /// <summary>
        /// Saves the full permission set for the specified role.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="dto">Save request DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> SaveByRoleAsync(string token, SaveRolePermissionsDTO dto, CancellationToken ct)
        {
            return await PutApiAsync<bool, SaveRolePermissionsDTO>(
                "/api/AuthRolePermission/SaveByRole",
                dto,
                token,
                ct
            );
        }

        /// <summary>
        /// Retrieves available roles for dropdown selection.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of roles.</returns>
        public async Task<ApiResponse<List<RoleOptionDTO>>> GetRolesAsync(string token, CancellationToken ct)
        {
            return await GetApiAsync<List<RoleOptionDTO>>(
                "/api/User/GetRoles",
                token,
                ct
            );
        }
    }
}
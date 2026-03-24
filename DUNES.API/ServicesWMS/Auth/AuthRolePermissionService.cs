using DUNES.API.Models.Auth;
using DUNES.API.ModelsWMS.Auth;
using DUNES.API.RepositoriesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service implementation for managing role-permission assignments.
    /// </summary>
    public class AuthRolePermissionService : IAuthRolePermissionService
    {
        private readonly IAuthRolePermissionRepository _rolePermissionRepository;
        private readonly IAuthPermissionRepository _permissionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRolePermissionService"/> class.
        /// </summary>
        /// <param name="rolePermissionRepository">Role-permission repository.</param>
        /// <param name="permissionRepository">Permission catalog repository.</param>
        public AuthRolePermissionService(
            IAuthRolePermissionRepository rolePermissionRepository,
            IAuthPermissionRepository permissionRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
        }

        /// <summary>
        /// Retrieves all permissions and marks which ones are assigned to the given role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission items for the selected role.</returns>
        public async Task<ApiResponse<List<RolePermissionItemDTO>>> GetByRoleAsync(string roleId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return ApiResponseFactory.Fail<List<RolePermissionItemDTO>>(
                    error: "INVALID_ROLE",
                    message: "Role id is required.",
                    statusCode: 400);
            }

            var allPermissions = await _permissionRepository.GetAllAsync(ct);
            var assignedIds = await _rolePermissionRepository.GetPermissionIdsByRoleAsync(roleId, ct);

            var data = allPermissions
                .Select(p => new RolePermissionItemDTO
                {
                    PermissionId = p.Id,
                    PermissionKey = p.PermissionKey,
                    Group = p.GroupName,
                    Resource = p.ModuleName,
                    Action = p.ActionName,
                    Description = p.Description,
                    IsActive = p.IsActive,
                    Assigned = assignedIds.Contains(p.Id),
                    DisplayOrder = p.DisplayOrder
                    
                })
               .OrderBy(x => x.Group)
                .ThenBy(x => x.Resource)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Action)
                .ToList();

            return ApiResponseFactory.Success(data, "Role permissions loaded successfully.");
        }

        /// <summary>
        /// Replaces the full permission set for a role.
        /// </summary>
        /// <param name="dto">Save request.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> SaveByRoleAsync(SaveRolePermissionsDTO dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.RoleId))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "INVALID_ROLE",
                    message: "Role id is required.",
                    statusCode: 400);
            }

            await _rolePermissionRepository.RemoveAllByRoleAsync(dto.RoleId, ct);

            var entities = dto.PermissionIds
                .Distinct()
                .Select(id => new AuthRolePermission
                {
                    RoleId = dto.RoleId,
                    PermissionId = id
                })
                .ToList();

            await _rolePermissionRepository.AddRangeAsync(entities, ct);

            return ApiResponseFactory.Success(true, "Role permissions saved successfully.");
        }
    }
}
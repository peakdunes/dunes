using DUNES.API.Data;
using DUNES.API.ModelsWMS.Auth;
using DUNES.API.RepositoriesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Auth.DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service for direct user-permission management.
    /// </summary>
    public class AuthUserPermissionService : IAuthUserPermissionService
    {
        private readonly IAuthPermissionRepository _permissionRepository;
        private readonly IAuthRolePermissionRepository _rolePermissionRepository;
        private readonly IAuthUserPermissionRepository _userPermissionRepository;
        private readonly IdentityDbContext _identityContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthUserPermissionService"/> class.
        /// </summary>
        /// <param name="permissionRepository">Permission repository.</param>
        /// <param name="rolePermissionRepository">Role-permission repository.</param>
        /// <param name="userPermissionRepository">User-permission repository.</param>
        /// <param name="identityContext">Identity context.</param>
        public AuthUserPermissionService(
            IAuthPermissionRepository permissionRepository,
            IAuthRolePermissionRepository rolePermissionRepository,
            IAuthUserPermissionRepository userPermissionRepository,
            IdentityDbContext identityContext)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userPermissionRepository = userPermissionRepository;
            _identityContext = identityContext;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<UserPermissionBundleDTO>> GetByUserAsync(string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return ApiResponseFactory.Fail<UserPermissionBundleDTO>(
                    error: "INVALID_USER",
                    message: "User id is required.",
                    statusCode: 400);
            }

            var userExists = await _identityContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId, ct);

            if (!userExists)
            {
                return ApiResponseFactory.Fail<UserPermissionBundleDTO>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: 404);
            }

            var roleIds = await _identityContext.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync(ct);

            if (roleIds.Count > 1)
            {
                return ApiResponseFactory.Fail<UserPermissionBundleDTO>(
                    error: "MULTIPLE_ROLES_NOT_ALLOWED",
                    message: "The user has more than one role assigned. Only one role is allowed.",
                    statusCode: 400);
            }

            var roleId = roleIds.FirstOrDefault();

            var allPermissions = await _permissionRepository.GetAllAsync(ct);

            var directPermissionIds = await _userPermissionRepository
                .GetPermissionIdsByUserAsync(userId, ct);

            var inheritedPermissionIds = string.IsNullOrWhiteSpace(roleId)
                ? new HashSet<int>()
                : (await _rolePermissionRepository.GetPermissionIdsByRoleAsync(roleId, ct)).ToHashSet();

            var directPermissionSet = directPermissionIds.ToHashSet();

            var effectivePermissionIds = inheritedPermissionIds
                .Union(directPermissionSet)
                .ToHashSet();

            var inheritedPermissions = allPermissions
                .Where(p => p.IsActive)
                .Where(p => inheritedPermissionIds.Contains(p.Id))
                .Select(MapPermissionToDto)
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Resource)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Action)
                .ToList();

            var directPermissions = allPermissions
                .Where(p => p.IsActive)
                .Where(p => directPermissionSet.Contains(p.Id))
                .Select(MapPermissionToDto)
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Resource)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Action)
                .ToList();

            var effectivePermissions = allPermissions
                .Where(p => p.IsActive)
                .Where(p => effectivePermissionIds.Contains(p.Id))
                .Select(MapPermissionToDto)
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Resource)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Action)
                .ToList();

            var availableDirectPermissions = allPermissions
                .Where(p => p.IsActive)
                .Where(p => !inheritedPermissionIds.Contains(p.Id))
                .Select(MapPermissionToDto)
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Resource)
                .ThenBy(x => x.DisplayOrder)
                .ThenBy(x => x.Action)
                .ToList();

            var response = new UserPermissionBundleDTO
            {
                UserId = userId,
                InheritedPermissions = inheritedPermissions,
                DirectPermissions = directPermissions,
                EffectivePermissions = effectivePermissions,
                AvailableDirectPermissions = availableDirectPermissions
            };

            return ApiResponseFactory.Success(response, "User permissions loaded successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> SaveByUserAsync(SaveUserPermissionsDTO request, CancellationToken ct)
        {
            if (request is null || string.IsNullOrWhiteSpace(request.UserId))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "INVALID_USER",
                    message: "User id is required.",
                    statusCode: 400);
            }

            var userExists = await _identityContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == request.UserId, ct);

            if (!userExists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: 404);
            }

            var roleIds = await _identityContext.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.RoleId)
                .ToListAsync(ct);

            if (roleIds.Count > 1)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "MULTIPLE_ROLES_NOT_ALLOWED",
                    message: "The user has more than one role assigned. Only one role is allowed.",
                    statusCode: 400);
            }

            var roleId = roleIds.FirstOrDefault();

            request.PermissionIds ??= new List<int>();

            var requestedPermissionIds = request.PermissionIds
                .Distinct()
                .ToList();

            var allPermissions = await _permissionRepository.GetAllAsync(ct);

            var validPermissionIds = allPermissions
                .Where(x => x.IsActive)
                .Select(x => x.Id)
                .ToHashSet();

            var invalidPermissionIds = requestedPermissionIds
                .Where(x => !validPermissionIds.Contains(x))
                .ToList();

            if (invalidPermissionIds.Any())
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "INVALID_PERMISSION",
                    message: $"One or more permissions do not exist or are not active. Invalid ids: {string.Join(", ", invalidPermissionIds)}",
                    statusCode: 400);
            }

            var inheritedPermissionIds = string.IsNullOrWhiteSpace(roleId)
                ? new HashSet<int>()
                : (await _rolePermissionRepository.GetPermissionIdsByRoleAsync(roleId, ct)).ToHashSet();

            var directOnlyPermissionIds = requestedPermissionIds
                .Where(x => !inheritedPermissionIds.Contains(x))
                .ToList();

            if (requestedPermissionIds.Any() && !directOnlyPermissionIds.Any())
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "PERMISSION_ALREADY_INHERITED",
                    message: "The selected permission(s) are already granted through the user's role and cannot be saved as direct permissions.",
                    statusCode: 400);
            }

            await _userPermissionRepository.DeleteByUserIdAsync(request.UserId, ct);

            var entities = directOnlyPermissionIds
                .Select(permissionId => new AuthUserPermission
                {
                    UserId = request.UserId,
                    PermissionId = permissionId
                })
                .ToList();

            await _userPermissionRepository.AddRangeAsync(entities, ct);

            return ApiResponseFactory.Success(true, "User direct permissions saved successfully.");
        }

        /// <summary>
        /// Gets effective permissions for the authenticated user.
        /// </summary>
        /// <param name="userId">Authenticated user id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Current user effective permissions.</returns>
        public async Task<ApiResponse<CurrentUserPermissionsDTO>> GetCurrentUserPermissionsAsync(string userId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return ApiResponseFactory.Fail<CurrentUserPermissionsDTO>(
                    error: "UNAUTHORIZED",
                    message: "Authenticated user was not found.",
                    statusCode: 401);
            }

            var userExists = await _identityContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId, ct);

            if (!userExists)
            {
                return ApiResponseFactory.Fail<CurrentUserPermissionsDTO>(
                    error: "USER_NOT_FOUND",
                    message: "User not found.",
                    statusCode: 404);
            }

            var roleIds = await _identityContext.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync(ct);

            if (roleIds.Count > 1)
            {
                return ApiResponseFactory.Fail<CurrentUserPermissionsDTO>(
                    error: "MULTIPLE_ROLES_NOT_ALLOWED",
                    message: "The user has more than one role assigned. Only one role is allowed.",
                    statusCode: 400);
            }

            var roleId = roleIds.FirstOrDefault();

            var rolePermissionIds = string.IsNullOrWhiteSpace(roleId)
                ? new List<int>()
                : await _rolePermissionRepository.GetPermissionIdsByRoleAsync(roleId, ct);

            var directPermissionIds = await _userPermissionRepository
                .GetPermissionIdsByUserAsync(userId, ct);

            var permissionIds = rolePermissionIds
                .Union(directPermissionIds)
                .Distinct()
                .ToList();

            var permissions = await _permissionRepository.GetByIdsAsync(permissionIds, ct);

            var keys = permissions
                .Where(x => x.IsActive)
                .Select(BuildPermissionKey)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(x => x)
                .ToList();

            var dto = new CurrentUserPermissionsDTO
            {
                UserId = userId,
                Permissions = keys
            };

            return ApiResponseFactory.Success(dto, "Permissions loaded successfully.");
        }

        /// <summary>
        /// Maps a permission entity to a role/user permission DTO item.
        /// </summary>
        /// <param name="permission">Permission entity.</param>
        /// <returns>Mapped dto item.</returns>
        private static RolePermissionItemDTO MapPermissionToDto(AuthPermission permission)
        {
            return new RolePermissionItemDTO
            {
                PermissionId = permission.Id,
                PermissionKey = permission.PermissionKey,
                Group = permission.GroupName?.Trim() ?? string.Empty,
                Resource = permission.ModuleName?.Trim() ?? string.Empty,
                Action = permission.ActionName?.Trim() ?? string.Empty,
                Description = permission.Description?.Trim(),
                IsActive = permission.IsActive,
                Assigned = true,
                DisplayOrder = permission.DisplayOrder
            };
        }

        /// <summary>
        /// Builds the permission key for UI usage.
        /// Example: Masters.Locations.Create
        /// </summary>
        /// <param name="permission">Permission entity.</param>
        /// <returns>Formatted permission key.</returns>
        private static string BuildPermissionKey(AuthPermission permission)
        {
            var group = permission.GroupName?.Trim() ?? string.Empty;
            var resource = permission.ModuleName?.Trim() ?? string.Empty;
            var action = permission.ActionName?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(group) ||
                string.IsNullOrWhiteSpace(resource) ||
                string.IsNullOrWhiteSpace(action))
            {
                return string.Empty;
            }

            return $"{group}.{resource}.{action}";
        }
    }
}
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

            var allPermissions = await _permissionRepository.GetAllAsync(ct);

            var directPermissionIds = await _userPermissionRepository.GetPermissionIdsByUserAsync(userId, ct);

            var roleIds = await _identityContext.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.RoleId)
                .ToListAsync(ct);

            var inheritedPermissionIds = new HashSet<int>();

            foreach (var roleId in roleIds)
            {
                var rolePermissionIds = await _rolePermissionRepository.GetPermissionIdsByRoleAsync(roleId, ct);

                foreach (var permissionId in rolePermissionIds)
                {
                    inheritedPermissionIds.Add(permissionId);
                }
            }

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

            // Catálogo editable para permisos directos:
            // todos los permisos activos que NO vienen heredados por rol.
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
        /// <summary>
        /// save user permission
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
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

            var roleIds = await _identityContext.UserRoles
                .AsNoTracking()
                .Where(x => x.UserId == request.UserId)
                .Select(x => x.RoleId)
                .ToListAsync(ct);

            var inheritedPermissionIds = new HashSet<int>();

            foreach (var roleId in roleIds)
            {
                var rolePermissionIds = await _rolePermissionRepository.GetPermissionIdsByRoleAsync(roleId, ct);

                foreach (var permissionId in rolePermissionIds)
                {
                    inheritedPermissionIds.Add(permissionId);
                }
            }

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
    }
}

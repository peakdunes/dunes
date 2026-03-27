using DUNES.API.Models.Auth;
using DUNES.API.ModelsWMS.Auth;
using DUNES.API.RepositoriesWMS.Auth;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.ComponentModel.DataAnnotations;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service implementation for permission catalog operations.
    /// </summary>
    public class AuthPermissionService : IAuthPermissionService
    {
        private readonly IAuthPermissionRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPermissionService"/> class.
        /// </summary>
        /// <param name="repository">Permission repository.</param>
        public AuthPermissionService(IAuthPermissionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all permissions from the catalog.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission records.</returns>
        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetAllAsync(CancellationToken ct)
        {
            var permissions = await _repository.GetAllAsync(ct);

            var data = permissions
                .Select(MapToReadDTO)
                .ToList();

            return ApiResponseFactory.Success(data, "Permissions loaded successfully.");
        }

        /// <summary>
        /// Retrieves a permission by its identifier.
        /// </summary>
        /// <param name="id">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Permission record if found.</returns>
        public async Task<ApiResponse<AuthPermissionReadDTO>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
            {
                return ApiResponseFactory.Fail<AuthPermissionReadDTO>(
                    error: "PERMISSION_NOT_FOUND",
                    message: "Permission not found.",
                    statusCode: 404);
            }

            return ApiResponseFactory.Success(
                MapToReadDTO(entity),
                "Permission loaded successfully.");
        }

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="dto">Permission creation data.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission record.</returns>
        public async Task<ApiResponse<AuthPermissionReadDTO>> CreateAsync(AuthPermissionCreateDTO dto, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(dto.PermissionKey))
            {
                return ApiResponseFactory.Fail<AuthPermissionReadDTO>(
                    error: "INVALID_PERMISSION_KEY",
                    message: "Permission key is required.",
                    statusCode: 400);
            }

            //var normalizedKey = dto.PermissionKey.Trim().ToUpperInvariant();

            //var existing = await _repository.GetByKeyAsync(normalizedKey, ct);

            var existing = await _repository.GetByKeyAsync(dto.PermissionKey.Trim(), ct);

            if (existing is not null)
            {
                return ApiResponseFactory.Fail<AuthPermissionReadDTO>(
                    error: "PERMISSION_ALREADY_EXISTS",
                    message: "A permission with the same key already exists.",
                    statusCode: 400);
            }

            var entity = new AuthPermission
            {
                PermissionKey = dto.PermissionKey.Trim(),
                Description = dto.Description?.Trim(),
                IsActive = dto.IsActive,
                GroupName  = dto.GroupName.Trim(),
                ModuleName = dto.ModuleName.Trim(),
                ActionName = dto.ActionName.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Success(
                MapToReadDTO(created),
                "Permission created successfully.");
        }

        /// <summary>
        /// Maps a permission entity to its read DTO representation.
        /// </summary>
        /// <param name="entity">Permission entity.</param>
        /// <returns>Mapped read DTO.</returns>
        private static AuthPermissionReadDTO MapToReadDTO(AuthPermission entity)
        {
            return new AuthPermissionReadDTO
            {
                Id = entity.Id,
                PermissionKey = entity.PermissionKey,
                Description = entity.Description,
                GroupName = entity.GroupName,
                ModuleName = entity.ModuleName,
                ActionName = entity.ActionName,

               
                MvcActionName = entity.MvcActionName,
                ButtonText = entity.ButtonText,
                IconCss = entity.IconCss,
                ButtonCss = entity.ButtonCss,
                TextCss = entity.TextCss,
                ButtonOrder = entity.ButtonOrder,
                ShowAsRowAction = entity.ShowAsRowAction,
                ShowAsToolbarAction = entity.ShowAsToolbarAction,
                RequiresConfirmation = entity.RequiresConfirmation,
                ConfirmationMessage = entity.ConfirmationMessage,

                // EXISTENTES
                IsActive = entity.IsActive,
                DisplayOrder = entity.DisplayOrder,
                CreatedAt = entity.CreatedAt
            };
        }

        /// <summary>
        /// Retrieves all permissions that belong to a specific functional group and module.
        /// This method returns the complete permission catalog for the requested module.
        /// </summary>
        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetByModuleAsync(
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            var permissions = await _repository.GetByModuleAsync(groupName, moduleName, ct);

            var data = permissions
                .Select(MapToReadDTO)
                .ToList();

            return ApiResponseFactory.Success(data, "Permissions loaded successfully.");
        }

        /// <summary>
        /// Retrieves active permissions configured as row-level actions for a module.
        /// </summary>
        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetRowActionsByModuleAsync(
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            var permissions = await _repository.GetRowActionsByModuleAsync(groupName, moduleName, ct);

            var data = permissions
                .Select(MapToReadDTO)
                .ToList();

            return ApiResponseFactory.Success(data, "Row actions loaded successfully.");
        }

        /// <summary>
        /// Retrieves active permissions configured as toolbar actions for a module.
        /// </summary>
        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetToolbarActionsByModuleAsync(
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            var permissions = await _repository.GetToolbarActionsByModuleAsync(groupName, moduleName, ct);

            var data = permissions
                .Select(MapToReadDTO)
                .ToList();

            return ApiResponseFactory.Success(data, "Toolbar actions loaded successfully.");
        }
    }
}
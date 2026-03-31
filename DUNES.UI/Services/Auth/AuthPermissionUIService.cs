using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Auth
{
    /// <summary>
    /// Service implementation for permission catalog operations from the MVC UI.
    /// </summary>
    public class AuthPermissionUIService : UIApiServiceBase, IAuthPermissionUIService
    {
        private readonly IUserPermissionSessionHelper _permissionSessionHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthPermissionUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        /// <param name="permissionSessionHelper">Permission session helper.</param>
        public AuthPermissionUIService(
            IHttpClientFactory factory,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(factory)
        {
            _permissionSessionHelper = permissionSessionHelper;
        }

        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetAllAsync(string token, CancellationToken ct)
        {
            return await GetApiAsync<List<AuthPermissionReadDTO>>(
                "/api/AuthPermission/GetAll",
                token,
                ct);
        }

        public async Task<ApiResponse<AuthPermissionReadDTO>> CreateAsync(string token, AuthPermissionCreateDTO dto, CancellationToken ct)
        {
            return await PostApiAsync<AuthPermissionReadDTO, AuthPermissionCreateDTO>(
                "/api/AuthPermission/Create",
                dto,
                token,
                ct);
        }

        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetByModuleAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            return await GetApiAsync<List<AuthPermissionReadDTO>>(
                $"/api/AuthPermission/GetByModule/{Uri.EscapeDataString(groupName)}/{Uri.EscapeDataString(moduleName)}",
                token,
                ct);
        }

        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetRowActionsByModuleAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            return await GetApiAsync<List<AuthPermissionReadDTO>>(
                $"/api/AuthPermission/GetRowActionsByModule/{Uri.EscapeDataString(groupName)}/{Uri.EscapeDataString(moduleName)}",
                token,
                ct);
        }

        public async Task<ApiResponse<List<AuthPermissionReadDTO>>> GetToolbarActionsByModuleAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            return await GetApiAsync<List<AuthPermissionReadDTO>>(
                $"/api/AuthPermission/GetToolbarActionsByModule/{Uri.EscapeDataString(groupName)}/{Uri.EscapeDataString(moduleName)}",
                token,
                ct);
        }

        public async Task<List<CrudActionItemModel>> BuildRowActionsAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            var response = await GetRowActionsByModuleAsync(token, groupName, moduleName, ct);

            if (!response.Success || response.Data is null)
                return new List<CrudActionItemModel>();

            return response.Data
                .Where(x => _permissionSessionHelper.HasPermission(x.PermissionKey))
                .Where(x => !string.IsNullOrWhiteSpace(x.MvcActionName))
                .OrderBy(x => x.ButtonOrder)
                .ThenBy(x => x.DisplayOrder)
                .Select(MapToCrudActionItem)
                .ToList();
        }

        public async Task<List<CrudActionItemModel>> BuildToolbarActionsAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct)
        {
            var response = await GetToolbarActionsByModuleAsync(token, groupName, moduleName, ct);

            if (!response.Success || response.Data is null)
                return new List<CrudActionItemModel>();

            return response.Data
                .Where(x => _permissionSessionHelper.HasPermission(x.PermissionKey))
                .Where(x => !string.IsNullOrWhiteSpace(x.MvcActionName))
                .OrderBy(x => x.ButtonOrder)
                .ThenBy(x => x.DisplayOrder)
                .Select(MapToCrudActionItem)
                .ToList();
        }

        private static CrudActionItemModel MapToCrudActionItem(AuthPermissionReadDTO dto)
        {
            return new CrudActionItemModel
            {
                PermissionKey = dto.PermissionKey,
                MvcActionName = dto.MvcActionName ?? string.Empty,
                ButtonText = dto.ButtonText,
                IconCss = dto.IconCss,
                ButtonCss = dto.ButtonCss,
                TextCss = dto.TextCss,
                ButtonOrder = dto.ButtonOrder,
                RequiresConfirmation = dto.RequiresConfirmation,
                ConfirmationMessage = dto.ConfirmationMessage,
                RouteParamsTemplate = dto.RouteParamsTemplate
               
            };
        }
    }
}
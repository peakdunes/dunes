using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Models;

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

        /// <summary>
        /// Retrieves all permissions that belong to a specific functional group and module.
        /// This method is useful when the UI needs the complete permission catalog for a module,
        /// including permissions that may not be rendered as buttons.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions for the requested module.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetByModuleAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct);

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as row-level actions in index tables.
        /// Example: Edit, Delete, ResetPassword, Deactivate.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of row-action permissions for the requested module.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetRowActionsByModuleAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct);

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as toolbar or header actions in index views.
        /// Example: Create, Export, Process.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of toolbar-action permissions for the requested module.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetToolbarActionsByModuleAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct);


        /// <summary>
        /// Builds the final row-action button models for a module using the permission catalog
        /// returned by the API. This method prepares the exact structure expected by the
        /// reusable CRUD partial in MVC views.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of row-action button models ready for UI rendering.</returns>
        Task<List<CrudActionItemModel>> BuildRowActionsAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct);

        /// <summary>
        /// Builds the final toolbar-action button models for a module using the permission catalog
        /// returned by the API. This method prepares the exact structure expected by MVC views
        /// for toolbar or header actions.
        /// </summary>
        /// <param name="token">Authenticated API token.</param>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of toolbar-action button models ready for UI rendering.</returns>
        Task<List<CrudActionItemModel>> BuildToolbarActionsAsync(
            string token,
            string groupName,
            string moduleName,
            CancellationToken ct);
    }
}
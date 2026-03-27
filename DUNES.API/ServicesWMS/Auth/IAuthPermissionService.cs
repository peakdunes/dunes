using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Auth
{
    /// <summary>
    /// Service contract for managing permission catalog operations.
    /// </summary>
    public interface IAuthPermissionService
    {
        /// <summary>
        /// Retrieves all permissions from the catalog.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permission records.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// Retrieves a permission by its identifier.
        /// </summary>
        /// <param name="id">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Permission record if found.</returns>
        Task<ApiResponse<AuthPermissionReadDTO>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// Creates a new permission in the catalog.
        /// </summary>
        /// <param name="dto">Permission creation data.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created permission record.</returns>
        Task<ApiResponse<AuthPermissionReadDTO>> CreateAsync(AuthPermissionCreateDTO dto, CancellationToken ct);

        /// <summary>
        /// Retrieves all permissions that belong to a specific functional group and module.
        /// This method returns the complete permission catalog for the requested module.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of permissions for the requested module.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetByModuleAsync(
            string groupName,
            string moduleName,
            CancellationToken ct);

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as row-level actions in index tables.
        /// Example: Edit, Delete, ResetPassword, Deactivate.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of row-action permissions for the requested module.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetRowActionsByModuleAsync(
            string groupName,
            string moduleName,
            CancellationToken ct);

        /// <summary>
        /// Retrieves active permissions for a specific functional group and module
        /// that are configured to be rendered as toolbar or header actions in index views.
        /// Example: Create, Export, Process.
        /// </summary>
        /// <param name="groupName">Functional group name. Example: Masters, Auth, Reports.</param>
        /// <param name="moduleName">Module name. Example: Locations, Users, CompanyClientItemStatus.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of toolbar-action permissions for the requested module.</returns>
        Task<ApiResponse<List<AuthPermissionReadDTO>>> GetToolbarActionsByModuleAsync(
            string groupName,
            string moduleName,
            CancellationToken ct);
    }
}
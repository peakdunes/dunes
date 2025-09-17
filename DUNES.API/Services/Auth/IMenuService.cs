using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Auth
{
    /// <summary>
    /// Service interface for handling menu business logic.
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// Gets a hierarchical menu structure filtered by the user roles.
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MenuItemDto>>> GetMenuHierarchyAsync(IEnumerable<string> userRoles, CancellationToken ct);

        /// <summary>
        /// Gets the level 2 menus associated with level 1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MenuItemDto>>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles, CancellationToken ct);



        /// <summary>
        /// get all menu option for a specific code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> BuildBreadcrumbAsync(string code, IEnumerable<string> roles, CancellationToken ct);

        /// <summary>
        /// get all menu information for a controller/action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<MenuItemDto>> GetCodeByControllerAction(string controller, string action, CancellationToken ct);
    }
}

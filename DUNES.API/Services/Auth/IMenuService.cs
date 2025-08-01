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
        /// <param name="userRoles">Roles of the logged-in user</param>
        /// <returns>Hierarchical list of MenuItemDto</returns>
        Task<ApiResponse<List<MenuItemDto>>> GetMenuHierarchyAsync(IEnumerable<string> userRoles);

        /// <summary>
        /// Gets the level 2 menus associated with level 1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <returns></returns>
        Task<ApiResponse<List<MenuItemDto>>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles);
        
    }
}

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using System.Collections.Generic;

namespace DUNES.API.Repositories.Auth
{
    /// <summary>
    /// Repository interface for accessing menu data from the database.
    /// </summary>
    public interface IMenuRepository
    {
        /// <summary>
        /// Retrieves all active menu items from the database.
        /// </summary>
        /// <returns>List of MenuItemDto with flat structure (not hierarchical yet).</returns>
        Task<List<MenuItemDto>> GetAllActiveMenusAsync(IEnumerable<string> userRoles);

        /// <summary>
        /// Gets the level 2 menus associated with level 1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles);

        /// <summary>
        /// get all menu options for role
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> GetAllMenusByRolesAsync(IEnumerable<string> roles);
    }
}

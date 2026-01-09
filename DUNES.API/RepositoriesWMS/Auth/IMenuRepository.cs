using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using System.Collections.Generic;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Repository interface for accessing menu data from the database.
    /// </summary>
    public interface IMenuRepository
    {

        /// <summary>
        /// get all menu options for create navegation scrum
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> GetAllMenusAsync(CancellationToken ct);



        /// <summary>
        /// get all menu for rol
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> GetAllActiveMenusAsync(IEnumerable<string> userRoles, CancellationToken ct);

        /// <summary>
        /// Gets the level 2 menus associated with level 1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles, CancellationToken ct);

        /// <summary>
        /// get all menu options for role
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MenuItemDto>> GetAllMenusByRolesAsync(IEnumerable<string> roles, CancellationToken ct);

        /// <summary>
        /// get all menu information for a controller/action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<MenuItemDto> GetCodeByControllerAction(string controller, string action, CancellationToken ct);
    }
}

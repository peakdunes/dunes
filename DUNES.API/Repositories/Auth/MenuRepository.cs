using DUNES.API.Data;

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.Auth
{
    /// <summary>
    /// Concrete repository for fetching menu items from the database.
    /// </summary>
    public class MenuRepository : IMenuRepository
    {

        private readonly AppDbContext _context;

        /// <summary>
        /// dependence injection
        /// </summary>
        /// <param name="context"></param>
        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all active menu items from the mvcPartRunnerMenu table.
        /// </summary>
        public async Task<List<MenuItemDto>> GetAllActiveMenusAsync(IEnumerable<string> userRoles)
        {
            var roleList = userRoles.ToList();
            var menus = await _context.MvcPartRunnerMenu
                .Where(m => m.Active == true && m.Code.Length == 2)
                .OrderBy(m => m.Order)
                .ToListAsync(); // ejecuta la query en SQL

            return menus
                .Where(m => roleList.Any(role => m.Roles.Contains(role))) // filtro en memoria
                .Select(m => new MenuItemDto
                {
                    Code = m.Code,
                    Title = m.Level1,
                    Utility = m.Utility,
                    Controller = m.Controller,
                    Action = m.Action,
                    Roles = m.Roles,
                    Active = m.Active,
                    Order = m.Order
                })
                .ToList();
        }
        /// <summary>
        /// get level2 menu options for level1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles)
        {

            var roleList = roles.ToList();
            var menus = await _context.MvcPartRunnerMenu
                .Where(m => m.Active == true && m.Code.Length == 4 && m.Code.StartsWith(level1.Trim()))
                .OrderBy(m => m.Order)
                .ToListAsync(); // ejecuta la query en SQL

            return menus
                .Where(m => roleList.Any(role => m.Roles!.Contains(role))) // filtro en memoria
                .Select(m => new MenuItemDto
                {
                    Code = m.Code,
                    Title = m.Level2,
                    Utility = m.Utility,
                    Controller = m.Controller,
                    Action = m.Action,
                    Roles = m.Roles,
                    Active = m.Active,
                    Order = m.Order
                })
                .ToList();
        }

        /// <summary>
        /// get all menu option for role
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetAllMenusByRolesAsync(IEnumerable<string> roles)
        {
            var roleList = roles.ToList();
            var menus = await _context.MvcPartRunnerMenu
                .Where(m => m.Active == true)
                .OrderBy(m => m.Order)
                .ToListAsync(); // ejecuta la query en SQL

            return menus
                .Where(m => roleList.Any(role => m.Roles!.Contains(role))) // filtro en memoria
                .Select(m => new MenuItemDto
                {
                    Code = m.Code,
                    Title = m.Level2,
                    Utility = m.Utility,
                    Controller = m.Controller,
                    Action = m.Action,
                    Roles = m.Roles,
                    Active = m.Active,
                    Order = m.Order
                })
                .ToList();
        }

    }
}

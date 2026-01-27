using DUNES.API.Data;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Auth
{
    /// <summary>
    /// Concrete repository for fetching menu items from the database.
    /// </summary>
    public class MenuRepository : IMenuRepository
    {

        private readonly IdentityDbContext _context;

        /// <summary>
        /// dependence injection
        /// </summary>
        /// <param name="context"></param>
        public MenuRepository(IdentityDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all active menu items from the mvcPartRunnerMenu table for a role list.
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetAllActiveMenusAsync(IEnumerable<string> userRoles, CancellationToken ct)
        {
            var roleList = userRoles.ToList();
            var menus = await _context.Menu
                .Where(m => m.Active == true && m.Code!.Length == 2)
                .OrderBy(m => m.Order)
                .ToListAsync(ct); // ejecuta la query en SQL

            return menus
                .Select(m => new MenuItemDto
                {
                    Code = m.Code ?? string.Empty,
                    Title = m.Title ?? string.Empty,
                    Utility = m.Utility ?? string.Empty,
                    Controller = m.Controller ?? string.Empty,
                    Action = m.Action ?? string.Empty,
                    Active = m.Active,
                    Order = m.Order,
                    previousmenu = ""
                })
                .ToList();
        }
        /// <summary>
        /// get level2 menu options for level1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles, CancellationToken ct)
        {
            string previoumenu = string.Empty;

            var large = level1.Length;

            //previoumenu = level1.ToString().Substring(0, large - 2);

            int nextlevel = large + 2;

            var roleList = roles.ToList();
            var menus = await _context.Menu
                .Where(m => m.Active == true && m.Code.Length == nextlevel && m.Code.StartsWith(level1.Trim()))
                  .OrderBy(m => m.Order)
                .ToListAsync(ct); 

           var menu2 = menus
               
                .Select(m => new MenuItemDto
                {
                    Code = m.Code ?? string.Empty,
                    Title = m.Title ?? string.Empty,
                    Utility = m.Utility ?? string.Empty,
                    Controller = m.Controller ?? string.Empty,
                    Action = m.Action ?? string.Empty,
                    Active = m.Active,
                    Order = m.Order,
                    previousmenu = level1
                  
                })
                .ToList();

            return menu2;
        }

        /// <summary>
        /// get all menu option for role
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetAllMenusByRolesAsync(IEnumerable<string> roles, CancellationToken ct)
        {
            var roleList = roles.ToList();
            var menus = await _context.Menu
                .Where(m => m.Active == true)
                .OrderBy(m => m.Order)
                .ToListAsync(ct); // ejecuta la query en SQL

           var menu2 = menus
               
                .Select(m => new MenuItemDto
                {
                    Code = m.Code ?? string.Empty,
                    Title = m.Title ?? string.Empty,
                    Utility = m.Utility ?? string.Empty,
                    Controller = m.Controller ?? string.Empty,
                    Action = m.Action ?? string.Empty,
                   
                    Active = m.Active,
                    Order = m.Order,
                    previousmenu = !string.IsNullOrEmpty(m.Code) && m.Code.Length > 2
                                ? m.Code.Substring(0, m.Code.Length - 2)
                                : m.Code ?? string.Empty

                })
                .ToList();

            return menu2;
        }

        /// <summary>
        /// get all menu information for a controller/action
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<MenuItemDto> GetCodeByControllerAction(string controller, string action, CancellationToken ct)
        {

            var m = await _context.Menu
                .FirstOrDefaultAsync(m => m.Active == true
                    && m.Controller!.ToLower() == controller.ToLower()
                    && m.Action!.ToLower() == action.ToLower(),ct
                );

            if (m == null) return null;

            return  new MenuItemDto
                {
                    Code = m.Code ?? string.Empty,
                    Title = m.Title ?? string.Empty,
                    Utility = m.Utility ?? string.Empty,
                    Controller = m.Controller ?? string.Empty,
                    Action = m.Action ?? string.Empty,
                 
                    Active = m.Active,
                    Order = m.Order,
                    previousmenu = !string.IsNullOrEmpty(m.Code) && m.Code.Length > 2 ? m.Code.Substring(0, m.Code.Length - 2) : m.Code ?? string.Empty
                };
        }
        /// <summary>
        /// return all active options menu
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetAllMenusAsync(CancellationToken ct)
        {
            var menus = await _context.Menu
              .Where(m => m.Active == true)
              .OrderBy(m => m.Order)
              .ToListAsync(ct); // ejecuta la query en SQL

            var menu2 = menus
                 .Select(m => new MenuItemDto
                 {
                     Code = m.Code ?? string.Empty,
                     Title = m.Title ?? string.Empty,
                     Utility = m.Utility ?? string.Empty,
                     Controller = m.Controller ?? string.Empty,
                     Action = m.Action ?? string.Empty,
                    
                     Active = m.Active,
                     Order = m.Order,
                     previousmenu = !string.IsNullOrEmpty(m.Code) && m.Code.Length > 2
                         ? m.Code.Substring(0, m.Code.Length - 2)
                         : string.Empty  

                 })
                 .ToList();

            return menu2;
        }
    }
}

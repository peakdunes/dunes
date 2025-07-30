using DUNES.API.Data;

using DUNES.Shared.DTOs.Auth;
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
        public async Task<List<MenuItemDto>> GetAllActiveMenusAsync()
        {
            return await _context.MvcPartRunnerMenu
                .Where(m => m.Active == true && m.Code.Length ==2)
                .OrderBy(m => m.Order)
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
                .ToListAsync();
        }
    }
}

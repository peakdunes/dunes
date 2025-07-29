using APIZEBRA.DTOs.Auth;
using APIZEBRA.Repositories.Auth;

namespace APIZEBRA.Services.Auth
{
    /// <summary>
    /// Handles the business logic for menu retrieval and hierarchy building.
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _repository;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public MenuService(IMenuRepository repository)
        {
            _repository = repository;
        }



        /// <summary>
        /// Builds the menu hierarchy for the given user roles.
        /// </summary>
        public async Task<List<MenuItemDto>> GetMenuHierarchyAsync(IEnumerable<string> userRoles)
        {
            // 1 Get flat menu list from repository
            var flatMenu = await _repository.GetAllActiveMenusAsync();

            // 2 Filter by roles
            //flatMenu = flatMenu
            //    .Where(m => m.Roles != null &&
            //                m.Roles.Split(',')
            //                       .Any(role => userRoles.Contains(role.Trim(), StringComparer.OrdinalIgnoreCase)))
            //    .ToList();

            // 3 Build recursive hierarchy
            return BuildHierarchy(flatMenu, null);
        }

        /// <summary>
        /// Recursively builds a menu hierarchy from flat data.
        /// </summary>
        private List<MenuItemDto> BuildHierarchy(List<MenuItemDto> allMenus, string? parentCode)
        {
            return allMenus
                .Where(m => parentCode == null
                    ? m.Code.Length == 2   // Level 1 items
                    : m.Code.StartsWith(parentCode) && m.Code.Length == parentCode.Length + 2)
                .Select(m => new MenuItemDto
                {
                    Code = m.Code,
                    Title = m.Title,
                    Utility = m.Utility,
                    Controller = m.Controller,
                    Action = m.Action,
                    Roles = m.Roles,
                    Active = m.Active,
                    Order = m.Order,
                    Children = BuildHierarchy(allMenus, m.Code)
                })
                .OrderBy(m => m.Order)
                .ToList();
        }
    }
}

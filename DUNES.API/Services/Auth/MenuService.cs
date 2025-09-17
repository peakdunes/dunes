using DUNES.API.DTOs.B2B;
using DUNES.API.Repositories.Auth;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using System.Data;


namespace DUNES.API.Services.Auth
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
        /// Gets the level 2 menus associated with level1 passed as parameter
        /// </summary>
        /// <param name="level1"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<MenuItemDto>>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles, CancellationToken ct)
        {
            var flatMenu = await _repository.GetLevel2MenuOptions(level1, roles, ct);

            return ApiResponseFactory.Ok(flatMenu, "Menu options level2 availables for this user");
        }



        /// <summary>
        /// get menu options nivel 1 for role list
        /// </summary>
        /// <param name="userRoles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<MenuItemDto>>> GetMenuHierarchyAsync(IEnumerable<string> userRoles, CancellationToken ct)
        {




            // 1 Get flat menu list from repository
            var flatMenu = await _repository.GetAllActiveMenusAsync(userRoles, ct);

            return ApiResponseFactory.Ok(flatMenu, "Menu options availables for this user");

        }

        /// <summary>
        /// get all menu option for a specific code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> BuildBreadcrumbAsync(string code, IEnumerable<string> roles, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return new List<MenuItemDto>
                    {
                        new MenuItemDto
                        {
                            Code = "root",
                            Title = "Menú General"
                        }
                    };
            }

            var allMenus = await _repository.GetAllMenusByRolesAsync(roles, ct);

            var breadcrumb = new List<MenuItemDto>();
                     
            for (int i = 2; i <= code.Length; i += 2)
            {
                var partialCode = code.Substring(0, i);
                var match = allMenus.FirstOrDefault(m => m.Code == partialCode);
                if (match != null)
                {
                    breadcrumb.Add(match);
                }
            }

            return breadcrumb;
        }
        /// <summary>
        /// Get all information about a menu option
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<MenuItemDto>> GetCodeByControllerAction(string controller, string action, CancellationToken ct)
        {
            var MenuInfo = await _repository.GetCodeByControllerAction(controller, action, ct);


            if (MenuInfo == null)
            {

                return ApiResponseFactory.NotFound<MenuItemDto>("Information Not found");
               

            }
            else
            {

                return ApiResponseFactory.Ok(MenuInfo, "OK");
            }
        
        }

      
    }
}

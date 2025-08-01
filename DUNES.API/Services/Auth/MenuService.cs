using DUNES.API.DTOs.B2B;
using DUNES.API.Repositories.Auth;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;


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
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<MenuItemDto>>> GetLevel2MenuOptions(string level1, IEnumerable<string> roles)
        {
            var flatMenu = await _repository.GetLevel2MenuOptions(level1, roles);

            return ApiResponseFactory.Ok(flatMenu, "Menu options level2 availables for this user");
        }



        /// <summary>
        /// get menu options nivel 1 for role list
        /// </summary>
        /// <param name="userRoles"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<MenuItemDto>>> GetMenuHierarchyAsync(IEnumerable<string> userRoles)
        {
            // 1 Get flat menu list from repository
            var flatMenu = await _repository.GetAllActiveMenusAsync(userRoles);

            return ApiResponseFactory.Ok(flatMenu, "Menu options availables for this user");

        }

     

    }
}

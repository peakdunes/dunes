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
        /// Builds the menu hierarchy for the given user roles.
        /// </summary>
        //public async Task<List<MenuItemDto>> GetMenuHierarchyAsync(IEnumerable<string> userRoles)
        //{
        //    // 1 Get flat menu list from repository
        //    var flatMenu = await _repository.GetAllActiveMenusAsync(userRoles);

        //    return (flatMenu);

        //}

        public async Task<ApiResponse<List<MenuItemDto>>> GetMenuHierarchyAsync(IEnumerable<string> userRoles)
        {
            // 1 Get flat menu list from repository
            var flatMenu = await _repository.GetAllActiveMenusAsync(userRoles);

            return ApiResponseFactory.Ok(flatMenu, "Menu options availables for this user");

        }

     

    }
}

using DUNES.Shared.DTOs.Auth;
using DUNES.API.Repositories.Auth;


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
        public async Task<List<MenuItemDto>> GetMenuHierarchyAsync(IEnumerable<string> userRoles)
        {
            // 1 Get flat menu list from repository
            var flatMenu = await _repository.GetAllActiveMenusAsync();

            return (flatMenu);

        }

       
    }
}

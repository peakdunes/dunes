using DUNES.Shared.DTOs.Auth;

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
        Task<List<MenuItemDto>> GetAllActiveMenusAsync();
    }
}

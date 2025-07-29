using APIZEBRA.DTOs.Auth;

namespace APIZEBRA.Services.Auth
{
    /// <summary>
    /// Service interface for handling menu business logic.
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// Gets a hierarchical menu structure filtered by the user roles.
        /// </summary>
        /// <param name="userRoles">Roles of the logged-in user</param>
        /// <returns>Hierarchical list of MenuItemDto</returns>
        Task<List<MenuItemDto>> GetMenuHierarchyAsync(IEnumerable<string> userRoles);
    }
}

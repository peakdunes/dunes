using DUNES.Shared.DTOs.Auth;

namespace DUNES.UI.Services.Admin
{
    public interface IMenuClientUIService
    {

        Task<List<MenuItemDto>> GetBreadcrumbAsync(string menuCode, string token);

        Task<string?> GetCodeByControllerActionAsync(string controller, string action, string token);
    }
}

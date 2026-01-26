using DUNES.Shared.DTOs.Auth;
using DUNES.UI.Models;

namespace DUNES.UI.Services.Admin
{
    public interface IMenuClientUIService
    {

        Task<List<BreadcrumbItem>> GetBreadcrumbAsync(string token, string menuCode, CancellationToken ct = default);

        Task<string?> GetCodeByControllerActionAsync(string controller, string action, string token);

        Task<List<MenuItemDto>> GetMenuAsync(string token, CancellationToken ct = default);

        Task<List<MenuItemDto>> GetMenuLevel2Async(string token,string level1,    CancellationToken ct = default);
    }
}

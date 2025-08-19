using DUNES.Shared.DTOs.Auth;

namespace DUNES.UI.Services.Admin
{
    public interface IMenuClientService
    {

        Task<List<MenuItemDto>> GetBreadcrumbAsync(string menuCode, string token);

       
    }
}

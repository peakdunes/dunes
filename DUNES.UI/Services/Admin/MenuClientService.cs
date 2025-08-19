using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.Admin
{
    /// <summary>
    /// Navegation menu
    /// </summary>
    public class MenuClientService : IMenuClientService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;
      

        public MenuClientService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };

         

        }
        /// <summary>
        /// Return navegation menu (show in all views for menu navegation
        /// </summary>
        /// <param name="menuCode"></param>
        /// <returns></returns>
        public async Task<List<MenuItemDto>> GetBreadcrumbAsync(string menuCode, string token)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemDto>>>(
                    $"/api/Menu/api/menu/breadcrumb/{menuCode}");

                return response?.Success == true ? response.Data : new List<MenuItemDto>();
            }
            catch
            {
                return new List<MenuItemDto>(); // Si falla, devuelve vacío
            }
        }


    }
}

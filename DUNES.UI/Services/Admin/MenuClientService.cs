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

        public async Task<string?> GetCodeByControllerActionAsync(string controller, string action, string token)
        {

          

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // endpoint que devuelva un menú por controller/action
            var response = await _httpClient.GetAsync($"/api/Menu/codeByControllerAction?controller={controller}&action={action}");

            if (!response.IsSuccessStatusCode)
                return null;

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<MenuItemDto>>();

            return apiResponse?.Success == true ? apiResponse.Data?.Code : null;
        }
    }
}

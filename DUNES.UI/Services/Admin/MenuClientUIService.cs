using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.Admin
{
    /// <summary>
    /// Navegation menu
    /// </summary>
    public class MenuClientUIService : IMenuClientUIService
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;

        private const string CACHE_KEY = "GLOBAL_MENU";

        public MenuClientUIService(IConfiguration config, IMemoryCache cache)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _cache = cache;
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
        public async Task<List<BreadcrumbItem>> GetBreadcrumbAsync(string token, string menuCode,  CancellationToken ct)
        {

            var allMenus = await GetMenuAsync(token, ct);

            var breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem
            {
                Text = "Home",
                Url = "/"
            }
        };

            if (string.IsNullOrWhiteSpace(menuCode))
                return breadcrumb;

            // Construimos la cadena padre → hijo
            var stack = new Stack<MenuItemDto>();
            var current = menuCode;

            while (!string.IsNullOrWhiteSpace(current))
            {
                var node = allMenus.FirstOrDefault(m => m.Code == current);
                if (node == null)
                    break;

                stack.Push(node);

                if (string.IsNullOrWhiteSpace(node.previousmenu))
                    break;

                current = node.previousmenu;
            }

            var path = stack.ToList();

            for (int i = 0; i < path.Count; i++)
            {
                var item = path[i];
                bool isLast = (i == path.Count - 1);

                breadcrumb.Add(new BreadcrumbItem
                {
                    Text = item.Title,
                    Url = isLast ? null : $"/Menu/Level/{item.Code}"
                });
            }

            return breadcrumb;

            //try
            //{
            //    if (!string.IsNullOrWhiteSpace(token))
            //    {
            //        _httpClient.DefaultRequestHeaders.Authorization =
            //            new AuthenticationHeaderValue("Bearer", token);
            //    }

            //    var response = await _httpClient.GetFromJsonAsync<ApiResponse<List<MenuItemDto>>>(
            //        $"/api/Menu/api/menu/breadcrumb/{menuCode}");

            //    return response?.Success == true ? response.Data : new List<MenuItemDto>();
            //}
            //catch
            //{
            //    return new List<MenuItemDto>(); // Si falla, devuelve vacío
            //}
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

        public async Task<List<MenuItemDto>> GetMenuAsync(string token, CancellationToken ct = default)
        {
            if (_cache.TryGetValue(CACHE_KEY, out List<MenuItemDto> cached))
                return cached;
                        
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

           

            var response = await _httpClient.GetAsync("/api/Menu/all", ct);
            if (!response.IsSuccessStatusCode)
                return new List<MenuItemDto>();

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItemDto>>>(cancellationToken: ct);
            var menu = apiResponse?.Data ?? new List<MenuItemDto>();

            // Cache 30 minutos (puede ser más si el menú casi nunca cambia)
            _cache.Set(CACHE_KEY, menu, TimeSpan.FromMinutes(30));

            return menu;
        }
    }
}

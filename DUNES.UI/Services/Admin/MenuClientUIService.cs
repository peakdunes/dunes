using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Models;
using DUNES.UI.Services.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace DUNES.UI.Services.Admin
{
    /// <summary>
    /// Navigation menu service (UI)
    /// </summary>
    public class MenuClientUIService: UIApiServiceBase, IMenuClientUIService
    {
        private readonly IMemoryCache _cache; //“Esta clase puede guardar y leer datos temporales en memoria.”

        private const string CACHE_KEY = "GLOBAL_MENU";

        public MenuClientUIService(
            IHttpClientFactory factory,
            IMemoryCache cache)
            : base(factory)
        {
            _cache = cache;
        }

        /// <summary>
        /// Returns breadcrumb navigation for current menu
        /// </summary>
        public async Task<List<BreadcrumbItem>> GetBreadcrumbAsync(
            string token,
            string menuCode,
            CancellationToken ct)
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

            // Build parent → child path
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
        }

        /// <summary>
        /// Gets menu code by controller/action
        /// </summary>
        public async Task<string?> GetCodeByControllerActionAsync(
            string controller,
            string action,
            string token)
        {
            var resp = await GetApiAsync<MenuItemDto>(
                $"/api/Menu/codeByControllerAction?controller={controller}&action={action}",
                token,
                CancellationToken.None);

            return resp.Success
                ? resp.Data?.Code
                : null;
        }

        /// <summary>
        /// Gets full menu (cached)
        /// </summary>
        public async Task<List<MenuItemDto>> GetMenuAsync(
            string token,
            CancellationToken ct = default)
        {
            if (_cache.TryGetValue(CACHE_KEY, out List<MenuItemDto> cached))
                return cached;

            var resp = await GetApiAsync<List<MenuItemDto>>(
                "/api/Menu/all",
                token,
                ct);

            var menu = resp.Success
                ? resp.Data ?? new List<MenuItemDto>()
                : new List<MenuItemDto>();

            // Cache for 30 minutes
            _cache.Set(CACHE_KEY, menu, TimeSpan.FromMinutes(30));

            return menu;
        }
    }
}

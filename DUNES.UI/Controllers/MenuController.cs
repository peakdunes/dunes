using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;

namespace DUNES.UI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly IConfiguration _config;
        private readonly IMenuClientService _menuClientService;

        public MenuController(IHttpClientFactory httpClientFactory, IConfiguration config, IMenuClientService menuClientService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _menuClientService = menuClientService;
        }

       

        [HttpGet("Menu/Level/{level1}")]
        public async Task<IActionResult> Level(string level1)
        {
            string menuCode = level1;


            if (string.IsNullOrEmpty(level1))
            {
                ViewBag.ApiType = "warning";
                ViewBag.ApiMessage = "Level 1 invalid.";
                return View(); // regresar a la misma vista


            }

            var token = HttpContext.Session.GetString("JWToken");
            
            if (token == null)
            {

                MessageHelper.SetMessage(this, "danger", "Token Invalid. Please try again.", MessageDisplay.Inline);

                return RedirectToAction("Index","Home"); // regresar a la misma vista
            }

            var baseUrl = _config["ApiSettings:BaseUrl"];

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl!);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            //se busca la opcion de menu para la barra de navegacion

            var breadcrumb = await _menuClientService.GetBreadcrumbAsync(menuCode, token);

            ViewData["Breadcrumb"] = breadcrumb;

            //fin  la barra de navegacion


            // Ahora usamos el parámetro level1, no "01" fijo
            var response = await client.GetAsync($"/api/Menu/level2/{level1}");
            if (!response.IsSuccessStatusCode)
                return Json(new { });

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItemDto>>>();

            if (apiResponse == null || apiResponse.Data == null || !apiResponse.Success)
            {
                MessageHelper.SetMessage(this, "danger", apiResponse?.Message ?? "Menu level2 not found.", MessageDisplay.Inline);
                return View(new List<MenuItemDto>());
            }

            var menuItems = apiResponse.Data;


            // ViewBag.GlobalAlert = "El sistema estará en mantenimiento hoy a las 8 PM. ¡Guarde su trabajo!";

            return View(menuItems);


        }
    }
}

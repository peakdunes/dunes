using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DUNES.UI.Controllers.Admin
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly IConfiguration _config;
        private readonly IMenuClientUIService _menuClientService;

        public HomeController(IHttpClientFactory httpClientFactory, IConfiguration config, IMenuClientUIService menuClientService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _menuClientService = menuClientService;
        }

        public async Task<IActionResult> Index()
        {

           
           

            var token = HttpContext.Session.GetString("JWToken");

            // Si no hay login, redirigimos
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var baseUrl = _config["ApiSettings:BaseUrl"];

            // Llamamos a la API de APIZEBRA
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(baseUrl!);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);




            var response = await client.GetAsync("/api/Menu/options");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Menu could not be loaded.";
                return View(new List<MenuItemDto>());
            }

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<MenuItemDto>>>();

            if (apiResponse == null || apiResponse.Data == null || !apiResponse.Success)
            {
                MessageHelper.SetMessage(this, "danger", apiResponse?.Message ?? "No menu data received.", MessageDisplay.Inline);
                return View(new List<MenuItemDto>());
            }

            var menuItems = apiResponse.Data;


           // ViewBag.GlobalAlert = "El sistema estará en mantenimiento hoy a las 8 PM. ¡Guarde su trabajo!";

            return View(menuItems);

         
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

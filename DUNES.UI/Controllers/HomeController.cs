using DUNES.Shared.DTOs.Auth;
using DUNES.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DUNES.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");

            // Si no hay login, redirigimos
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Llamamos a la API de APIZEBRA
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5251");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/Menu/options");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Menu could not be loaded.";
                return View(new List<MenuItemDto>());
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var menuItems = JsonSerializer.Deserialize<List<MenuItemDto>>(jsonResponse);

            // Mandamos el menú completo a la vista
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

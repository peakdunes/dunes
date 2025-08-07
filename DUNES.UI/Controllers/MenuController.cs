using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
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

        public MenuController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        //[HttpGet]
        //public async Task<IActionResult> LoadMenu()
        //{
        //    var token = HttpContext.Session.GetString("JWToken");
        //    if (token == null)
        //        return Json(new { });

        //    var client = _httpClientFactory.CreateClient();
        //    client.BaseAddress = new Uri("http://localhost:5251"); // 
        //    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    var response = await client.GetAsync("/api/Menu/options");
        //    if (!response.IsSuccessStatusCode)
        //        return Json(new { });

        //    var jsonResponse = await response.Content.ReadAsStringAsync();

        //    var menu = JsonSerializer.Deserialize<object>(jsonResponse);

        //    return Json(menu);
        //}


        [HttpGet("Menu/Level/{level1}")]
        public async Task<IActionResult> Level(string level1)
        {

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



            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5251");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
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

        [HttpGet]
        public async Task<IActionResult> LoadMenu()
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (token == null)
                return Json(new { });

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("http://localhost:5251"); // 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/Menu/options");
            if (!response.IsSuccessStatusCode)
                return Json(new { });

            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            var menu = JsonSerializer.Deserialize<object>(jsonResponse);

            return Json(menu);
        }
    }
}

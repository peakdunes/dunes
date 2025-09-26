using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DUNES.UI.Controllers.Auth
{
    public class AuthController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IConfiguration _config;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;

        }

        [HttpGet]
        public IActionResult Login()
        {

           
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Login(string email, string password)
        {
          
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {

                ViewBag.Error = "Email and Password are required";
                return View();
            }
            try
            {
                var baseUrl = _config["ApiSettings:BaseUrl"];

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(baseUrl!); 

                var loginData = new
                {
                    username = email,
                    password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                var response = await client.PostAsync("/api/Auth/login", content);

                var apiResponseContent = await response.Content.ReadAsStringAsync();

               

                if (!response.IsSuccessStatusCode)
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid login. Please try again.", MessageDisplay.Inline);
                                       
                    return View();
                }

                // Leemos el token desde la respuesta de APIZEBRA
                var responseContent = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(responseContent);
                
                var token = doc.RootElement.GetProperty("data").GetProperty("token").GetString();

                var expiration = doc.RootElement.GetProperty("data").GetProperty("expiration").GetDateTime();

                var userName = doc.RootElement.GetProperty("data").GetProperty("userName").GetString();

                // Guardamos el token en Session
                HttpContext.Session.SetString("JWToken", token!);
                HttpContext.Session.SetString("UserName", userName!);

                // Redirigimos al Home (o a donde quieras)
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                MessageHelper.SetMessage(this, "danger", $"Error connecting to API: {ex.Message}", MessageDisplay.Inline);

                return View();
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("LAST_LEVEL1");
            HttpContext.Session.Remove("JWToken");
            return RedirectToAction("Index", "Home");
        }
    }
}
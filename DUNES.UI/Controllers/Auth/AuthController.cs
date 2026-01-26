//NUEVO

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DUNES.UI.Controllers.Auth
{
    public class AuthController : Controller
    {

        private readonly IAuthUIService _authService;

        public AuthController(IAuthUIService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login() => View();


        [HttpPost]

        public async Task<IActionResult> Login(string email, string password, CancellationToken ct)
        {


            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageHelper.SetMessage(
                    this,
                    "danger",
                    "Email and Password are required",
                    MessageDisplay.Inline
                );
                return View();
            }

            var resp = await _authService.LoginAsync(email, password, ct);

            if (!resp.Success || resp.Data == null)
            {
                MessageHelper.SetMessage(
                    this,
                    "danger",
                    resp.Error ?? "Login Error",
                    MessageDisplay.Inline
                );
                return View();
            }

            var login = resp.Data;

            var session = new SessionDTO
            {
                Token = login.Token,
                UserName = login.UserName,

                CompanyId = login.CompanyId,
                companyName = login.companyName,
                CompanyClientId = login.CompanyClientId,
                companyClientName = login.companyClientName,
                LocationId = login.LocationId,
                LocationName = login.LocationName,
                Expiration = login.Expiration,
                Environment = login.Enviromentname!,
                RoleName = login.RoleName!





            };

            HttpContext.Session.SetString(
                "UserSession",
                JsonSerializer.Serialize(session)
            );

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
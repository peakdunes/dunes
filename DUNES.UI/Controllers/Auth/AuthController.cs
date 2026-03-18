//NUEVO

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using DUNES.Shared.WiewModels.Auth;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> Login(string username, string password, CancellationToken ct)
        {


            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageHelper.SetMessage(
                    this,
                    "danger",
                    "Email and Password are required",
                    MessageDisplay.Inline
                );
                return View();
            }

            var resp = await _authService.LoginAsync(username, password, ct);

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

            if (!login.HasConfiguration)
            {
                MessageHelper.SetMessage(
                       this,
                       "warning",
                       "Your account was successfully authenticated, but your work environment has not been configured yet. Please contact your administrator before using the system.",
                       MessageDisplay.Inline
                   );
                return View("Login");
            }

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
                RoleName = login.RoleName!,
                companiesContractId = login.companiesContractId
            };

            HttpContext.Session.SetString(
                "UserSession",
                JsonSerializer.Serialize(session)
            );

            Response.Cookies.Append(
                "api_token",
                login.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Expires = login.Expiration
                }
            );


            if (login.MustChangePassword)
            {
                // Aquí va tu redirección al flujo de cambio de contraseña
                // Ejemplo:
                 return RedirectToAction("ChangePassword", "Auth");


            }
                   

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("api_token");
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Displays the change password view.
        /// </summary>
        /// <returns>Change password view.</returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordVM());


        }

        /// <summary>
        /// Processes the password change request.
        /// </summary>
        /// <param name="model">Change password view model.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to login or menu depending on result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new ChangePasswordDTO
            {
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword,
                ConfirmPassword = model.ConfirmPassword
            };

            var resp = await _authService.ChangePasswordAsync(dto, ct);

            if (!resp.Success)
            {
                MessageHelper.SetMessage(
                    this,
                    "danger",
                    resp.Message ?? resp.Error ?? "Unable to change password.",
                    MessageDisplay.Inline
                );

                return View(model);
            }

            MessageHelper.SetMessage(
                this,
                "success",
                "Your password was changed successfully.",
                MessageDisplay.Inline
            );

            HttpContext.Session.Clear();
            Response.Cookies.Delete("api_token");

            return RedirectToAction("Login", "Auth");

            
        }
    }
}

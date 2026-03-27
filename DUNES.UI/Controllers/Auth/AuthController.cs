using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using DUNES.Shared.WiewModels.Auth;
using DUNES.UI.Helpers;
using DUNES.UI.Models.Auth;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace DUNES.UI.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly IAuthUIService _authService;
        private readonly ICurrentUserPermissionUIService _currentUserPermissionUIService;
        private readonly IUserPermissionSessionHelper _userPermissionSessionHelper;

        public AuthController(
            IAuthUIService authService,
            ICurrentUserPermissionUIService currentUserPermissionUIService,
            IUserPermissionSessionHelper userPermissionSessionHelper)
        {
            _authService = authService;
            _currentUserPermissionUIService = currentUserPermissionUIService;
            _userPermissionSessionHelper = userPermissionSessionHelper;
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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, login.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, login.UserId ?? string.Empty)
            };

            if (!string.IsNullOrWhiteSpace(login.RoleName))
                claims.Add(new Claim(ClaimTypes.Role, login.RoleName));

            if (login.CompanyId > 0)
                claims.Add(new Claim("CompanyId", login.CompanyId.ToString()));

            if (login.CompanyClientId > 0)
                claims.Add(new Claim("CompanyClientId", login.CompanyClientId.ToString()));

            if (login.LocationId > 0)
                claims.Add(new Claim("LocationId", login.LocationId.ToString()));

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = login.Expiration
                }
            );

            var permissionResponse = await _currentUserPermissionUIService
                .GetMyPermissionsAsync(login.Token, ct);

            if (permissionResponse.Success && permissionResponse.Data is not null)
            {
                var permissionSession = new UserPermissionSessionDTO
                {
                    UserId = permissionResponse.Data.UserId,
                    Permissions = permissionResponse.Data.Permissions
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToHashSet(StringComparer.OrdinalIgnoreCase)
                };

                _userPermissionSessionHelper.Save(permissionSession);

                var savedPermissions = _userPermissionSessionHelper.Get();

            }
            else
            {
                _userPermissionSessionHelper.Clear();
            }

            if (login.MustChangePassword)
            {
                return RedirectToAction("ChangePassword", "Auth");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            _userPermissionSessionHelper.Clear();

            HttpContext.Session.Clear();

            Response.Cookies.Delete("api_token");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

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

            _userPermissionSessionHelper.Clear();
            HttpContext.Session.Clear();
            Response.Cookies.Delete("api_token");

            return RedirectToAction("Login", "Auth");
        }
    }
}
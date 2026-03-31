using DUNES.Shared.DTOs.Auth;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DUNES.UI.Controllers.Admin
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IMenuClientUIService _menuClientService;

        public HomeController(
            IMenuClientUIService menuClientService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _menuClientService = menuClientService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return await HandleAsync(async ct =>
            {
                var token = CurrentToken;

                if (string.IsNullOrWhiteSpace(token))
                    return RedirectToAction("Login", "Auth");

                var menuItems = await _menuClientService.GetMenuAsync(token, ct);

                return View(menuItems);
            }, ct);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
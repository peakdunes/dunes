using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.Admin
{
    public class MenuController : BaseController
    {
        private readonly IMenuClientUIService _menuClientService;

        public MenuController(IMenuClientUIService menuClientService)
        {
            _menuClientService = menuClientService;
        }

        [HttpGet("Menu/Level/{level1}")]
        public async Task<IActionResult> Level(string level1, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(level1))
            {
                MessageHelper.SetMessage(this, "warning", "Invalid menu level.", MessageDisplay.Inline);
                return RedirectToAction("Index", "Home");
            }

            // ✅ Token desde BaseController / SessionDTO
            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            // 🔹 Breadcrumb (ya usa servicio)
            var breadcrumb = await _menuClientService.GetBreadcrumbAsync(token, level1, ct);
            if (breadcrumb.Count > 0)
                breadcrumb[0].Url = Url.Action("Index", "Home");

            SetBreadcrumb(breadcrumb.ToArray());

            // 🔹 Obtener menú nivel 2 (idealmente esto debería moverse a un service)

            var listmenu = await _menuClientService.GetMenuLevel2Async(token, level1, ct);

            if (listmenu == null || listmenu.Count == 0)
            {
                MessageHelper.SetMessage(
                    this,
                    "info",
                    "No menu options are configured for this level.",
                    MessageDisplay.Inline
                );
            }

            return View(listmenu);
        }
    }
}

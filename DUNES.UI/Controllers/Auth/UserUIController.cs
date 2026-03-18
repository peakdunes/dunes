using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.Auth
{
    public class UserUIController : Controller
    {
        private readonly IUserUIService _userService;

        public UserUIController(IUserUIService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Displays the users list.
        /// </summary>
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var resp = await _userService.GetAllAsync(ct);

            if (!resp.Success || resp.Data == null)
            {
                MessageHelper.SetMessage(
                    this,
                    "danger",
                    resp.Message ?? "Unable to load users.",
                    MessageDisplay.Inline
                );

                return View(new List<UserReadDTO>());
            }

            return View(resp.Data);
        }
    }
}


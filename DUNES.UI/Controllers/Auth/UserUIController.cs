using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Options;

namespace DUNES.UI.Controllers.Auth
{
    public class UserUIController : BaseController
    {
        private readonly IUserUIService _userService;
        private readonly IMenuClientUIService _menuClientService;
        private readonly UserPhotoSettings _userPhotoSettings;
        private readonly IWebHostEnvironment _environment;

        private const string MENU_CODE_INDEX = "0301";
        private const string MENU_CODE_CRUD = "0301ZZ";

        public UserUIController(
              IUserUIService userService,
              IMenuClientUIService menuClientService,
              IOptions<UserPhotoSettings> userPhotoOptions,
              IWebHostEnvironment environment)
        {
            _userService = userService;
            _menuClientService = menuClientService;
            _userPhotoSettings = userPhotoOptions.Value;
            _environment = environment;
        }

        /// <summary>
        /// Displays the users list.
        /// </summary>
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _userService.GetAllAsync(CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<UserReadDTO>());
                }

                return View(result.Data ?? new List<UserReadDTO>());
            }, ct);
        }

        /// <summary>
        /// Displays the create user view.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Add New User", Url = null });

            await LoadRolesDropdownAsync(CurrentToken, ct);

            return View(new UserCreateDTO());

         
        }


        /// <summary>
        /// Creates a new user.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDTO model, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Users", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Create", Url = null });

            await LoadRolesDropdownAsync(CurrentToken, ct);

            return await HandleAsync(async ct =>
            {
                if (!ModelState.IsValid)
                {
                    MessageHelper.SetMessage(this, "danger", "Please review the required fields.", MessageDisplay.Inline);
                    return View(model);
                }

                var result = await _userService.CreateAsync(CurrentToken, model, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(model);
                }

                MessageHelper.SetMessage(this, "success", "User created successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        /// <summary>
        /// Displays the edit view for a specific user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
               
                new BreadcrumbItem { Text = "Edit User", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid user id.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                await LoadRolesDropdownAsync(CurrentToken, ct);

                var result = await _userService.GetByIdAsync(CurrentToken, id, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var model = new UserUpdateDTO
                {
                    Id = result.Data.Id,
                    UserName = result.Data.UserName,
                    Email = result.Data.Email,
                    FullName = result.Data.FullName,
                    RoleId = result.Data.RoleId ?? string.Empty,
                    IsActive = result.Data.IsActive
                };

                return View(model);
            }, ct);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO model, IFormFile? photo, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Users", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Edit", Url = null });

            return await HandleAsync(async ct =>
            {
                if (!ModelState.IsValid)
                {
                    await LoadRolesDropdownAsync(CurrentToken, ct);
                    MessageHelper.SetMessage(this, "danger", "Please review the required fields.", MessageDisplay.Inline);
                    return View(model);
                }

                var result = await _userService.UpdateAsync(CurrentToken, model, ct);

                if (!result.Success)
                {
                    await LoadRolesDropdownAsync(CurrentToken, ct);
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(model);
                }

                if (photo is not null && photo.Length > 0)
                {
                    var allowedContentTypes = new[] { "image/jpeg" };
                    var allowedExtensions = new[] { ".jpg", ".jpeg" };
                    var maxSizeBytes = 2 * 1024 * 1024; // 2 MB

                    var extension = Path.GetExtension(photo.FileName).ToLowerInvariant();

                    if (!allowedContentTypes.Contains(photo.ContentType) || !allowedExtensions.Contains(extension))
                    {
                        await LoadRolesDropdownAsync(CurrentToken, ct);
                        MessageHelper.SetMessage(this, "danger", "Invalid image format. Only JPG and JPEG files are allowed.", MessageDisplay.Inline);
                        return View(model);
                    }

                    if (photo.Length > maxSizeBytes)
                    {
                        await LoadRolesDropdownAsync(CurrentToken, ct);
                        MessageHelper.SetMessage(this, "danger", "The image file is too large. Maximum allowed size is 2 MB.", MessageDisplay.Inline);
                        return View(model);
                    }

                    var configuredBaseUrl = _userPhotoSettings.BaseUrl?.TrimStart('/', '\\') ?? "uploads/users";
                    var physicalFolder = Path.Combine(_environment.WebRootPath, configuredBaseUrl.Replace('/', Path.DirectorySeparatorChar));

                    if (!Directory.Exists(physicalFolder))
                        Directory.CreateDirectory(physicalFolder);

                    var targetFilePath = Path.Combine(physicalFolder, $"{model.Id}.jpg");

                    await using var stream = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await photo.CopyToAsync(stream, ct);
                }

                MessageHelper.SetMessage(this, "success", "User updated successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }
        /// <summary>
        /// Activates a user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Activate(string id, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid user id.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userService.ActivateAsync(CurrentToken, id, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                }
                else
                {
                    MessageHelper.SetMessage(this, "success", "User activated successfully.", MessageDisplay.Inline);
                }

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        /// <summary>
        /// Deactivates a user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Deactivate(string id, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid user id.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var result = await _userService.DeactivateAsync(CurrentToken, id, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                }
                else
                {
                    MessageHelper.SetMessage(this, "success", "User deactivated successfully.", MessageDisplay.Inline);
                }

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        /// <summary>
        /// Displays the reset password view for a specific user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Users", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Reset Password", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid user id.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var userResult = await _userService.GetByIdAsync(CurrentToken, id, ct);

                if (!userResult.Success || userResult.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", userResult.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var model = new ResetPasswordDTO
                {
                    UserId = userResult.Data.Id
                };

                ViewBag.UserName = userResult.Data.FullName;
                ViewBag.UserEmail = userResult.Data.Email;

                return View(model);
            }, ct);
        }

        /// <summary>
        /// Resets the password for a specific user.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Reset Password", Url = null });

            return await HandleAsync(async ct =>
            {
                async Task LoadUserInfoAsync()
                {
                    if (!string.IsNullOrWhiteSpace(model.UserId))
                    {
                        var userResult = await _userService.GetByIdAsync(CurrentToken, model.UserId, ct);

                        if (userResult.Success && userResult.Data is not null)
                        {
                            ViewBag.UserName = userResult.Data.FullName;
                            ViewBag.UserEmail = userResult.Data.Email;
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    await LoadUserInfoAsync();
                    MessageHelper.SetMessage(this, "danger", "Please review the password information.", MessageDisplay.Inline);
                    return View(model);
                }

                var result = await _userService.ResetPasswordAsync(CurrentToken, model, ct);

                if (!result.Success)
                {
                    await LoadUserInfoAsync();
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(model);
                }

                MessageHelper.SetMessage(this, "success", "Password reset successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }


        private async Task LoadRolesDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _userService.GetRolesAsync(token, ct); // o GetAll / GetAllActive

            ViewBag.Roles = master.Success && master.Data is not null
                     ? master.Data.Select(r => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                     {
                         Value = r.Id,
                         Text = r.Name
                     }).ToList()
                     : new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

          
        }
    }
}
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Models.Auth;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace DUNES.UI.Controllers.Auth
{
    [Authorize]
    public class UserUIController : BaseController
    {
        private readonly IUserUIService _userService;
        private readonly IMenuClientUIService _menuClientService;
        private readonly UserPhotoSettings _userPhotoSettings;
        private readonly IWebHostEnvironment _environment;

        private const string MENU_CODE_INDEX = "0301";
        private const string MENU_CODE_CRUD = "0301ZZ";

        private const string PERMISSION_ACCESS = "Auth.User.Access";
        private const string PERMISSION_CREATE = "Auth.User.Create";
        private const string PERMISSION_UPDATE = "Auth.User.Update";
        private const string PERMISSION_DELETE = "Auth.User.Delete";

        private const string SUPER_ADMIN_ROLE_NAME = "SuperAdmin";

        public UserUIController(
              IUserUIService userService,
              IMenuClientUIService menuClientService,
              IOptions<UserPhotoSettings> userPhotoOptions,
              IWebHostEnvironment environment,
              IUserPermissionSessionHelper permissionSessionHelper)
              : base(permissionSessionHelper)
        {
            _userService = userService;
            _menuClientService = menuClientService;
            _userPhotoSettings = userPhotoOptions.Value;
            _environment = environment;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_INDEX, _menuClientService, ct, CurrentToken, new BreadcrumbItem { Text = "", Url = null });

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

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "Add New User", Url = null });

            await LoadRolesDropdownAsync(CurrentToken, ct);

            return View(new UserCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDTO model, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
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

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, model.RoleId, ct))
                {
                    MessageHelper.SetMessage(this, "danger", "You are not allowed to assign the SuperAdmin role.", MessageDisplay.Inline);
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

        [HttpGet]
        public async Task<IActionResult> Edit(string id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "Edit User", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RedirectToAction(nameof(Index));

                var result = await _userService.GetByIdAsync(CurrentToken, id, ct);

                if (!result.Success || result.Data is null)
                    return RedirectToAction(nameof(Index));

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, result.Data.RoleId, ct))
                    return Forbid();

                await LoadRolesDropdownAsync(CurrentToken, ct);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateDTO model, IFormFile? photo, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "Edit", Url = null });

            return await HandleAsync(async ct =>
            {
                if (!ModelState.IsValid)
                {
                    await LoadRolesDropdownAsync(CurrentToken, ct);
                    return View(model);
                }

                var currentTargetResult = await _userService.GetByIdAsync(CurrentToken, model.Id, ct);
                if (!currentTargetResult.Success || currentTargetResult.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", currentTargetResult.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, currentTargetResult.Data.RoleId, ct))
                    return Forbid();

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, model.RoleId, ct))
                {
                    await LoadRolesDropdownAsync(CurrentToken, ct);
                    MessageHelper.SetMessage(this, "danger", "You are not allowed to assign the SuperAdmin role.", MessageDisplay.Inline);
                    return View(model);
                }

                var result = await _userService.UpdateAsync(CurrentToken, model, ct);

                if (!result.Success)
                {
                    await LoadRolesDropdownAsync(CurrentToken, ct);
                    return View(model);
                }

                MessageHelper.SetMessage(this, "success", "User updated successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Activate(string id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RedirectToAction(nameof(Index));

                var userResult = await _userService.GetByIdAsync(CurrentToken, id, ct);
                if (!userResult.Success || userResult.Data is null)
                    return RedirectToAction(nameof(Index));

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, userResult.Data.RoleId, ct))
                    return Forbid();

                var result = await _userService.ActivateAsync(CurrentToken, id, ct);

                if (!result.Success)
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                else
                    MessageHelper.SetMessage(this, "success", "User activated successfully.", MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Deactivate(string id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RedirectToAction(nameof(Index));

                var userResult = await _userService.GetByIdAsync(CurrentToken, id, ct);
                if (!userResult.Success || userResult.Data is null)
                    return RedirectToAction(nameof(Index));

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, userResult.Data.RoleId, ct))
                    return Forbid();

                var result = await _userService.DeactivateAsync(CurrentToken, id, ct);

                if (!result.Success)
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                else
                    MessageHelper.SetMessage(this, "success", "User deactivated successfully.", MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RedirectToAction(nameof(Index));

                var userResult = await _userService.GetByIdAsync(CurrentToken, id, ct);
                if (!userResult.Success || userResult.Data is null)
                    return RedirectToAction(nameof(Index));

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, userResult.Data.RoleId, ct))
                    return Forbid();

                var model = new ResetPasswordDTO { UserId = id };
                return View(model);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                if (model is null || string.IsNullOrWhiteSpace(model.UserId))
                    return RedirectToAction(nameof(Index));

                var userResult = await _userService.GetByIdAsync(CurrentToken, model.UserId, ct);
                if (!userResult.Success || userResult.Data is null)
                    return RedirectToAction(nameof(Index));

                if (!IsCurrentUserSuperAdmin() && await IsSuperAdminRoleAsync(CurrentToken, userResult.Data.RoleId, ct))
                    return Forbid();

                if (!ModelState.IsValid)
                    return View(model);

                var result = await _userService.ResetPasswordAsync(CurrentToken, model, ct);

                if (!result.Success)
                    return View(model);

                MessageHelper.SetMessage(this, "success", "Password reset successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        private async Task LoadRolesDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _userService.GetRolesAsync(token, ct);

            var roles = master.Success && master.Data is not null
                ? master.Data.ToList()
                : new List<RoleOptionDTO>();

            if (!IsCurrentUserSuperAdmin())
            {
                roles = roles
                    .Where(r => !string.Equals(r.Name, SUPER_ADMIN_ROLE_NAME, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            ViewBag.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            }).ToList();
        }

        private bool IsCurrentUserSuperAdmin()
        {
            return User.IsInRole(SUPER_ADMIN_ROLE_NAME);
        }

        private async Task<bool> IsSuperAdminRoleAsync(string token, string? roleId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(roleId))
                return false;

            var rolesResult = await _userService.GetRolesAsync(token, ct);

            if (!rolesResult.Success || rolesResult.Data is null)
                return false;

            return rolesResult.Data.Any(r =>
                string.Equals(r.Id, roleId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(r.Name, SUPER_ADMIN_ROLE_NAME, StringComparison.OrdinalIgnoreCase));
        }
    }
}
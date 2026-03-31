using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DUNES.UI.Controllers.Auth
{
    [Authorize]
    public class AuthPermissionUIController : BaseController
    {
        private readonly IAuthPermissionUIService _authPermissionService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "0302";
        private const string MENU_CODE_CRUD = "0302ZZ";

        private const string PERMISSION_ACCESS = "Auth.Permission.Access";
        private const string PERMISSION_CREATE = "Auth.Permission.Create";
        private const string PERMISSION_UPDATE = "Auth.Permission.Update";
        private const string PERMISSION_DELETE = "Auth.Permission.Delete";

        private const string SUPER_ADMIN_ROLE_NAME = "SuperAdmin";

        public AuthPermissionUIController(
            IAuthPermissionUIService authPermissionService,
            IMenuClientUIService menuClientService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _authPermissionService = authPermissionService;
            _menuClientService = menuClientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
           

            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

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
                var result = await _authPermissionService.GetAllAsync(CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<AuthPermissionReadDTO>());
                }

                return View(result.Data ?? new List<AuthPermissionReadDTO>());
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Add New Permission", Url = null });

            return await HandleAsync(async ct =>
            {
                var model = new AuthPermissionCreateDTO
                {
                    IsActive = true,
                    DisplayOrder = 1
                };

                return View(model);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthPermissionCreateDTO model, CancellationToken ct)
        {
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Create", Url = null });

            return await HandleAsync(async ct =>
            {
                NormalizeCreateModel(model);

                if (!ModelState.IsValid)
                {
                    MessageHelper.SetMessage(this, "danger", "Please review the required fields.", MessageDisplay.Inline);
                    return View(model);
                }

                var result = await _authPermissionService.CreateAsync(CurrentToken, model, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(model);
                }

                MessageHelper.SetMessage(this, "success", "Permission created successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        private static void NormalizeCreateModel(AuthPermissionCreateDTO model)
        {
            model.PermissionKey = model.PermissionKey?.Trim() ?? string.Empty;
            model.GroupName = model.GroupName?.Trim() ?? string.Empty;
            model.ModuleName = model.ModuleName?.Trim() ?? string.Empty;
            model.ActionName = model.ActionName?.Trim() ?? string.Empty;
            model.Description = string.IsNullOrWhiteSpace(model.Description)
                ? null
                : model.Description.Trim();

            if (model.DisplayOrder < 1)
                model.DisplayOrder = 1;
        }
    }
}
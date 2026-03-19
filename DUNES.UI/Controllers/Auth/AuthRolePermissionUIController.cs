using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.Auth
{
    /// <summary>
    /// MVC controller for role-permission assignments.
    /// </summary>
    public class AuthRolePermissionUIController : BaseController
    {
        private readonly IAuthRolePermissionUIService _rolePermissionService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "0303";
        private const string MENU_CODE_CRUD = "0303ZZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRolePermissionUIController"/> class.
        /// </summary>
        /// <param name="rolePermissionService">Role-permission UI service.</param>
        /// <param name="menuClientService">Menu client UI service.</param>
        public AuthRolePermissionUIController(
            IAuthRolePermissionUIService rolePermissionService,
            IMenuClientUIService menuClientService)
        {
            _rolePermissionService = rolePermissionService;
            _menuClientService = menuClientService;
        }

        /// <summary>
        /// Displays the role-permissions page.
        /// </summary>
        /// <param name="roleId">Selected role identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Role permissions view.</returns>
        [HttpGet]
        public async Task<IActionResult> Index(string? roleId, CancellationToken ct)
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
                var vm = new RolePermissionsPageVM
                {
                    RoleId = roleId ?? string.Empty
                };

                await LoadRolesAsync(vm, ct);

                if (!string.IsNullOrWhiteSpace(vm.RoleId))
                {
                    var result = await _rolePermissionService.GetByRoleAsync(CurrentToken, vm.RoleId, ct);

                    if (!result.Success)
                    {
                        MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                        return View(vm);
                    }

                    vm.Permissions = (result.Data ?? new List<RolePermissionItemDTO>())
                        .Select(x => new RolePermissionSelectionItemVM
                        {
                            PermissionId = x.PermissionId,
                            Group = x.Group,
                            Resource = x.Resource,
                            Action = x.Action,
                            Description = x.Description,
                            Assigned = x.Assigned
                        })
                        .ToList();
                }

                return View(vm);
            }, ct);
        }

        /// <summary>
        /// Saves the full permission set for the selected role.
        /// </summary>
        /// <param name="model">Role permissions page model.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects back to the selected role view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(RolePermissionsPageVM model, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Role Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Save", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(model.RoleId))
                {
                    await LoadRolesAsync(model, ct);
                    MessageHelper.SetMessage(this, "danger", "Please select a role.", MessageDisplay.Inline);
                    return View("Index", model);
                }

                var dto = new SaveRolePermissionsDTO
                {
                    RoleId = model.RoleId,
                    PermissionIds = model.Permissions
                        .Where(x => x.Assigned)
                        .Select(x => x.PermissionId)
                        .ToList()
                };

                var result = await _rolePermissionService.SaveByRoleAsync(CurrentToken, dto, ct);

                if (!result.Success)
                {
                    await LoadRolesAsync(model, ct);
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View("Index", model);
                }

                MessageHelper.SetMessage(this, "success", "Role permissions saved successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index), new { roleId = model.RoleId });
            }, ct);
        }

        /// <summary>
        /// Loads available roles into the page model.
        /// </summary>
        /// <param name="model">Page model.</param>
        /// <param name="ct">Cancellation token.</param>
        private async Task LoadRolesAsync(RolePermissionsPageVM model, CancellationToken ct)
        {
            var rolesResult = await _rolePermissionService.GetRolesAsync(CurrentToken!, ct);

            model.Roles = rolesResult.Success && rolesResult.Data is not null
                ? rolesResult.Data
                    .OrderBy(x => x.Name)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = x.Name
                    })
                    .ToList()
                : new List<SelectListItem>();
        }
    }
}
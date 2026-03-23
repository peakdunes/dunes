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

                    var allPermissions = result.Data ?? new List<RolePermissionItemDTO>();

                    vm.AssignedPermissions = allPermissions
                        .Where(x => x.Assigned)
                        .Select(MapToVm)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Resource)
                        .ThenBy(x => x.Action)
                        .ToList();

                    vm.AvailablePermissions = allPermissions
                        .Where(x => !x.Assigned)
                        .Select(MapToVm)
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Resource)
                        .ThenBy(x => x.Action)
                        .ToList();
                }

                return View(vm);
            }, ct);
        }

        /// <summary>
        /// Adds a single permission to the selected role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="permissionId">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects back to index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSelected(string roleId, int permissionId, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Role Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Add Permission", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(roleId) || permissionId <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid role or permission.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var currentResult = await _rolePermissionService.GetByRoleAsync(CurrentToken, roleId, ct);

                if (!currentResult.Success)
                {
                    MessageHelper.SetMessage(this, "danger", currentResult.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var finalPermissionIds = (currentResult.Data ?? new List<RolePermissionItemDTO>())
                    .Where(x => x.Assigned)
                    .Select(x => x.PermissionId)
                    .ToHashSet();

                finalPermissionIds.Add(permissionId);

                var dto = new SaveRolePermissionsDTO
                {
                    RoleId = roleId,
                    PermissionIds = finalPermissionIds.ToList()
                };

                var result = await _rolePermissionService.SaveByRoleAsync(CurrentToken, dto, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Success ? "Permission assigned successfully." : result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index), new { roleId });
            }, ct);
        }

        /// <summary>
        /// Adds multiple permissions to the selected role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="permissionIds">Permission identifiers.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects back to index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMultiple(string roleId, List<int>? permissionIds, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Role Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Add Multiple Permissions", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(roleId))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid role.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                permissionIds ??= new List<int>();

                if (!permissionIds.Any())
                {
                    MessageHelper.SetMessage(this, "warning", "Please select at least one permission.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var currentResult = await _rolePermissionService.GetByRoleAsync(CurrentToken, roleId, ct);

                if (!currentResult.Success)
                {
                    MessageHelper.SetMessage(this, "danger", currentResult.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var finalPermissionIds = (currentResult.Data ?? new List<RolePermissionItemDTO>())
                    .Where(x => x.Assigned)
                    .Select(x => x.PermissionId)
                    .ToHashSet();

                foreach (var permissionId in permissionIds.Where(x => x > 0))
                {
                    finalPermissionIds.Add(permissionId);
                }

                var dto = new SaveRolePermissionsDTO
                {
                    RoleId = roleId,
                    PermissionIds = finalPermissionIds.ToList()
                };

                var result = await _rolePermissionService.SaveByRoleAsync(CurrentToken, dto, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Success ? "Permissions assigned successfully." : result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index), new { roleId });
            }, ct);
        }

        /// <summary>
        /// Removes multiple permissions from the selected role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="permissionIds">Permission identifiers to remove.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects back to index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveMultiple(string roleId, List<int>? permissionIds, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Role Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Remove Multiple Permissions", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(roleId))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid role.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                permissionIds ??= new List<int>();

                if (!permissionIds.Any())
                {
                    MessageHelper.SetMessage(this, "warning", "Please select at least one permission to remove.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var currentResult = await _rolePermissionService.GetByRoleAsync(CurrentToken, roleId, ct);

                if (!currentResult.Success)
                {
                    MessageHelper.SetMessage(this, "danger", currentResult.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var idsToRemove = permissionIds.ToHashSet();

                var finalPermissionIds = (currentResult.Data ?? new List<RolePermissionItemDTO>())
                    .Where(x => x.Assigned)
                    .Select(x => x.PermissionId)
                    .Where(x => !idsToRemove.Contains(x))
                    .ToList();

                var dto = new SaveRolePermissionsDTO
                {
                    RoleId = roleId,
                    PermissionIds = finalPermissionIds
                };

                var result = await _rolePermissionService.SaveByRoleAsync(CurrentToken, dto, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Success ? "Selected permissions removed successfully." : result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index), new { roleId });
            }, ct);
        }


        /// <summary>
        /// Removes a permission from the selected role.
        /// </summary>
        /// <param name="roleId">Role identifier.</param>
        /// <param name="permissionId">Permission identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects back to index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(string roleId, int permissionId, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Role Permissions", Url = Url.Action(nameof(Index)) },
                new BreadcrumbItem { Text = "Remove Permission", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(roleId) || permissionId <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid role or permission.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var currentResult = await _rolePermissionService.GetByRoleAsync(CurrentToken, roleId, ct);

                if (!currentResult.Success)
                {
                    MessageHelper.SetMessage(this, "danger", currentResult.Message, MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index), new { roleId });
                }

                var finalPermissionIds = (currentResult.Data ?? new List<RolePermissionItemDTO>())
                    .Where(x => x.Assigned)
                    .Select(x => x.PermissionId)
                    .Where(x => x != permissionId)
                    .ToList();

                var dto = new SaveRolePermissionsDTO
                {
                    RoleId = roleId,
                    PermissionIds = finalPermissionIds
                };

                var result = await _rolePermissionService.SaveByRoleAsync(CurrentToken, dto, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Success ? "Permission removed successfully." : result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index), new { roleId });
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

        /// <summary>
        /// Maps a role permission DTO item into the page VM item.
        /// </summary>
        /// <param name="dto">Source dto item.</param>
        /// <returns>Mapped view model item.</returns>
        private static RolePermissionItemVM MapToVm(RolePermissionItemDTO dto)
        {
            return new RolePermissionItemVM
            {
                PermissionId = dto.PermissionId,
                Group = dto.Group,
                Resource = dto.Resource,
                Action = dto.Action,
                Description = dto.Description
            };
        }
    }
}
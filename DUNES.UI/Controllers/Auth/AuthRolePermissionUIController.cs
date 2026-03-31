using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Models.Auth;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.Auth
{
    /// <summary>
    /// MVC controller for role-permission assignments.
    /// </summary>
    [Authorize]
    public class AuthRolePermissionUIController : BaseController
    {
        private readonly IAuthRolePermissionUIService _rolePermissionService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "0303";
        private const string MENU_CODE_CRUD = "0303ZZ";

        private const string PERMISSION_ACCESS = "Auth.RolePermission.Access";
        private const string PERMISSION_UPDATE = "Auth.RolePermission.Update";
        private const string SUPER_ADMIN_ROLE_NAME = "SuperAdmin";

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRolePermissionUIController"/> class.
        /// </summary>
        /// <param name="rolePermissionService">Role-permission UI service.</param>
        /// <param name="menuClientService">Menu client UI service.</param>
        /// <param name="permissionSessionHelper">Permission session helper.</param>
        public AuthRolePermissionUIController(
            IAuthRolePermissionUIService rolePermissionService,
            IMenuClientUIService menuClientService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
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
                var vm = new RolePermissionsMatrixPageVM
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

                    var allPermissions = (result.Data ?? new List<RolePermissionItemDTO>())
                        .Select(x => new RolePermissionItemDTO
                        {
                            PermissionId = x.PermissionId,
                            PermissionKey = x.PermissionKey?.Trim() ?? string.Empty,
                            Group = x.Group?.Trim() ?? string.Empty,
                            Resource = x.Resource?.Trim() ?? string.Empty,
                            Action = x.Action?.Trim() ?? string.Empty,
                            Description = x.Description?.Trim(),
                            Assigned = x.Assigned,
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder
                        })
                        .ToList();

                    vm.Actions = allPermissions
                        .Where(x => !string.IsNullOrWhiteSpace(x.Action))
                        .GroupBy(x => x.Action, StringComparer.OrdinalIgnoreCase)
                        .OrderBy(g => g.Min(x => x.DisplayOrder))
                        .ThenBy(g => g.Key)
                        .Select(g => g.Key)
                        .ToList();

                    vm.Rows = allPermissions
                        .GroupBy(x => new
                        {
                            Group = x.Group,
                            Resource = x.Resource
                        })
                        .Select(g => new RolePermissionsMatrixRowVM
                        {
                            Group = g.Key.Group,
                            Module = g.Key.Resource,
                            Cells = vm.Actions.ToDictionary(
                                action => action,
                                action =>
                                {
                                    var permission = g.FirstOrDefault(x =>
                                        string.Equals(x.Action, action, StringComparison.OrdinalIgnoreCase));

                                    return new RolePermissionsMatrixCellVM
                                    {
                                        PermissionId = permission?.PermissionId ?? 0,
                                        Exists = permission != null,
                                        Assigned = permission?.Assigned ?? false
                                    };
                                },
                                StringComparer.OrdinalIgnoreCase)
                        })
                        .OrderBy(x => x.Group)
                        .ThenBy(x => x.Module)
                        .ToList();
                }

                return View(vm);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMatrix(SaveRolePermissionsMatrixVM model, CancellationToken ct)
        {
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Role Permissions", Url = Url.Action(nameof(Index), new { roleId = model.RoleId }) },
                new BreadcrumbItem { Text = "Save Matrix", Url = null });

            return await HandleAsync(async ct =>
            {
                if (string.IsNullOrWhiteSpace(model.RoleId))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid role.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var dto = new SaveRolePermissionsDTO
                {
                    RoleId = model.RoleId,
                    PermissionIds = model.PermissionIds?.Distinct().ToList() ?? new List<int>()
                };

                var result = await _rolePermissionService.SaveByRoleAsync(CurrentToken, dto, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Success ? "Role permissions updated successfully." : result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index), new { roleId = model.RoleId });
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
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

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
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

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
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

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
            if (!User.IsInRole(SUPER_ADMIN_ROLE_NAME) || !_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

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

        private async Task LoadRolesAsync(RolePermissionsMatrixPageVM model, CancellationToken ct)
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
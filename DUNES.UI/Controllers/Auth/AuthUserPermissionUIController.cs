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
    [Authorize]
    public class AuthUserPermissionUIController : BaseController
    {
        private readonly IAuthUserPermissionUIService _userPermissionService;
        private readonly IUserUIService _userService;
        private readonly IMenuClientUIService _menuClientService;

       

        private const string MENU_CODE_INDEX = "0304";
        private const string MENU_CODE_CRUD = "0304ZZ";

        private const string PERMISSION_ACCESS = "Auth.UserPermission.Access";
        private const string PERMISSION_UPDATE = "Auth.UserPermission.Update";

        private const string SUPER_ADMIN_ROLE_NAME = "SuperAdmin";

        public AuthUserPermissionUIController(
            IAuthUserPermissionUIService userPermissionService,
            IUserUIService userService,
            IMenuClientUIService menuClientService,
              IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _userPermissionService = userPermissionService;
            _userService = userService;
            _menuClientService = menuClientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? userId, CancellationToken ct)
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
                var vm = new UserPermissionsMatrixPageVM
                {
                    UserId = userId ?? string.Empty
                };

                await LoadUsersAsync(vm, ct);

                if (!string.IsNullOrWhiteSpace(vm.UserId))
                {
                    var result = await _userPermissionService.GetByUserAsync(CurrentToken, vm.UserId, ct);

                    if (!result.Success || result.Data is null)
                    {
                        MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                        return View(vm);
                    }

                    var bundle = result.Data;

                    var inheritedPermissions = bundle.InheritedPermissions
                        .Select(x => new RolePermissionItemDTO
                        {
                            PermissionId = x.PermissionId,
                            PermissionKey = x.PermissionKey?.Trim() ?? string.Empty,
                            Group = x.Group?.Trim() ?? string.Empty,
                            Resource = x.Resource?.Trim() ?? string.Empty,
                            Action = x.Action?.Trim() ?? string.Empty,
                            Description = x.Description?.Trim(),
                            Assigned = true,
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder
                        })
                        .ToList();

                    var directPermissions = bundle.DirectPermissions
                        .Select(x => new RolePermissionItemDTO
                        {
                            PermissionId = x.PermissionId,
                            PermissionKey = x.PermissionKey?.Trim() ?? string.Empty,
                            Group = x.Group?.Trim() ?? string.Empty,
                            Resource = x.Resource?.Trim() ?? string.Empty,
                            Action = x.Action?.Trim() ?? string.Empty,
                            Description = x.Description?.Trim(),
                            Assigned = true,
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder
                        })
                        .ToList();

                    var effectivePermissions = bundle.EffectivePermissions
                        .Select(x => new RolePermissionItemDTO
                        {
                            PermissionId = x.PermissionId,
                            PermissionKey = x.PermissionKey?.Trim() ?? string.Empty,
                            Group = x.Group?.Trim() ?? string.Empty,
                            Resource = x.Resource?.Trim() ?? string.Empty,
                            Action = x.Action?.Trim() ?? string.Empty,
                            Description = x.Description?.Trim(),
                            Assigned = true,
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder
                        })
                        .ToList();

                    var availableDirectPermissions = bundle.AvailableDirectPermissions
                        .Select(x => new RolePermissionItemDTO
                        {
                            PermissionId = x.PermissionId,
                            PermissionKey = x.PermissionKey?.Trim() ?? string.Empty,
                            Group = x.Group?.Trim() ?? string.Empty,
                            Resource = x.Resource?.Trim() ?? string.Empty,
                            Action = x.Action?.Trim() ?? string.Empty,
                            Description = x.Description?.Trim(),
                            Assigned = false,
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder
                        })
                        .ToList();

                    var directIds = directPermissions
                        .Select(x => x.PermissionId)
                        .ToHashSet();

                    var editableDirectPermissions = availableDirectPermissions
                        .Select(x => new RolePermissionItemDTO
                        {
                            PermissionId = x.PermissionId,
                            PermissionKey = x.PermissionKey,
                            Group = x.Group,
                            Resource = x.Resource,
                            Action = x.Action,
                            Description = x.Description,
                            Assigned = directIds.Contains(x.PermissionId),
                            IsActive = x.IsActive,
                            DisplayOrder = x.DisplayOrder
                        })
                        .ToList();

                    var allPermissionsForActions = inheritedPermissions
                        .Concat(availableDirectPermissions)
                        .Concat(effectivePermissions)
                        .GroupBy(x => x.PermissionId)
                        .Select(g => g.First())
                        .ToList();

                    vm.Actions = allPermissionsForActions
                        .Where(x => !string.IsNullOrWhiteSpace(x.Action))
                        .GroupBy(x => x.Action, StringComparer.OrdinalIgnoreCase)
                        .OrderBy(g => g.Min(x => x.DisplayOrder))
                        .ThenBy(g => g.Key)
                        .Select(g => g.Key)
                        .ToList();

                    vm.InheritedRows = BuildMatrix(inheritedPermissions, vm.Actions);
                    vm.DirectRows = BuildMatrix(editableDirectPermissions, vm.Actions);
                    vm.EffectiveRows = BuildMatrix(effectivePermissions, vm.Actions);
                }

                return View(vm);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDirect(SaveUserPermissionsMatrixVM model, CancellationToken ct)
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
                new BreadcrumbItem { Text = "User Permissions", Url = Url.Action(nameof(Index), new { userId = model.UserId }) },
                new BreadcrumbItem { Text = "Save Direct Permissions", Url = null });

            return await HandleAsync(async ct =>
            {
                if (model is null || string.IsNullOrWhiteSpace(model.UserId))
                {
                    MessageHelper.SetMessage(this, "danger", "Please select a user.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var userResult = await _userService.GetByIdAsync(CurrentToken, model.UserId, ct);
                if (!userResult.Success || userResult.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", userResult.Message ?? "User not found.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var dto = new SaveUserPermissionsDTO
                {
                    UserId = model.UserId,
                    PermissionIds = model.PermissionIds?.Where(x => x > 0).Distinct().ToList() ?? new List<int>()
                };

                var result = await _userPermissionService.SaveByUserAsync(CurrentToken, dto, ct);

                MessageHelper.SetMessage(
                    this,
                    result.Success ? "success" : "danger",
                    result.Success ? "Direct user permissions updated successfully." : result.Message,
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index), new { userId = model.UserId });
            }, ct);
        }

        private async Task LoadUsersAsync(UserPermissionsMatrixPageVM model, CancellationToken ct)
        {
            var result = await _userService.GetAllAsync(CurrentToken!, ct);

            model.Users = result.Success && result.Data is not null
                ? result.Data
                    .OrderBy(x => x.FullName)
                    .ThenBy(x => x.Email)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id,
                        Text = string.IsNullOrWhiteSpace(x.FullName)
                            ? x.Email
                            : $"{x.FullName} ({x.Email})"
                    })
                    .ToList()
                : new List<SelectListItem>();
        }

        private static List<PermissionMatrixRowVM> BuildMatrix(
            List<RolePermissionItemDTO> source,
            List<string> actions)
        {
            var normalized = source
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

            return normalized
                .GroupBy(x => new { x.Group, x.Resource })
                .Select(g => new PermissionMatrixRowVM
                {
                    Group = g.Key.Group,
                    Module = g.Key.Resource,
                    Cells = actions.ToDictionary(
                        action => action,
                        action =>
                        {
                            var permission = g.FirstOrDefault(x =>
                                string.Equals(x.Action, action, StringComparison.OrdinalIgnoreCase));

                            return new PermissionMatrixCellVM
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
    }
}
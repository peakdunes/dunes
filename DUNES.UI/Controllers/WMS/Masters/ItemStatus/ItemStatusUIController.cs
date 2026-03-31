using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.ItemStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.ItemStatus
{
    [Authorize]
    public class ItemStatusUIController : BaseController
    {
        private readonly IItemStatusWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;
        

        private const string MENU_CODE_INDEX = "01020810";
        private const string MENU_CODE_CRUD = "01020810ZZ";

        private const string PERMISSION_ACCESS = "Masters.ItemStatus.Access";
        private const string PERMISSION_CREATE = "Masters.ItemStatus.Create";
        private const string PERMISSION_UPDATE = "Masters.ItemStatus.Update";
        private const string PERMISSION_DELETE = "Masters.ItemStatus.Delete";

        public ItemStatusUIController(
            IItemStatusWMSUIService service,
            IMenuClientUIService menuClientService,
            IAuthPermissionUIService authPermissionUIService,
             IUserPermissionSessionHelper permissionSessionHelper) : base(permissionSessionHelper, authPermissionUIService)
        {
            _service = service;
            _menuClientService = menuClientService;
          
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });

            await LoadCrudActionsAsync("Masters", "ItemStatus", ct);


            return await HandleAsync(async ct =>
            {
                var result = await _service.GetAllAsync(CurrentToken!, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message ?? "Error loading item status.", MessageDisplay.Inline);
                    return View(new List<WMSItemStatusReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Add New Status", Url = null });

            return View(new WMSItemStatusCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSItemStatusCreateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var result = await _service.CreateAsync(dto, CurrentToken!, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message ?? "Error creating item status.", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", result.Message ?? "Item status created.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Edit Item Status", Url = null });

            return await HandleAsync(async ct =>
            {
                var res = await _service.GetByIdAsync(id, CurrentToken!, ct);

                if (!res.Success || res.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", res.Message ?? "Item status not found.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var model = new WMSItemStatusUpdateDTO
                {
                    Id = res.Data.Id,
                    Name = res.Data.Name,
                    Observations = res.Data.Observations,
                    Active = res.Data.Active
                };

                return View(model);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSItemStatusUpdateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _service.UpdateAsync(id, dto, CurrentToken!, ct);

                if (!res.Success)
                {
                    MessageHelper.SetMessage(this, "danger", res.Message ?? "Error updating item status.", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", res.Message ?? "Item status updated.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int id, bool isActive, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _service.SetActiveAsync(id, isActive, CurrentToken!, ct);

                MessageHelper.SetMessage(
                    this,
                    res.Success ? "success" : "danger",
                    res.Message ?? (res.Success ? "Updated." : "Error updating."),
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Delete Item Status", Url = null });

            return await HandleAsync(async ct =>
            {
                var res = await _service.GetByIdAsync(id, CurrentToken!, ct);

                if (!res.Success || res.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", res.Message ?? "Item status not found.", MessageDisplay.Inline);
                    return RedirectToAction(nameof(Index));
                }

                var model = new WMSItemStatusReadDTO
                {
                    Id = res.Data.Id,
                    Name = res.Data.Name,
                    Observations = res.Data.Observations,
                    Active = res.Data.Active,
                    Idcompany = res.Data.Idcompany,
                    CompanyName = res.Data.CompanyName,
                };

                return View(model);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSItemStatusReadDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _service.DeleteByIdAsync(id, CurrentToken!, ct);

                MessageHelper.SetMessage(
                    this,
                    res.Success ? "success" : "danger",
                    res.Message ?? (res.Success ? "Deleted." : "Error deleting."),
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }
    }
}
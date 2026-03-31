using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.CompanyClientItemStatus;
using DUNES.UI.Services.WMS.Masters.ItemStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompanyClientItemStatus
{
    [Authorize]
    public class CompanyClientItemStatusUIController : BaseController
    {
        private readonly ICompanyClientItemStatusWMSUIService _service;
        private readonly IItemStatusWMSUIService _itemStatusService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020306";
        private const string MENU_CODE_CRUD = "01020306ZZ";

        private const string PERMISSION_ACCESS = "Masters.CompanyClientItemStatus.Access";
        private const string PERMISSION_CREATE = "Masters.CompanyClientItemStatus.Create";
        private const string PERMISSION_UPDATE = "Masters.CompanyClientItemStatus.Update";
        private const string PERMISSION_DELETE = "Masters.CompanyClientItemStatus.Delete";

        public CompanyClientItemStatusUIController(
            ICompanyClientItemStatusWMSUIService service,
            IMenuClientUIService menuClientService,
            IItemStatusWMSUIService itemStatusService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _service = service;
            _menuClientService = menuClientService;
            _itemStatusService = itemStatusService;
        }

        [HttpGet]
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

            await LoadCrudActionsAsync("Masters", "CompanyClientItemStatus", ct);

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetAllAsync(CurrentToken, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSCompanyClientItemStatusReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

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
                new BreadcrumbItem { Text = "Edit Item Status Mapping", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetByIdAsync(CurrentToken, id, ct);

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new WMSCompanyClientItemStatusReadDTO());
                }

                return View(result.Data);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSCompanyClientItemStatusSetActiveDTO collection, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_UPDATE))
                return Forbid();

            try
            {
                if (CurrentToken is null)
                    return RedirectToLogin();

                await SetMenuBreadcrumbAsync(
                    MENU_CODE_CRUD,
                    _menuClientService,
                    ct,
                    CurrentToken,
                    new BreadcrumbItem { Text = "Edit Item Status Mapping", Url = null });

                return await HandleAsync(async ct =>
                {
                    var result = await _service.SetActiveAsync(CurrentToken, id, collection, ct);

                    if (!result.Data)
                    {
                        MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    }
                    else
                    {
                        MessageHelper.SetMessage(this, "success",
                            collection.IsActive ? "Mapping activated." : "Mapping deactivated.",
                            MessageDisplay.Inline);
                    }

                    return RedirectToAction(nameof(Index));
                }, ct);
            }
            catch
            {
                return View();
            }
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
                new BreadcrumbItem { Text = "New Item Status Mapping", Url = null });

            await LoadTypesDropdownAsync(CurrentToken, ct);

            return View(new WMSCompanyClientItemStatusCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompanyClientItemStatusCreateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadTypesDropdownAsync(CurrentToken, ct);
                return View(dto);
            }

            var result = await _service.CreateAsync(CurrentToken, dto, ct);

            if (!result.Success)
            {
                MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                await LoadTypesDropdownAsync(CurrentToken, ct);
                return View(dto);
            }

            MessageHelper.SetMessage(this, "success", result.Message, MessageDisplay.Inline);
            return RedirectToAction(nameof(Index));
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
             new BreadcrumbItem { Text = "Delete Item Status Mapping", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _service.GetByIdAsync(CurrentToken, id, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Toast);
                    return RedirectToAction(nameof(Index));
                }

                return View(result.Data);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSCompanyClientItemStatusUpdateDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            var res = await _service.DeleteAsync(CurrentToken, id, ct);

            MessageHelper.SetMessage(
                   this,
                   res.Success ? "success" : "danger",
                   res.Message ?? (res.Success ? "Deleted." : "Error deleting."),
                   MessageDisplay.Inline);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadTypesDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _itemStatusService.GetActiveAsync(token, ct);

            var mapped = await _service.GetEnabledAsync(token, ct);
            var mappedIds = (mapped.Data ?? new List<WMSCompanyClientItemStatusReadDTO>())
                .Select(x => x.ItemStatusId)
                .ToHashSet();

            var available = master.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.ItemStatuses = new SelectList(available, "Id", "Name");
        }
    }
}
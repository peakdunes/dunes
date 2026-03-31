using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.CompanyClientInventoryCategory;
using DUNES.UI.Services.WMS.Masters.InventoryCategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompanyClientInventoryCategory
{
    [Authorize]
    public class CompanyClientInventoryCategoryUIController : BaseController
    {
        private readonly ICompanyClientInventoryCategoryWMSUIService _inventoryCategoriesService;
        private readonly IInventoryCategoriesWMSUIService _masterCategoriesService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020304";
        private const string MENU_CODE_CRUD = "01020304ZZ";

        private const string PERMISSION_ACCESS = "Masters.CompanyClientInventoryCategory.Access";
        private const string PERMISSION_CREATE = "Masters.CompanyClientInventoryCategory.Create";
        private const string PERMISSION_UPDATE = "Masters.CompanyClientInventoryCategory.Update";
        private const string PERMISSION_DELETE = "Masters.CompanyClientInventoryCategory.Delete";

        public CompanyClientInventoryCategoryUIController(
            ICompanyClientInventoryCategoryWMSUIService inventoryCategoriesService,
            IInventoryCategoriesWMSUIService masterCategoriesService,
            IMenuClientUIService menuClientService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _inventoryCategoriesService = inventoryCategoriesService;
            _menuClientService = menuClientService;
            _masterCategoriesService = masterCategoriesService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_ACCESS);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });

            await LoadCrudActionsAsync("Masters", "CompanyClientInventoryCategory", ct);

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryCategoriesService.GetAllAsync(CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSCompanyClientInventoryCategoryReadDTO>());
                }

                return View(result.Data ?? new List<WMSCompanyClientInventoryCategoryReadDTO>());
            }, ct);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_CREATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "New Inventory Category Mapping", Url = null });

            await LoadCategoryDropdownAsync(CurrentToken, ct);

            return View(new WMSCompanyClientInventoryCategoryCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompanyClientInventoryCategoryCreateDTO dto, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_CREATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            if (!ModelState.IsValid)
            {
                await LoadCategoryDropdownAsync(CurrentToken, ct);
                return View(dto);
            }

            return await HandleAsync(async ct =>
            {
                var created = await _inventoryCategoriesService.CreateAsync(dto, CurrentToken, ct);

                if (created.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", created.Message, MessageDisplay.Inline);
                    await LoadCategoryDropdownAsync(CurrentToken!, ct);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", "Mapping created successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_UPDATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Edit Inventory Category Mapping", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryCategoriesService.GetByIdAsync(id, CurrentToken, ct);

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new WMSCompanyClientInventoryCategoryReadDTO());
                }

                return View(result.Data);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSCompanyClientInventoryCategoryReadDTO collection, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_UPDATE);
            if (deny is not null)
                return deny;

            try
            {
                if (CurrentToken is null)
                    return RedirectToLogin();

                await SetMenuBreadcrumbAsync(
                    MENU_CODE_CRUD,
                    _menuClientService,
                    ct,
                    CurrentToken,
                    new BreadcrumbItem { Text = "Edit Inventory Category Mapping", Url = null });

                return await HandleAsync(async ct =>
                {
                    var result = await _inventoryCategoriesService.SetActiveAsync(id, collection.IsActive, CurrentToken, ct);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int id, bool isActive, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_UPDATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryCategoriesService.SetActiveAsync(id, isActive, CurrentToken, ct);

                if (!result.Data)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                }
                else
                {
                    MessageHelper.SetMessage(this, "success",
                        isActive ? "Mapping activated." : "Mapping deactivated.",
                        MessageDisplay.Inline);
                }

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_DELETE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "Delete Inventory Category Mapping", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryCategoriesService.GetByIdAsync(id, CurrentToken, ct);

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new WMSCompanyClientInventoryCategoryReadDTO());
                }

                return View(result.Data);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSCompanyClientInventoryCategoryReadDTO dto, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_DELETE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _inventoryCategoriesService.DeleteAsync(id, CurrentToken!, ct);

                MessageHelper.SetMessage(
                    this,
                    res.Success ? "success" : "danger",
                    res.Message ?? (res.Success ? "Deleted." : "Error deleting."),
                    MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        private async Task LoadCategoryDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _inventoryCategoriesService.GetEnabledAsync(token, ct);
            var masterActive = await _masterCategoriesService.GetAllAsync(token, ct);

            var mapped = await _inventoryCategoriesService.GetEnabledAsync(token, ct);
            var mappedIds = (mapped.Data ?? new List<WMSCompanyClientInventoryCategoryReadDTO>())
                .Select(x => x.InventoryCategoryId)
                .ToHashSet();

            var available = masterActive.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.InventoryCategorySelectList = new SelectList(available, "Id", "Name");
        }
    }
}
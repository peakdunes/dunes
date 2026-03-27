using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.CompanyClientInventoryType;
using DUNES.UI.Services.WMS.Masters.InventoryCategories;
using DUNES.UI.Services.WMS.Masters.InventoryTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompanyClientInventoryTypes
{
    [Authorize]
    public class CompanyClientInventoryTypesUIController : BaseController
    {
        private readonly ICompanyClientInventoryTypeWMSUIService _inventoryTypeService;
        private readonly IInventoryTypesWMSUIService _masterInventoryTypeService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020305";
        private const string MENU_CODE_CRUD = "01020305ZZ";

        private const string PERMISSION_ACCESS = "Masters.CompanyClientInventoryCategory.Access";
        private const string PERMISSION_CREATE = "Masters.CompanyClientInventoryCategory.Create";
        private const string PERMISSION_UPDATE = "Masters.CompanyClientInventoryCategory.Update";
        private const string PERMISSION_DELETE = "Masters.CompanyClientInventoryCategory.Delete";

        public CompanyClientInventoryTypesUIController(
            ICompanyClientInventoryTypeWMSUIService inventoryTypeService,
            IInventoryTypesWMSUIService masterInventoryTypeService,
            IMenuClientUIService menuClientService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper)
        {
            _inventoryTypeService = inventoryTypeService;
            _menuClientService = menuClientService;
            _masterInventoryTypeService = masterInventoryTypeService;
        }

        /// <summary>
        /// Lists enabled inventory category mappings for the current client (scoped by token).
        /// </summary>
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

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryTypeService.GetAllAsync(CurrentToken, ct);

                if (!result.Success)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSCompanyClientInventoryTypeReadDTO>());
                }

                return View(result.Data ?? new List<WMSCompanyClientInventoryTypeReadDTO>());
            }, ct);
        }

        /// <summary>
        /// Form for creating a new mapping (enabling a category for this client).
        /// </summary>
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
                new BreadcrumbItem { Text = "New Inventory Type Mapping", Url = null });

            await LoadTypesDropdownAsync(CurrentToken, ct);

            return View(new WMSCompanyClientInventoryTypeCreateDTO());
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
                new BreadcrumbItem { Text = "Edit Inventory Type Mapping", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryTypeService.GetByIdAsync(id, CurrentToken, ct);

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new WMSCompanyClientInventoryTypeReadDTO());
                }

                return View(result.Data);
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSCompanyClientInventoryTypeReadDTO collection, CancellationToken ct)
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
                    new BreadcrumbItem { Text = "Edit Inventory Type Mapping", Url = null });

                return await HandleAsync(async ct =>
                {
                    var result = await _inventoryTypeService.SetActiveAsync(id, collection.IsActive, CurrentToken, ct);

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

        /// <summary>
        /// Creates a new mapping (scoped by token).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompanyClientInventoryTypeCreateDTO dto, CancellationToken ct)
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

            return await HandleAsync(async ct =>
            {
                var created = await _inventoryTypeService.CreateAsync(dto, CurrentToken, ct);

                if (created.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", created.Message, MessageDisplay.Inline);
                    await LoadTypesDropdownAsync(CurrentToken!, ct);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", "Mapping created successfully.", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        /// <summary>
        /// Enables or disables a mapping by mapping Id (scoped by token).
        /// </summary>
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
                var result = await _inventoryTypeService.SetActiveAsync(id, isActive, CurrentToken, ct);

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

        /// <summary>
        /// Loads the dropdown list of master inventory types available to enable:
        /// - only master Active=true
        /// - excludes types already enabled for this client
        /// </summary>
        private async Task LoadTypesDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _inventoryTypeService.GetEnabledAsync(token, ct);
            var masterActive = await _masterInventoryTypeService.GetAllAsync(token, ct);

            var mapped = await _inventoryTypeService.GetEnabledAsync(token, ct);
            var mappedIds = (mapped.Data ?? new List<WMSCompanyClientInventoryTypeReadDTO>())
                .Select(x => x.InventoryTypeId)
                .ToHashSet();

            var available = masterActive.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.InventoryTypesSelectList = new SelectList(available, "Id", "Name");
        }

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
                new BreadcrumbItem { Text = "Delete Inventory Type Mapping", Url = null });

            return await HandleAsync(async ct =>
            {
                var result = await _inventoryTypeService.GetByIdAsync(id, CurrentToken, ct);

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new WMSCompanyClientInventoryCategoryReadDTO());
                }

                return View(result.Data);
            }, ct);
        }

        /// <summary>
        /// Hard delete (Option B): only if not used.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSCompanyClientInventoryTypeReadDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_DELETE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var res = await _inventoryTypeService.DeleteAsync(id, CurrentToken!, ct);

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
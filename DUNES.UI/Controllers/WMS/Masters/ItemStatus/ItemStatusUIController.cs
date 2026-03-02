using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.ItemStatus;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.ItemStatus
{
    /// <summary>
    /// UI Controller for Item Status (WMS / Masters).
    /// One controller per model (non-generic) by design.
    /// </summary>
    public class ItemStatusUIController : BaseController
    {
        private readonly IItemStatusWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020810";
        private const string MENU_CODE_CRUD = "01020810ZZ";

        public ItemStatusUIController(
            IItemStatusWMSUIService service,
            IMenuClientUIService menuClientService)
        {
            _service = service;
            _menuClientService = menuClientService;
        }

        /// <summary>
        /// Item Status list.
        /// </summary>
        public async Task<IActionResult> Index(CancellationToken ct)
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
                var result = await _service.GetAllAsync(CurrentToken!, ct);

                if (!result.Success || result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message ?? "Error loading item status.", MessageDisplay.Inline);
                    return View(new List<WMSItemStatusReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

        /// <summary>
        /// Create page.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
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

        /// <summary>
        /// Create action.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSItemStatusCreateDTO dto, CancellationToken ct)
        {
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

        /// <summary>
        /// Edit page.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
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

        /// <summary>
        /// Edit action.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WMSItemStatusUpdateDTO dto, CancellationToken ct)
        {
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

        /// <summary>
        /// Toggle active status.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActive(int id, bool isActive, CancellationToken ct)
        {
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

        /// <summary>
        /// Hard delete (Option B): only if not used.
        /// Requires API/UI support for Delete. If not implemented yet, API will return 404.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
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

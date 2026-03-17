using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.CompanyClientItemStatus;
using DUNES.UI.Services.WMS.Masters.ItemStatus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompanyClientItemStatus
{
    public class CompanyClientItemStatusUIController : BaseController
    {
        private readonly ICompanyClientItemStatusWMSUIService _service;
        private readonly IItemStatusWMSUIService _itemStatusService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020306";
        private const string MENU_CODE_CRUD = "01020306ZZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSUIController"/> class.
        /// </summary>
        /// <param name="service">UI service for client Item Status mappings.</param>
        public CompanyClientItemStatusUIController(
            ICompanyClientItemStatusWMSUIService service,
            IMenuClientUIService menuClientService,
            IItemStatusWMSUIService itemStatusService)
        {
            _service = service;
            _menuClientService = menuClientService;
            _itemStatusService = itemStatusService;
        }

        /// <summary>
        /// Displays the list of enabled Item Status mappings for the current client.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Index view with the client Item Status mappings.</returns>
        [HttpGet]
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

        // POST: DepotController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //  public async Task <IActionResult> Edit(int id, IFormCollection collection, CancellationToken ct)
        public async Task<IActionResult> Edit(int id, WMSCompanyClientItemStatusSetActiveDTO collection, CancellationToken ct)
        {
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


        /// <summary>
        /// Form for creating a new mapping (enabling a category for this client).
        /// </summary>
        /// 
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
                new BreadcrumbItem { Text = "New Item Status Mapping", Url = null });

            // ✅ Load combo data
            await LoadTypesDropdownAsync(CurrentToken, ct);

            return View(new WMSCompanyClientItemStatusCreateDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompanyClientItemStatusCreateDTO dto, CancellationToken ct)
        {
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


        /// <summary>
        /// Displays the delete confirmation view for a specific client Item Status mapping.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Delete confirmation view.</returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
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

        /// <summary>
        /// Confirms and executes the deletion of a client Item Status mapping.
        /// This deletes only the client relation, not the master Item Status.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Redirects to Index after delete attempt.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, WMSCompanyClientItemStatusUpdateDTO dto, CancellationToken ct)
        {
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

        /// <summary>
        /// Loads the dropdown list of master categories available to enable:
        /// - only master Active=true
        /// - excludes categories already enabled for this client
        /// </summary>
        private async Task LoadTypesDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _itemStatusService.GetActiveAsync(token, ct); // o GetAll / GetAllActive
           

            // 2) Existing enabled mappings (enough to prevent duplicates)
            var mapped = await _service.GetEnabledAsync(token, ct);
            var mappedIds = (mapped.Data ?? new List<WMSCompanyClientItemStatusReadDTO>())
                .Select(x => x.ItemStatusId)
                .ToHashSet();

            // 3) Available to enable
            var available = master.Data
                .Where(x => !mappedIds.Contains(x.Id) && x.Active)
                .ToList();

            ViewBag.ItemStatuses = new SelectList(available, "Id", "Name");
        }

    }
}


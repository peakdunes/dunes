using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.CompanyClientInventoryCategory;
using DUNES.UI.Services.WMS.Masters.InventoryCategories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompanyClientInventoryCategory
{
    public class CompanyClientInventoryCategoryUIController : BaseController
    {
      
        private readonly ICompanyClientInventoryCategoryWMSUIService _inventoryCategoriesService;
        private readonly IInventoryCategoriesWMSUIService _masterCategoriesService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020304";
        private const string MENU_CODE_CRUD = "01020304ZZ";

        public CompanyClientInventoryCategoryUIController(
            ICompanyClientInventoryCategoryWMSUIService inventoryCategoriesService,
            IInventoryCategoriesWMSUIService masterCategoriesService,
            IMenuClientUIService menuClientService)
        {
            _inventoryCategoriesService = inventoryCategoriesService;
            _menuClientService = menuClientService;
            _masterCategoriesService = masterCategoriesService;
        }

        /// <summary>
        /// Lists enabled inventory category mappings for the current client (scoped by token).
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
                var result = await _inventoryCategoriesService.GetAllAsync(CurrentToken, ct);

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSCompanyClientInventoryCategoryReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }

        /// <summary>
        /// Form for creating a new mapping (enabling a category for this client).
        /// </summary>
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "New Inventory Category Mapping", Url = null });

            // ✅ Load combo data
            await LoadCategoryDropdownAsync(CurrentToken, ct);

            return View(new WMSCompanyClientInventoryCategoryCreateDTO());
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

        // POST: DepotController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
      //  public async Task <IActionResult> Edit(int id, IFormCollection collection, CancellationToken ct)
             public async Task<IActionResult> Edit(int id, WMSCompanyClientInventoryCategoryReadDTO collection, CancellationToken ct)
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
                    new BreadcrumbItem { Text = "Edit Inventory Category Mapping", Url = null });

                return await HandleAsync(async ct =>
                {
                    var result = await _inventoryCategoriesService.SetActiveAsync(id,  collection.IsActive, CurrentToken, ct);

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
        public async Task<IActionResult> Create(WMSCompanyClientInventoryCategoryCreateDTO dto, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            // ✅ If ModelState fails, reload combo and return view
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

                    // ✅ reload combo again because we’re returning the View
                    await LoadCategoryDropdownAsync(CurrentToken!, ct);
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

        /// <summary>
        /// Loads the dropdown list of master categories available to enable:
        /// - only master Active=true
        /// - excludes categories already enabled for this client
        /// </summary>
        private async Task LoadCategoryDropdownAsync(string token, CancellationToken ct)
        {
            var master = await _inventoryCategoriesService.GetEnabledAsync(token, ct); // o GetAll / GetAllActive
            var masterActive = await _masterCategoriesService.GetActiveAsync(token, ct);
                

            // 2) Existing enabled mappings (enough to prevent duplicates)
            var mapped = await _inventoryCategoriesService.GetEnabledAsync(token, ct);
            var mappedIds = (mapped.Data ?? new List<WMSCompanyClientInventoryCategoryReadDTO>())
                .Select(x => x.InventoryCategoryId)
                .ToHashSet();

            // 3) Available to enable
            var available = masterActive.Data
                .Where(x => !mappedIds.Contains(x.Id))
                .ToList();

            ViewBag.InventoryCategorySelectList = new SelectList(available, "Id", "Name");
        }
    }
}

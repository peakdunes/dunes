using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.InventoryCategories;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.InventoryCategories
{
    public class InventoryCategoriesUIController : BaseController
    {

        private readonly IInventoryCategoriesWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020809";
        private const string MENU_CODE_CRUD = "01020809ZZ";

        public InventoryCategoriesUIController(IInventoryCategoriesWMSUIService service,
            IMenuClientUIService menuClientService)
        {
            _service = service;
            _menuClientService = menuClientService;
        }


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

                if (result.Data is null)
                {
                    MessageHelper.SetMessage(this, "danger", result.Message, MessageDisplay.Inline);
                    return View(new List<WMSCompanyClientInventoryCategoryReadDTO>());
                }

                return View(result.Data);
            }, ct);
        }
    }
}

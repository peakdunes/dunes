using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Countries;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Countries
{
    public class CountriesController : BaseController
    {
        private readonly ICountriesWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020801";
        private const string MENU_CODE_CRUD = "01020801ZZ";

        public CountriesController(
            ICountriesWMSUIService service,
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
                new BreadcrumbItem
                {
                    Text = "", // actual
                    Url = null
                });

            return await HandleAsync(async ct =>
            {
                var listcountries = await _service.GetAllCountriesInformation(CurrentToken, ct);
                return View(listcountries.Data);
            }, ct);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem
                {
                    Text = "New Country",
                    Url = null
                });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCountriesDTO dto, CancellationToken ct)
        {
            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem
                {
                    Text = "New Country",
                    Url = null
                });

            return await HandleAsync(async ct =>
            {
                var exists = await _service.GetCountryInformationByIdentificationAsync(
                    dto.Name!.Trim(),
                    null,
                    CurrentToken,
                    ct);

                if (exists.Data)
                {
                    MessageHelper.SetMessage(this, "danger", "Country Name already exists", MessageDisplay.Inline);
                    return View(dto);
                }

                var result = await _service.AddCountryAsync(dto, CurrentToken, ct);

                if (!result.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating country: {result.Message}", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", "Country created successfully", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }
    }
}

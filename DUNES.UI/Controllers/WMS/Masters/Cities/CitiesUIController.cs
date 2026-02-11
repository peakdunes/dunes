using DUNES.Shared.DTOs.WMS;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Cities;
using DUNES.UI.Services.WMS.Masters.Countries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.Cities
{
    public class CitiesUIController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICitiesWMSUIService _service;
        private readonly ICountriesWMSUIService _countryService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020803";
        private const string MENU_CODE_CRUD = "01020803ZZ";

        public CitiesUIController(
            IHttpClientFactory httpClientFactory,
            ICitiesWMSUIService service,
            IMenuClientUIService menuClientService,
            ICountriesWMSUIService countryService)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
            _menuClientService = menuClientService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(int? countryId, CancellationToken ct)
        {
            var token = CurrentToken;
            if (token == null)
                return RedirectToLogin();

            var session = CurrentToken;
            if (session == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                token,
                new BreadcrumbItem
                {
                    Text = "", // actual
                    Url = null
                });

            if (countryId == null)
            {
                countryId = 0;
            }

            return await HandleAsync(async ct =>
            {
                await LoadInfoAsync(token, ct, 0);
                var listcities = await _service.GetAllCitiesInformation(countryId.Value,token,ct);

                return View(listcities.Data ?? new List<WMSCitiesReadDTO>());
            }, ct);
        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {
            var listcountries = await _countryService.GetAllCountriesInformation(token, ct);
            var data = listcountries.Data ?? new List<WMSCountriesDTO>();
            var listactives = data.Where(x => x.Active == true).ToList();

            ViewBag.listcountries = idselected == 0
                ? new SelectList(listactives, "Id", "Name")
                : new SelectList(listactives, "Id", "Name", idselected);
        }

        // Métodos Create/Edit/Delete pueden ser implementados igual si aplican
    }
}

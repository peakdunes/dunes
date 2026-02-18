using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Cities;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.Companies;
using DUNES.UI.Services.WMS.Masters.Countries;
using DUNES.UI.Services.WMS.Masters.StatesCountries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.ClientCompanies
{
    public class ClientCompaniesUIController : BaseController
    {
        private readonly ICompaniesWMSUIService _companyservice;
        private readonly IClientCompaniesWMSUIService _service;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICountriesWMSUIService _countryService;
        private readonly ICitiesWMSUIService _cityService;
        private readonly IStatesCountriesWMSUIService _statesservice;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020301";
        private const string MENU_CODE_CRUD = "01020301ZZ";

        public ClientCompaniesUIController(
            IHttpClientFactory httpClientFactory,
            IClientCompaniesWMSUIService service,
            ICompaniesWMSUIService companyservice,
            IMenuClientUIService menuClientService,
            ICountriesWMSUIService countryService,
            IStatesCountriesWMSUIService statesservice,
            ICitiesWMSUIService cityService)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
            _companyservice = companyservice;
            _menuClientService = menuClientService;
            _countryService = countryService;
            _statesservice = statesservice;
            _cityService = cityService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            if (CurrentToken == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_INDEX, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });

            return await HandleAsync(async ct =>
            {
                var listcompanies = await _service.GetAllClientCompaniesInformation(CurrentToken, ct);
                return View(listcompanies.Data);
            }, ct);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (CurrentToken == null)
                return RedirectToLogin();

            await LoadInfoAsync(CurrentToken, ct, 0);

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "New Company Client", Url = null });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WmsCompanyclientDto dto, CancellationToken ct)
        {
            if (CurrentToken == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "New Company Client", Url = null });

            return await HandleAsync(async ct =>
            {
                var infocompany = await _service.GetClientCompanyInformationByIdentificationAsync(dto.CompanyId!, CurrentToken, ct);
                if (infocompany.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", infocompany.Message.Trim(), MessageDisplay.Inline);
                    await LoadInfoAsync(CurrentToken, ct, dto.Idcountry);
                    return View(dto);
                }

                var infocompanyname = await _service.GetClientCompanyInformationByNameAsync(dto.Name!, CurrentToken, ct);
                if (infocompanyname.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", infocompany.Message.Trim(), MessageDisplay.Inline);
                    await LoadInfoAsync(CurrentToken, ct, dto.Idcountry);
                    return View(dto);
                }

                var createrecord = await _service.AddClientCompanyAsync(dto, CurrentToken, ct);
                if (!createrecord.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating this Company Client Error:{createrecord.Message}", MessageDisplay.Inline);
                    await LoadInfoAsync(CurrentToken, ct, dto.Idcountry);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", $"Company Client created", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatesByCountry(int countryId, CancellationToken ct)
        {
            if (CurrentToken == null)
                return Unauthorized();

            var response = await _statesservice.GetAllStatesCountryInformation(countryId, CurrentToken, ct);
            var list = response.Data ?? new List<WMSStatesCountriesReadDTO>();

            var data = list.Where(x => x.Active)
                .Select(x => new { id = x.Id, name = x.Name })
                .ToList();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesByState(int stateId, int countryId, CancellationToken ct)
        {
            if (CurrentToken == null)
                return Unauthorized();

            var response = await _cityService.GetAllCitiesInformation(countryId, CurrentToken, ct);
            var list = response.Data ?? new List<WMSCitiesReadDTO>();

            var data = list.Where(x => x.Active && x.Idstate == stateId)
                .Select(x => new { id = x.Id, name = x.Name })
                .ToList();

            return Json(data);
        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {
            var listcountries = await _countryService.GetAllCountriesInformation(token, ct);
            var data = listcountries.Data ?? new List<WMSCountriesDTO>();
            var listactives = data.Where(x => x.Active).ToList();

            ViewBag.listcountries = idselected == 0
                ? new SelectList(listactives, "Id", "Name")
                : new SelectList(listactives, "Id", "Name", idselected);

            var listcompanies = await _companyservice.GetAllCompaniesInformation(token, ct);
            var datacompanies = listcompanies.Data ?? new List<WMSCompaniesDTO>();
            var listCompaniesActives = datacompanies.Where(x => x.Active).ToList();

            ViewBag.listcompanies = new SelectList(listCompaniesActives, "Id", "Name");
        }
    }
}

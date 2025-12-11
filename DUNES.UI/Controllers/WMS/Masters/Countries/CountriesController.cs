using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;

using DUNES.UI.Services.WMS.Masters.Countries;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;

namespace DUNES.UI.Controllers.WMS.Masters.Countries
{
    public class CountriesController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICountriesWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        public readonly IConfiguration _config;
        public readonly int _companyDefault;

        private const string MENU_CODE_INDEX = "01020801";
        private const string MENU_CODE_CRUD = "01020801ZZ";

        public CountriesController(IHttpClientFactory httpClientFactory, IConfiguration config,
            ICountriesWMSUIService service, IMenuClientUIService menuClientService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;

        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var token = GetToken();


            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
            MENU_CODE_INDEX,
           _menuClientService, ct, token,
           new BreadcrumbItem
           {
               Text = "",   // actual
               Url = null
           });

            return await HandleAsync(async ct =>
            {
                var listcountries = await _service.GetAllCountriesInformation(token, ct);

                return View(listcountries.Data);

            }, ct);
        }


        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
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
            
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();


            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
            new BreadcrumbItem
            {
                Text = "New Country",
                Url = null
            });

            return await HandleAsync(async ct =>
            {
                var infocountry = await _service.GetCountryInformationByIdentificationAsync(dto.Name!.Trim(),null, token, ct);

                if (infocountry.Data)
                {
                    MessageHelper.SetMessage(this, "danger", "Country Name already exist", MessageDisplay.Inline);
                    return View(dto);
                }

                var createrecord = await _service.AddCountryAsync(dto,token,ct);

                if (!createrecord.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating this country Error:{createrecord.Message}", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", $"Country created", MessageDisplay.Inline);


                return RedirectToAction(nameof(Index));
            }, ct);


        }
    }
}

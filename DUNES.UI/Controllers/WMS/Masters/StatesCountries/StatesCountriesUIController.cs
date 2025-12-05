using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Countries;
using DUNES.UI.Services.WMS.Masters.StatesCountries;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.StatesCountries
{
    public class StatesCountriesUIController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IStatesCountriesWMSUIService _service;
        private readonly ICountriesWMSUIService _countryService;
        private readonly IMenuClientUIService _menuClientService;

        public readonly IConfiguration _config;
        public readonly int _companyDefault;

        private const string MENU_CODE_INDEX = "01020802";
        private const string MENU_CODE_CRUD = "01020802ZZ";

        public StatesCountriesUIController(IHttpClientFactory httpClientFactory, IConfiguration config,
            IStatesCountriesWMSUIService service, IMenuClientUIService menuClientService,
            ICountriesWMSUIService countryService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;
            _countryService = countryService;
        }

        // GET: StatesCountriesUIController
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
                var listcountries = await _service.GetAllStatesCountryInformation(token, ct);

                return View(listcountries.Data);

            }, ct);
        }

        // GET: StatesCountriesUIController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatesCountriesUIController/Create
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
                new BreadcrumbItem
                {
                    Text = "New State",
                    Url = null
                });

           var listcountries = await  _countryService.GetAllCountriesInformation(token, ct);

            if (listcountries.Data == null || listcountries.Data.Count <= 0)
            {
                MessageHelper.SetMessage(this, "danger", "Country Name already exist", MessageDisplay.Inline);
                return View();
            }

            var listactives = listcountries.Data.Where(x => x.Active == true).ToList();

            ViewBag.listcountries = new SelectList(listactives,"Id","Name");

            return View();
        }

        // POST: StatesCountriesUIController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatesCountriesUIController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatesCountriesUIController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatesCountriesUIController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatesCountriesUIController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Cities;
using DUNES.UI.Services.WMS.Masters.Countries;
using DUNES.UI.Services.WMS.Masters.StatesCountries;
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

        public readonly IConfiguration _config;
        public readonly int _companyDefault;

        private const string MENU_CODE_INDEX = "01020803";
        private const string MENU_CODE_CRUD = "01020803ZZ";

        public CitiesUIController(IHttpClientFactory httpClientFactory, IConfiguration config,
            ICitiesWMSUIService service, IMenuClientUIService menuClientService,
            ICountriesWMSUIService countryService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;
            _countryService = countryService;
        }

        // GET: CitiesUIController
        public async Task<IActionResult> Index(int? countryId, CancellationToken ct)
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

            if (countryId == null)
            {
                countryId = 0;
            }

            return await HandleAsync(async ct =>
            {
                await LoadInfoAsync(token, ct, 0);

                var listcities = await _service.GetAllCitiesInformation(Convert.ToInt32(countryId), token, ct);

                return View(listcities.Data ?? new List<WMSCitiesReadDTO>());

            }, ct);
        }

        // GET: CitiesUIController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CitiesUIController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CitiesUIController/Create
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

        // GET: CitiesUIController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CitiesUIController/Edit/5
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

        // GET: CitiesUIController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CitiesUIController/Delete/5
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

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {

            var listcountries = await _countryService.GetAllCountriesInformation(token, ct);

            var data = listcountries.Data ?? new List<WMSCountriesDTO>();

            var listactives = data.Where(x => x.Active == true).ToList();

            if (idselected == 0)
            {
                ViewBag.listcountries = new SelectList(listactives, "Id", "Name");
            }
            else
            {
                ViewBag.listcountries = new SelectList(listactives, "Id", "Name", idselected);
            }

        }
    }
}

using DUNES.Shared.DTOs.WMS;
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
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NuGet.Common;
using System.Xml;

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
        public async Task<IActionResult> Index(int? countryId, CancellationToken ct)
        {
            var token = CurrentToken;


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

                var liststates = await _service.GetAllStatesCountryInformation(Convert.ToInt32(countryId), token, ct);

                return View(liststates.Data ?? new List<WMSStatesCountriesReadDTO>());

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
            var token = CurrentToken;

            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
                new BreadcrumbItem
                {
                    Text = "New State",
                    Url = null
                });

            await LoadInfoAsync(token, ct, 0);
            
            return View();
        }

        // POST: StatesCountriesUIController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(WMSStatesCountriesDTO dto, CancellationToken ct)
        {


            var token = CurrentToken;

            if (token == null)
                return RedirectToLogin();


            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
            new BreadcrumbItem
            {
                Text = "New State",
                Url = null
            });

            return await HandleAsync(async ct =>
                {
                    var infostate = await _service.GetStateCountryInformationByIdentificationAsync(dto.Idcountry, dto.Name!.Trim(), token, ct);

                    if (infostate.Data != null)
                    {
                        MessageHelper.SetMessage(this, "danger", infostate.Message.Trim(), MessageDisplay.Inline);

                        await LoadInfoAsync(token, ct, dto.Idcountry);
                        
                        return View(dto);
                    }

                    var infostateiso = await _service.GetStateCountryInformationByISOCodeAsync(dto.Idcountry, dto.Sigla!.Trim(), token, ct);

                    if (infostateiso.Data != null)
                    {
                        MessageHelper.SetMessage(this, "danger", infostateiso.Message.Trim(), MessageDisplay.Inline);

                        await LoadInfoAsync(token, ct, dto.Idcountry);

                        return View(dto);
                    }

                    var createrecord = await _service.AddStateCountryAsync(dto, token, ct);

                    if (!createrecord.Data)
                    {
                        MessageHelper.SetMessage(this, "danger", $"Error creating this country Error:{createrecord.Message}", MessageDisplay.Inline);

                        await LoadInfoAsync(token, ct, dto.Idcountry);

                        return View(dto);
                    }

                    MessageHelper.SetMessage(this, "success", $"Country created", MessageDisplay.Inline);


                    return RedirectToAction(nameof(Index));
                }, ct);





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

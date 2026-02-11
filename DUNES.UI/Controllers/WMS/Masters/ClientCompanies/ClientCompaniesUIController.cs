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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public readonly IConfiguration _config;
        public readonly int _companyDefault;
        private readonly IMenuClientUIService _menuClientService;
      

        private const string MENU_CODE_INDEX = "01020301";
        private const string MENU_CODE_CRUD = "01020301ZZ";


        public ClientCompaniesUIController(IHttpClientFactory httpClientFactory, IConfiguration config,
            IClientCompaniesWMSUIService service, ICompaniesWMSUIService companyservice,
            IMenuClientUIService menuClientService, ICountriesWMSUIService countryService,
            IStatesCountriesWMSUIService statesservice, ICitiesWMSUIService cityService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _companyservice = companyservice;
            _menuClientService = menuClientService;
            _countryService = countryService;
            _statesservice = statesservice;
            _cityService = cityService;


        }

        // GET: ClientCompaniesUIController
        public async Task<IActionResult> Index(CancellationToken ct)
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


            return await HandleAsync(async ct =>
            {

                var listcompanies = await _service.GetAllClientCompaniesInformation(token, ct);

                return View(listcompanies.Data);

            }, ct);



        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var token = CurrentToken;

            if (token == null)
                return RedirectToLogin();

            await LoadInfoAsync(token, ct, 0);

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
                new BreadcrumbItem
                {
                    Text = "New Company Client",
                    Url = null
                });

            var listcompanies = await _companyservice.GetAllCompaniesInformation(token, ct);

            if(listcompanies.Data == null || listcompanies.Data.Count == 0)
            {

            }
            var listactive = listcompanies.Data.Select(x => x.Active == true).ToList();

            ViewData["companies"] = new SelectList(listactive,"Id","Name");
          

            return View();
        }

        

        // POST: StatesCountriesUIController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(WmsCompanyclientDto dto, CancellationToken ct)
        {


            var token = CurrentToken;

            if (token == null)
                return RedirectToLogin();


            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
            new BreadcrumbItem
            {
                Text = "New Company Client",
                Url = null
            });

            return await HandleAsync(async ct =>
            {
                var infocompany = await _service.GetClientCompanyInformationByIdentificationAsync(dto.CompanyId!, token, ct);

                if (infocompany.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", infocompany.Message.Trim(), MessageDisplay.Inline);

                    await LoadInfoAsync(token, ct, dto.Idcountry);

                    return View(dto);
                }

                var infocompanyname = await _service.GetClientCompanyInformationByNameAsync(dto.Name!, token, ct);

                if (infocompanyname.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", infocompany.Message.Trim(), MessageDisplay.Inline);

                    await LoadInfoAsync(token, ct, dto.Idcountry);

                    return View(dto);
                }

                var createrecord = await _service.AddClientCompanyAsync(dto, token, ct);

                if (!createrecord.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating this Company Client Error:{createrecord.Message}", MessageDisplay.Inline);

                    await LoadInfoAsync(token, ct, dto.Idcountry);

                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", $"Company Client created", MessageDisplay.Inline);


                return RedirectToAction(nameof(Index));
            }, ct);





        }
















        [HttpGet]
        public async Task<IActionResult> GetStatesByCountry(int countryId, CancellationToken ct)
        {
            var token = CurrentToken;
            if (token == null)
                return Unauthorized();

            var response = await _statesservice.GetAllStatesCountryInformation(countryId, token, ct);

            var list = response.Data ?? new List<WMSStatesCountriesReadDTO>();

            var data = list.Where(x => x.Active == true)
               .Select(x => new
               {
                   id = x.Id,
                   name = x.Name
               })
               .ToList();   

            return Json(data);

         
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesByState(int stateId, int countryId, CancellationToken ct)
        {
            var token = CurrentToken;
            if (token == null)
                return Unauthorized();

            var response = await _cityService.GetAllCitiesInformation(countryId, token, ct);
            // Ajusta el nombre del método según lo tengas

            var list = response.Data ?? new List<WMSCitiesReadDTO>();

            var data = list.Where(x => x.Active == true && x.Idstate == stateId)
               .Select(x => new
               {
                   id = x.Id,
                   name = x.Name
               })
               .ToList();

            return Json(data);

            
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


            var listcompanies = await _companyservice.GetAllCompaniesInformation(token, ct);

            var datacompanies = listcompanies.Data ?? new List<WMSCompaniesDTO>();

            var listCompaniesActives = datacompanies.Where(x => x.Active == true).ToList();


            ViewBag.listcompanies = new SelectList(listCompaniesActives, "Id", "Name");
        }
    }
}

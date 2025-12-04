using DUNES.UI.Models;
using DUNES.UI.Services.WMS.Common;
using DUNES.UI.Services.WMS.Masters.Countries;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Countries
{
    public class CountriesController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICountriesWMSUIService _service;

        public readonly IConfiguration _config;
        public readonly int _companyDefault;



        public CountriesController(IHttpClientFactory httpClientFactory, IConfiguration config,
            ICountriesWMSUIService service)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue("companyDefault", 1);


        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var token = GetToken();


            if (token == null)
                return RedirectToLogin();


            SetBreadcrumb(
          new BreadcrumbItem { Text = "Home", Url = Url.Action("Index", "Home") },
          new BreadcrumbItem { Text = "Configuration", Url = Url.Action("Index", "ConfigurationUI") },
          new BreadcrumbItem { Text = "WMS", Url = Url.Action("Index", "WMSUI") },
          new BreadcrumbItem { Text = "General Configuration", Url = Url.Action("Index", "WMSGeneralConfigUI") },
          new BreadcrumbItem { Text = "Countries List", Url = null } // ← ACTUAL (amarillo)
      );


            return await HandleAsync(async ct =>
            {


                var listcountries = await _service.GetAllCountriesInformation(token, ct);

                return View(listcountries.Data);

            }, ct);



        }
    }
}

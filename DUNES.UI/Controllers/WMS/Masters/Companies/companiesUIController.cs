using DUNES.UI.Services.Inventory.Common;
using DUNES.UI.Services.Inventory.PickProcess;
using DUNES.UI.Services.Print;
using DUNES.UI.Services.WMS.Common;
using DUNES.UI.Services.WMS.Masters.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Companies
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class companiesUIController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICompaniesWMSUIService _service;
       
        public readonly IConfiguration _config;
        public readonly int _companyDefault;
              


        public companiesUIController(IHttpClientFactory httpClientFactory, IConfiguration config,
            ICompaniesWMSUIService service)
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

            return await HandleAsync(async ct =>
            {

             
                var listcompanies = await _service.GetAllCompaniesInformation(token, ct);

                return View(listcompanies.Data);

            }, ct);


          
        }

    }
}

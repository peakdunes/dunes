using DUNES.Shared.DTOs.WMS;
using DUNES.UI.Services.WMS.Common;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
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

        private readonly ICommonWMSUIService _companyservice;
        private readonly IClientCompaniesWMSUIService _service;
        private readonly IHttpClientFactory _httpClientFactory;
        public readonly IConfiguration _config;
        public readonly int _companyDefault;



        public ClientCompaniesUIController(IHttpClientFactory httpClientFactory, IConfiguration config,
            IClientCompaniesWMSUIService service, ICommonWMSUIService companyservice)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _companyservice = companyservice;


        }

        // GET: ClientCompaniesUIController
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {

                var listcompanies = await _service.GetAllClientCompaniesInformation(token, ct);

                return View(listcompanies.Data);

            }, ct);



        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();
            var listcompanies = await _companyservice.GetAllCompaniesInformation(token, ct);

            if(listcompanies.Data == null || listcompanies.Data.Count == 0)
            {

            }
            var listactive = listcompanies.Data.Select(x => x.Active == true).ToList();

            ViewData["companies"] = new SelectList(listactive,"Id","Name");
          

            return View();
        }

    }
}

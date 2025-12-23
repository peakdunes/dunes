using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.CompaniesClientDivision;
using DUNES.UI.Services.WMS.Masters.CompaniesContract;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.CompaniesContract
{
    public class CompaniesClientContractUIController : Controller
    {

        public readonly ICompaniesClientContractWMSUIService _service;
              
        private readonly IMenuClientUIService _menuClientService;
        
        public readonly IConfiguration _config;
        public readonly int _companyDefault;
        private const string MENU_CODE_INDEX = "01020302";
        private const string MENU_CODE_CRUD = "01020302ZZ";


        public CompaniesClientContractUIController(ICompaniesClientContractWMSUIService service, IConfiguration config,
            IMenuClientUIService menuClientService, IClientCompaniesWMSUIService companyClientService)
        {
            _service = service;
            _config = config;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;
           
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

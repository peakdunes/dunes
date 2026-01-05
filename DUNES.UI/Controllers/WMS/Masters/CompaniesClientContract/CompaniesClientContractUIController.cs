using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.Companies;
using DUNES.UI.Services.WMS.Masters.CompaniesClientDivision;
using DUNES.UI.Services.WMS.Masters.CompaniesContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Common;

namespace DUNES.UI.Controllers.WMS.Masters.CompaniesContract
{
    public class CompaniesClientContractUIController : BaseController
    {

        public readonly ICompaniesClientContractWMSUIService _service;
        private readonly IClientCompaniesWMSUIService _companyClientService;
        private readonly ICompaniesWMSUIService _companyService;

        private readonly IMenuClientUIService _menuClientService;

        public readonly IConfiguration _config;
        public readonly int _companyDefault;
        private const string MENU_CODE_INDEX = "01020303";
        private const string MENU_CODE_CRUD = "01020303ZZ";


        public CompaniesClientContractUIController(ICompaniesClientContractWMSUIService service, IConfiguration config,
            IMenuClientUIService menuClientService, IClientCompaniesWMSUIService companyClientService,
            ICompaniesWMSUIService companyService)
        {
            _service = service;
            _config = config;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;
            _companyClientService = companyClientService;
            _companyService = companyService;

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


            var contractList = await _service.GetAllClientCompaniesContractInformationAsync(token, ct);


            return View(contractList.Data);
        }


        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();

            await LoadInfoAsync(token, ct, 0);

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
             new BreadcrumbItem
             {
                 Text = "New Contract",
                 Url = null
             });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(WMSCompaniesContractDTO dto, CancellationToken ct)
        {
            var token = GetToken();

            if (token == null)
                return RedirectToLogin();


            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, token,
            new BreadcrumbItem
            {
                Text = "New Contract",
                Url = null
            });

            return await HandleAsync(async ct =>
            {

                var createrecord = await _service.AddClientCompanyContractAsync(dto, token, ct);

                if (createrecord == null || createrecord.Data == false)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating this Company Client Contract Error:{createrecord.Message}", MessageDisplay.Inline);

                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", createrecord.Message, MessageDisplay.Inline);

                await LoadInfoAsync(token, ct, 0);

                return RedirectToAction(nameof(Index));

            }, ct);

        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {

            List<WMSClientCompaniesReadDTO> listclientcompaniesactives = new List<WMSClientCompaniesReadDTO>();

            List<WMSCompaniesDTO> listactivecompanies = new List<WMSCompaniesDTO>();

            var listclientcompanies = await _companyClientService.GetAllClientCompaniesInformation(token, ct);

            if (listclientcompanies.Data != null)
            {
                listclientcompaniesactives = listclientcompanies.Data.Where(x => x.Active == true).ToList();
            }
            if (idselected == 0)
            {

                ViewData["companiesclient"] = new SelectList(listclientcompaniesactives, "Id", "Name");
            }
            else
            {
                ViewData["companiesclient"] = new SelectList(listclientcompaniesactives, "Id", "Name", idselected);
            }


            var listcompanies = await _companyService.GetAllCompaniesInformation(token, ct);



            if (listcompanies.Data != null)
            {
                listactivecompanies = listcompanies.Data.Where(x => x.Active == true).ToList();
            }
            if (idselected == 0)
            {

                ViewData["listcompanies"] = new SelectList(listactivecompanies, "Id", "Name");
            }
            else
            {
                ViewData["listcompanies"] = new SelectList(listactivecompanies, "Id", "Name", idselected);
            }
        }
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.Companies;
using DUNES.UI.Services.WMS.Masters.CompaniesContract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompaniesContract
{
    [Authorize]
    public class CompaniesClientContractUIController : BaseController
    {
        private readonly ICompaniesClientContractWMSUIService _service;
        private readonly IClientCompaniesWMSUIService _companyClientService;
        private readonly ICompaniesWMSUIService _companyService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020303";
        private const string MENU_CODE_CRUD = "01020303ZZ";

        private const string PERMISSION_ACCESS = "Masters.CompaniesContract.Access";
        private const string PERMISSION_CREATE = "Masters.CompaniesContract.Create";
        private const string PERMISSION_UPDATE = "Masters.CompaniesContract.Update";
        private const string PERMISSION_DELETE = "Masters.CompaniesContract.Delete";

        public CompaniesClientContractUIController(
            ICompaniesClientContractWMSUIService service,
            IMenuClientUIService menuClientService,
            IClientCompaniesWMSUIService companyClientService,
            ICompaniesWMSUIService companyService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper)
        {
            _service = service;
            _menuClientService = menuClientService;
            _companyClientService = companyClientService;
            _companyService = companyService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_ACCESS);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });

            var contractList = await _service.GetAllClientCompaniesContractInformationAsync(CurrentToken, ct);
            return View(contractList.Data);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_CREATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await LoadInfoAsync(CurrentToken, ct, 0);

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "New Contract", Url = null });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompaniesContractDTO dto, CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_CREATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "New Contract", Url = null });

            return await HandleAsync(async ct =>
            {
                var createrecord = await _service.AddClientCompanyContractAsync(dto, CurrentToken, ct);

                if (createrecord is null || createrecord.Data == false)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating this Company Client Contract Error:{createrecord?.Message}", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", createrecord.Message, MessageDisplay.Inline);
                await LoadInfoAsync(CurrentToken, ct, 0);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {
            var listclientcompanies = await _companyClientService.GetAllClientCompaniesInformation(token, ct);
            var clientCompaniesActive = listclientcompanies.Data?.Where(x => x.Active).ToList() ?? new();

            ViewData["companiesclient"] = new SelectList(clientCompaniesActive, "Id", "Name", idselected == 0 ? null : idselected);

            var listcompanies = await _companyService.GetAllCompaniesInformation(token, ct);
            var companiesActive = listcompanies.Data?.Where(x => x.Active).ToList() ?? new();

            ViewData["listcompanies"] = new SelectList(companiesActive, "Id", "Name", idselected == 0 ? null : idselected);
        }
    }
}
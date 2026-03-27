using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.CompaniesClientDivision;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.CompaniesClientDivision
{
    [Authorize]
    public class CompaniesClientDivisionUIController : BaseController
    {
        private readonly IClientCompaniesWMSUIService _companyClientService;
        private readonly IMenuClientUIService _menuClientService;
        private readonly ICompaniesClientDivisionWMSUIService _service;

        private const string MENU_CODE_INDEX = "01020302";
        private const string MENU_CODE_CRUD = "01020302ZZ";

        private const string PERMISSION_ACCESS = "WMS.CompanyClientDivision.Access";
        private const string PERMISSION_CREATE = "WMS.CompanyClientDivision.Create";

        public CompaniesClientDivisionUIController(
            ICompaniesClientDivisionWMSUIService service,
            IMenuClientUIService menuClientService,
            IClientCompaniesWMSUIService companyClientService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper)
        {
            _service = service;
            _menuClientService = menuClientService;
            _companyClientService = companyClientService;
        }

        public async Task<IActionResult> Index(int? companyclientid, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "", Url = null });

            companyclientid ??= 0;

            return await HandleAsync(async ct =>
            {
                await LoadInfoAsync(CurrentToken, ct, companyclientid.Value);

                if (companyclientid < 0)
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid company client id.", MessageDisplay.Inline);
                    return View(new List<WMSCompanyClientDivisionReadDTO>());
                }

                var listsdivision = await _service.GetAllCompaniesClientDivisionInformationByCompanyClient(
                    companyclientid.Value, CurrentToken, ct);

                if (!listsdivision.Success)
                {
                    MessageHelper.SetMessage(this, "danger", listsdivision.Message, MessageDisplay.Inline);
                }

                if (companyclientid != 0 && listsdivision.Data == null)
                {
                    MessageHelper.SetMessage(this, "information", "Division not found for this company client", MessageDisplay.Inline);
                }

                return View(listsdivision.Data ?? new List<WMSCompanyClientDivisionReadDTO>());
            }, ct);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await LoadInfoAsync(CurrentToken, ct, 0);

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "New Company Client Division", Url = null });

            return View(new WMSCompanyClientDivisionDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCompanyClientDivisionDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(MENU_CODE_CRUD, _menuClientService, ct, CurrentToken,
                new BreadcrumbItem { Text = "New Company Client Division", Url = null });

            return await HandleAsync(async ct =>
            {
                if (dto == null || dto.Idcompanyclient <= 0 || string.IsNullOrWhiteSpace(dto.DivisionName))
                {
                    MessageHelper.SetMessage(this, "danger", "Invalid data.", MessageDisplay.Inline);
                    await LoadInfoAsync(CurrentToken, ct, dto?.Idcompanyclient ?? 0);
                    return View(dto);
                }

                var infocompany = await _companyClientService.GetClientCompanyInformationByIdAsync(
                    dto.Idcompanyclient, CurrentToken, ct);

                if (infocompany.Data == null)
                {
                    MessageHelper.SetMessage(this, "danger", "Company client not found", MessageDisplay.Inline);
                    await LoadInfoAsync(CurrentToken, ct, dto.Idcompanyclient);
                    return View(dto);
                }

                var infodivisoname = await _service.GetCompanyClientDivisionByNameAsync(
                    dto.Idcompanyclient, dto.DivisionName.Trim(), CurrentToken, ct);

                if (infodivisoname.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", "Company Client division name already exists", MessageDisplay.Inline);
                    await LoadInfoAsync(CurrentToken, ct, dto.Idcompanyclient);
                    return View(dto);
                }

                var createrecord = await _service.AddClientCompanyDivisionAsync(dto, CurrentToken, ct);

                if (!createrecord.Data)
                {
                    MessageHelper.SetMessage(this, "danger",
                        $"Error creating this Company Client Division. Error: {createrecord.Message}",
                        MessageDisplay.Inline);

                    await LoadInfoAsync(CurrentToken, ct, dto.Idcompanyclient);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", "Company Client Division created", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index), new { companyclientid = dto.Idcompanyclient });
            }, ct);
        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {
            var listcompanies = await _companyClientService.GetAllClientCompaniesInformation(token, ct);
            var listcompaniesactives = listcompanies.Data?.Where(x => x.Active).ToList() ?? new();

            ViewData["companiesclient"] = idselected != 0
                ? new SelectList(listcompaniesactives, "Id", "Name", idselected)
                : new SelectList(listcompaniesactives, "Id", "Name");
        }
    }
}
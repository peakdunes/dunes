using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.Countries;
using DUNES.UI.Services.WMS.Masters.StatesCountries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Controllers.WMS.Masters.StatesCountries
{
    [Authorize]
    public class StatesCountriesUIController : BaseController
    {
        private readonly IStatesCountriesWMSUIService _service;
        private readonly ICountriesWMSUIService _countryService;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020802";
        private const string MENU_CODE_CRUD = "01020802ZZ";

        private const string PERMISSION_ACCESS = "Masters.StatesCountries.Access";
        private const string PERMISSION_CREATE = "Masters.StatesCountries.Create";
        private const string PERMISSION_UPDATE = "Masters.StatesCountries.Update";
        private const string PERMISSION_DELETE = "Masters.StatesCountries.Delete";

        public StatesCountriesUIController(
            IStatesCountriesWMSUIService service,
            IMenuClientUIService menuClientService,
            ICountriesWMSUIService countryService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _service = service;
            _menuClientService = menuClientService;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(int? countryId, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

            var token = CurrentToken;
            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                token,
                new BreadcrumbItem { Text = "", Url = null });

            countryId ??= 0;

            await LoadCrudActionsAsync("Masters", "StatesCountries", ct);

            return await HandleAsync(async ct =>
            {
                await LoadInfoAsync(token, ct, 0);

                var liststates = await _service.GetAllStatesCountryInformation(countryId.Value, token, ct);

                return View(liststates.Data ?? new List<WMSStatesCountriesReadDTO>());
            }, ct);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            var token = CurrentToken;
            if (token == null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                token,
                new BreadcrumbItem { Text = "New State", Url = null });

            await LoadInfoAsync(token, ct, 0);

            return View(new WMSStatesCountriesDTO());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSStatesCountriesDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            var token = CurrentToken;
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var existsName = await _service.GetStateCountryInformationByIdentificationAsync(
                    dto.Idcountry,
                    dto.Name!.Trim(),
                    token,
                    ct);

                if (existsName.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", existsName.Message.Trim(), MessageDisplay.Inline);
                    await LoadInfoAsync(token, ct, dto.Idcountry);
                    return View(dto);
                }

                var existsIso = await _service.GetStateCountryInformationByISOCodeAsync(
                    dto.Idcountry,
                    dto.Sigla!.Trim(),
                    token,
                    ct);

                if (existsIso.Data != null)
                {
                    MessageHelper.SetMessage(this, "danger", existsIso.Message.Trim(), MessageDisplay.Inline);
                    await LoadInfoAsync(token, ct, dto.Idcountry);
                    return View(dto);
                }

                var result = await _service.AddStateCountryAsync(dto, token, ct);

                if (!result.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating state: {result.Message}", MessageDisplay.Inline);
                    await LoadInfoAsync(token, ct, dto.Idcountry);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", "State created successfully", MessageDisplay.Inline);

                return RedirectToAction(nameof(Index));
            }, ct);
        }

        private async Task LoadInfoAsync(string token, CancellationToken ct, int idselected)
        {
            var listcountries = await _countryService.GetAllCountriesInformation(token, ct);

            var data = listcountries.Data ?? new List<WMSCountriesDTO>();
            var listactives = data.Where(x => x.Active).ToList();

            ViewBag.listcountries = idselected == 0
                ? new SelectList(listactives, "Id", "Name")
                : new SelectList(listactives, "Id", "Name", idselected);
        }
    }
}
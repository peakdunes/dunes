using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.Countries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Countries
{
    [Authorize]
    public class CountriesController : BaseController
    {
        private readonly ICountriesWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020801";
        private const string MENU_CODE_CRUD = "01020801ZZ";

        private const string PERMISSION_ACCESS = "Masters.Countries.Access";
        private const string PERMISSION_CREATE = "Masters.Countries.Create";
        private const string PERMISSION_UPDATE = "Masters.Countries.Update";
        private const string PERMISSION_DELETE = "Masters.Countries.Delete";

        public CountriesController(
            ICountriesWMSUIService service,
            IMenuClientUIService menuClientService,
             IAuthPermissionUIService authPermissionUIService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper, authPermissionUIService)
        {
            _service = service;
            _menuClientService = menuClientService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_ACCESS))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem
                {
                    Text = "",
                    Url = null
                });

            await LoadCrudActionsAsync("Masters", "Countries", ct);

            return await HandleAsync(async ct =>
            {
                var listcountries = await _service.GetAllCountriesInformation(CurrentToken, ct);
                return View(listcountries.Data);
            }, ct);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem
                {
                    Text = "New Country",
                    Url = null
                });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSCountriesDTO dto, CancellationToken ct)
        {
            if (!_permissionSessionHelper.HasPermission(PERMISSION_CREATE))
                return Forbid();

            if (CurrentToken is null)
                return RedirectToLogin();

            await SetMenuBreadcrumbAsync(
                MENU_CODE_CRUD,
                _menuClientService,
                ct,
                CurrentToken,
                new BreadcrumbItem
                {
                    Text = "New Country",
                    Url = null
                });

            return await HandleAsync(async ct =>
            {
                var exists = await _service.GetCountryInformationByIdentificationAsync(
                    dto.Name!.Trim(),
                    null,
                    CurrentToken,
                    ct);

                if (exists.Data)
                {
                    MessageHelper.SetMessage(this, "danger", "Country Name already exists", MessageDisplay.Inline);
                    return View(dto);
                }

                var result = await _service.AddCountryAsync(dto, CurrentToken, ct);

                if (!result.Data)
                {
                    MessageHelper.SetMessage(this, "danger", $"Error creating country: {result.Message}", MessageDisplay.Inline);
                    return View(dto);
                }

                MessageHelper.SetMessage(this, "success", "Country created successfully", MessageDisplay.Inline);
                return RedirectToAction(nameof(Index));
            }, ct);
        }
    }
}
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory.Common;
using DUNES.UI.Services.Inventory.PickProcess;
using DUNES.UI.Services.Print;
using DUNES.UI.Services.WMS.Masters.Companies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Companies
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class companiesUIController : BaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICompaniesWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "010201";
        private const string MENU_CODE_CRUD = "010201ZZ";

        private const string PERMISSION_ACCESS = "Masters.Companies.Access";
        private const string PERMISSION_CREATE = "Masters.Companies.Create";
        private const string PERMISSION_UPDATE = "Masters.Companies.Update";
        private const string PERMISSION_DELETE = "Masters.Companies.Delete";

        public companiesUIController(
            IHttpClientFactory httpClientFactory,
            ICompaniesWMSUIService service,
            IMenuClientUIService menuClientService,
            IUserPermissionSessionHelper permissionSessionHelper)
            : base(permissionSessionHelper)
        {
            _httpClientFactory = httpClientFactory;
            _service = service;
            _menuClientService = menuClientService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var deny = RequireTokenAndPermission(PERMISSION_ACCESS);
            if (deny is not null)
                return deny;

            var token = CurrentToken;

            await SetMenuBreadcrumbAsync(
                MENU_CODE_INDEX,
                _menuClientService, ct, token!,
                new BreadcrumbItem
                {
                    Text = "List",
                    Url = null
                });

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
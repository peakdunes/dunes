using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Auth;
using DUNES.UI.Services.WMS.Masters.Locations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Locations
{
    [Authorize]
    public class LocationsUIController : BaseController
    {
        private readonly ILocationsWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;
        private readonly IAuthPermissionUIService _authPermissionUIService;


        private const string MENU_CODE_INDEX = "01020804";
        private const string MENU_CODE_CRUD = "01020804ZZ";

        private const string PERMISSION_ACCESS = "Masters.Locations.Access";
        private const string PERMISSION_CREATE = "Masters.Locations.Create";
        private const string PERMISSION_UPDATE = "Masters.Locations.Update";
        private const string PERMISSION_DELETE = "Masters.Locations.Delete";

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public LocationsUIController(
            ILocationsWMSUIService service,
            IMenuClientUIService menuClientService,
            IAuthPermissionUIService authPermissionUIService,
           
             IUserPermissionSessionHelper permissionSessionHelper) : base(permissionSessionHelper,authPermissionUIService) 
        {
            _service = service;
            _authPermissionUIService = authPermissionUIService;
            _menuClientService = menuClientService;
            
        }

        /// <summary>
        /// Muestra el listado de ubicaciones (Locations).
        /// </summary>
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
                new BreadcrumbItem
                {
                    Text = "List",
                    Url = null
                });

            await LoadCrudActionsAsync("Masters", "Locations", ct);


            var locationList = await _service.GetAllAsync(CurrentToken, ct);

            return View(locationList.Data);
        }

        public IActionResult Details(int id)
        {

            var deny = RequireTokenAndPermission(PERMISSION_ACCESS);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

           

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var deny = RequireTokenAndPermission(PERMISSION_CREATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

           

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {

            var deny = RequireTokenAndPermission(PERMISSION_CREATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

          

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {

            var deny = RequireTokenAndPermission(PERMISSION_UPDATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

         

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            var deny = RequireTokenAndPermission(PERMISSION_UPDATE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

            

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var deny = RequireTokenAndPermission(PERMISSION_DELETE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

          

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            var deny = RequireTokenAndPermission(PERMISSION_DELETE);
            if (deny is not null)
                return deny;

            if (CurrentToken is null)
                return RedirectToLogin();

           
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
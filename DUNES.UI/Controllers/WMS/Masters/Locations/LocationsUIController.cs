using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Locations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Locations
{
    public class LocationsUIController : BaseController
    {

        private readonly ILocationsWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        public readonly IConfiguration _config;
        public readonly int _companyDefault;
        private const string MENU_CODE_INDEX = "010202";
        private const string MENU_CODE_CRUD = "010202ZZ";

        /// <summary>
        /// Contructor (DI)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="menuClientService"></param>
        /// <param name="config"></param>
        public LocationsUIController(ILocationsWMSUIService service, IMenuClientUIService menuClientService, IConfiguration config)
        {
            _service = service;
            _config = config;
            _companyDefault = _config.GetValue("companyDefault", 1);
            _menuClientService = menuClientService;
            _config = config;
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
              Text = "List",   // actual
              Url = null
          });


            var locationlist = await _service.GetAllAsync(token, ct);


            return View(locationlist.Data);
        }

        // GET: LocationsUIController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: LocationsUIController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LocationsUIController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationsUIController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LocationsUIController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LocationsUIController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LocationsUIController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
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

using DUNES.UI.Models;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.WMS.Masters.Locations;
using Microsoft.AspNetCore.Mvc;

namespace DUNES.UI.Controllers.WMS.Masters.Locations
{
    public class LocationsUIController : BaseController
    {
        private readonly ILocationsWMSUIService _service;
        private readonly IMenuClientUIService _menuClientService;

        private const string MENU_CODE_INDEX = "01020804";
        private const string MENU_CODE_CRUD = "01020804ZZ";

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public LocationsUIController(
            ILocationsWMSUIService service,
            IMenuClientUIService menuClientService)
        {
            _service = service;
            _menuClientService = menuClientService;
        }

        /// <summary>
        /// Muestra el listado de ubicaciones (Locations).
        /// </summary>
        public async Task<IActionResult> Index(CancellationToken ct)
        {
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

            var locationList = await _service.GetAllAsync(CurrentToken, ct);

            return View(locationList.Data);
        }

        // 👇 Las siguientes acciones son placeholders vacíos (puedes eliminarlos si no los necesitas)

        public ActionResult Details(int id) => View();

        public ActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try { return RedirectToAction(nameof(Index)); }
            catch { return View(); }
        }

        public ActionResult Edit(int id) => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try { return RedirectToAction(nameof(Index)); }
            catch { return View(); }
        }

        public ActionResult Delete(int id) => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try { return RedirectToAction(nameof(Index)); }
            catch { return View(); }
        }
    }
}

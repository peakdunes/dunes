using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Inventory.ASN;
using DUNES.UI.Services.Inventory.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;

namespace DUNES.UI.Controllers.Inventory.CurrentInventory
{
    public class CurrentInventoryController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IASNUIService _ASNService;
        private readonly ICommonINVUIService _CommonINVService;
        private readonly IConfiguration _config;
        private readonly int _companyDefault;
        private readonly string _typeDocument = "ASN";


        public CurrentInventoryController(IHttpClientFactory httpClientFactory, IConfiguration config, IASNUIService ASNService,
            ICommonINVUIService CommonINVService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _ASNService = ASNService;
            _CommonINVService = CommonINVService;
            _companyDefault = _config.GetValue<int>("companyDefault", 1);

        }

        // GET: CurrentInventoryController
        public async Task<ActionResult> Index(CancellationToken ct)
        {
            var token = GetToken();
            
            if (token == null)
                return (ActionResult)RedirectToLogin();
            //get all active locations

            var locations = await _CommonINVService.GetAllActiveLocationsByCompany(_companyDefault, token, ct);

            if (locations.Error != null)
            {
                MessageHelper.SetMessage(this, "danger", locations.Error, MessageDisplay.Inline);
                return View();
            }

            if (locations.Data == null || locations.Data.Count <= 0)
            {
                MessageHelper.SetMessage(this, "danger", "there is not company locations actives", MessageDisplay.Inline);
                return View();
            }

            ViewData["locations"] = new SelectList(locations.Data, "Id", "Name");

            return View();
        }

        public async Task<ActionResult> getAllInventoryTypeAndItems(int locationid)
        {



            return new ObjectResult(new { });
        }

        // GET: CurrentInventoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CurrentInventoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurrentInventoryController/Create
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

        // GET: CurrentInventoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CurrentInventoryController/Edit/5
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

        // GET: CurrentInventoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CurrentInventoryController/Delete/5
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

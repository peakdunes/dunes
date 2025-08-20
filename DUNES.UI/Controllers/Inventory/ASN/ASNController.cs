using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DUNES.UI.Controllers.Inventory.ASN
{
    public class AsnController : Controller
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IASNService _ASNService;
        public readonly IConfiguration _config;


        public AsnController(IHttpClientFactory httpClientFactory, IConfiguration config, IASNService ASNService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _ASNService = ASNService;

        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Receiving(string? asnnumber)
        {
            ASNHdr objenc = new ASNHdr();
            List<ASNItemDetail> objdet = new List<ASNItemDetail>();

            ASNDto objdto = new ASNDto { asnHdr = objenc, itemDetail = objdet };


            if (string.IsNullOrWhiteSpace(asnnumber))
                return View(objdto);

            var token = HttpContext.Session.GetString("JWToken");

            if (token == null)
            {

                MessageHelper.SetMessage(this, "danger", "Token Invalid. Please try again.", MessageDisplay.Inline);

                return RedirectToAction("Index", "Home"); // regresar a la misma vista
            }

            var infoasn = await _ASNService.GetAsnInfo(asnnumber, token);
          

            if (infoasn.Error != null)
            {
                MessageHelper.SetMessage(this, "danger", infoasn.Error, MessageDisplay.Inline);
                return View(objdto);
            }
            else
            {
                return View(infoasn.Data);
            }
        }
      
    }
}

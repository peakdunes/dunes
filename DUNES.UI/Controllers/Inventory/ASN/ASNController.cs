using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory;
using DUNES.UI.WiewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DUNES.UI.Controllers.Inventory.ASN
{
    public class AsnController : BaseController
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

        public async Task<IActionResult> Receiving(string? asnnumber, CancellationToken ct)
        {

          
            return await HandleAsync(async ct =>
            {
                ASNDto objdto = new ASNDto { asnHdr = new ASNHdr(), itemDetail = new List<ASNItemDetail>() };

               
                AsnDtoCompanyClients objresult = new AsnDtoCompanyClients
                {
                    asdDto = objdto,
                    listcompanyclients = new List<WMSClientCompanies>()
                };

                if (string.IsNullOrWhiteSpace(asnnumber))
                    return View(objresult);

                var token = GetToken();
                if (token == null)
                    return RedirectToLogin();

                var infoasn = await _ASNService.GetAsnInfo(asnnumber, token, ct);

                if (infoasn.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", infoasn.Error, MessageDisplay.Inline);
                    return View(objdto);
                }


                var listclients = await _ASNService.GetClientCompanies( token, ct);

                if (listclients.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", listclients.Error, MessageDisplay.Inline);
                    return View(objresult);
                }

                if (listclients.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not company clients registed", MessageDisplay.Inline);
                    return View(objresult);
                }

                ViewData["companies"] = new SelectList(listclients.Data, "Id", "CompanyId");

                objresult.asdDto = infoasn.Data!;
                objresult.listcompanyclients = listclients.Data!;

                return View(objresult);

            }, ct);
        }
      
    }
}

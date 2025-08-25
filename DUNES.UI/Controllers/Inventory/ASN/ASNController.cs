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
        public readonly int _companyDefault;


        public AsnController(IHttpClientFactory httpClientFactory, IConfiguration config, IASNService ASNService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _ASNService = ASNService;
            _companyDefault = _config.GetValue<int>("companyDefault", 1);

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
                    return View(objresult);
                }


                //var infobines = await _ASNService.GetAllActiveBinsByCompanyClient(1, "ZEBRA PAR1", token, ct);


                var listclients = await _ASNService.GetClientCompanies(token, ct);

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

                ViewData["companies"] = new SelectList(listclients.Data, "CompanyId", "CompanyId");

                objresult.asdDto = infoasn.Data!;
                objresult.listcompanyclients = listclients.Data!;

                return View(objresult);

            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> getClientBins(string companyclient, CancellationToken ct)
        {

            int companyid = _companyDefault;

            List<WMSBins> listbinesresult = new List<WMSBins>();

            if (companyid <= 0 || string.IsNullOrWhiteSpace(companyclient))
                return BadRequest(new { message = "Parámetros inválidos.", error = "VALIDATION", data = Array.Empty<object>() });

            // 2) Token (estándar de tu BaseController)
            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                var listbines = await _ASNService.GetAllActiveBinsByCompanyClient(companyid, companyclient, token, ct);
                
                if (listbines.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not company clients registed", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach(var b in listbines.Data)
                {
                    WMSBins objdet = new WMSBins();

                    objdet.Id = b.Id;
                    objdet.TagName = b.TagName.Trim();

                    listbinesresult.Add(objdet);
                }

                return Ok(new
                {
                    message = "OK",
                    error = (string)null,
                    data = listbinesresult
                });

            }, ct);

         
            
        }
    }
}

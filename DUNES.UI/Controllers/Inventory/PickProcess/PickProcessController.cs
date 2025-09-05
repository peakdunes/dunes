using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Inventory.ASN;
using DUNES.UI.Services.Inventory.PickProcess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DUNES.UI.Controllers.Inventory.PickProcess
{

    public class PickProcessController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPickProcessService _service;
        private readonly IASNService _ASNService;
        public readonly IConfiguration _config;
        public readonly int _companyDefault;


        public PickProcessController(IHttpClientFactory httpClientFactory, IConfiguration config,
            IPickProcessService service, IASNService ASNService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue<int>("companyDefault", 1);
            _ASNService = ASNService;

        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PickProcessReceiving(string? pickprocessnumber, CancellationToken ct)
        {

            return await HandleAsync(async ct =>
            {
                PickProcessHdr objhdr = new PickProcessHdr();
                List<PickProcessItemDetail> objlist = new List<PickProcessItemDetail>();

                PickProcessDto objresult = new PickProcessDto
                {
                    PickProcessHdr = objhdr,
                    ListItems = objlist

                };


                if (string.IsNullOrWhiteSpace(pickprocessnumber))
                    return View(objresult);

                var token = GetToken();
                if (token == null)
                    return RedirectToLogin();

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



                var infopickProcess = await _service.GetPickProcessAllInfo(pickprocessnumber, token, ct);

                if (infopickProcess.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", infopickProcess.Error, MessageDisplay.Inline);
                    return View(objresult);
                }

                objresult.PickProcessHdr = infopickProcess.Data!.PickProcessHdr;
                objresult.ListItems = infopickProcess.Data.ListItems;


                HttpContext.Session.SetString("pickProcessDetail", JsonConvert.SerializeObject(infopickProcess.Data.ListItems));


                return View(objresult);

            }, ct);
        }

        [HttpPost]

        public async Task<IActionResult> checkInventoryByPartNumber(string companyclient, string partnumber, int lineid, CancellationToken ct)
        {

            var token = GetToken();


            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {

                string partnumberzeb = partnumber.Contains("ZEBRA") ? partnumber.Trim() : "ZEBRA-" + partnumber.Trim();

                var listinventory = await _ASNService.GetInventoryByItem(_companyDefault, companyclient, partnumberzeb, token, ct);

                return new ObjectResult(new { status = listinventory.Error, listinventory = listinventory.Data, });

            }, ct);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> getAllAditionalInformation(string companyclient, CancellationToken ct)
        {


            ASNProcessInformationDto objinformation = new ASNProcessInformationDto();

            List<WMSBinsDto> listbinesresult = new List<WMSBinsDto>();

            List<WMSConceptsDto> listconceptsresult = new List<WMSConceptsDto>();

            List<WMSInputTransactionsDto> listinputtransactionsresult = new List<WMSInputTransactionsDto>();



            if (_companyDefault <= 0 || string.IsNullOrWhiteSpace(companyclient))
                return BadRequest(new { message = "Parámetros inválidos.", error = "VALIDATION", data = Array.Empty<object>() });

            // 2) Token (estándar de tu BaseController)
            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                //load bins

                var listbines = await _ASNService.GetAllActiveBinsByCompanyClient(_companyDefault, companyclient, token, ct);

                if (listbines.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not bins created for this  company", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listbines.Data)
                {
                    WMSBinsDto objdet = new WMSBinsDto();

                    objdet.Id = b.Id;
                    objdet.TagName = b.TagName.Trim();

                    listbinesresult.Add(objdet);
                }

                objinformation.listbines = listbinesresult;

                //load concepts

                var listconcepts = await _ASNService.GetAllActiveConceptsByCompanyClient(_companyDefault, companyclient, token, ct);

                if (listconcepts.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not WMS transactions concept created for this  company", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listconcepts.Data)
                {
                    WMSConceptsDto objdet = new WMSConceptsDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();

                    listconceptsresult.Add(objdet);
                }

                objinformation.listconcepts = listconceptsresult;

                //load input transactions

                var listtransactions = await _ASNService.GetAllActiveOutputTransactionsByCompanyClient(_companyDefault, companyclient, token, ct);

                if (listtransactions.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not WMS output transactions created for this  company", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listtransactions.Data)
                {
                    WMSInputTransactionsDto objdet = new WMSInputTransactionsDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();

                    listinputtransactionsresult.Add(objdet);
                }

                objinformation.listinputtransactions = listinputtransactionsresult;

                return Ok(new
                {
                    message = "OK",
                    data = objinformation
                });

            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantityPickProcess([FromBody] UpdateQuantityPickProcessRequestTm req, CancellationToken ct)
        {

            return await HandleAsync(async ct =>
            {

                if (req?.itemslist == null || req.itemslist.Count == 0)

                    return Ok(new { status = "There is not Bins Distribution" });

                var lista = JsonConvert.DeserializeObject<List<PickProcessItemDetail>>(HttpContext.Session.GetString("pickProcessDetail"));

                foreach (var list in lista!)
                {
                    foreach (var info in req!.itemslist)
                    {
                        if (Convert.ToInt32(list.LindId) == info.lineid)
                        {
                            list.QuantityProcess += info.qty;
                        }
                    }
                }

                HttpContext.Session.SetString("pickProcessDetail", JsonConvert.SerializeObject(lista));

                 return Ok(new { status = "OK", lista = lista });
            }, ct);
        }
    }
}

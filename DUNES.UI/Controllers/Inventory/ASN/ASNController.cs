using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory;
using DUNES.UI.WiewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
            //we create a session variable to load all bins item distribution

            List<BinsToLoadTm> listbinespartno = new List<BinsToLoadTm>();
            
            HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(listbinespartno));


            //////////
            return await HandleAsync(async ct =>
            {
                ASNWm objdto = new ASNWm { asnHdr = new ASNHdr(), itemDetail = new List<ASNItemDetail>() };


                AsnCompanyClientsWm objresult = new AsnCompanyClientsWm
                {
                    asdDto = objdto,
                    listcompanyclients = new List<WMSClientCompaniesDto>()
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

        public async Task<IActionResult> getAllAditionalInformation(string companyclient, CancellationToken ct)
        {


            ASNProcessInformationDto objinformation = new ASNProcessInformationDto();

            int companyid = _companyDefault;

            List<WMSBinsDto> listbinesresult = new List<WMSBinsDto>();

            List<WMSConceptsDto> listconceptsresult = new List<WMSConceptsDto>();

            List<WMSInputTransactionsDto> listinputtransactionsresult = new List<WMSInputTransactionsDto>();

            List<InventoryTypeDto> listinventorytypesresult = new List<InventoryTypeDto>();

            List<itemstatusDto> listitemstatusresult = new List<itemstatusDto>();

            List<WMSInventoryTypeDto> listwmsinventorytypesresult = new List<WMSInventoryTypeDto>();

            if (companyid <= 0 || string.IsNullOrWhiteSpace(companyclient))
                return BadRequest(new { message = "Parámetros inválidos.", error = "VALIDATION", data = Array.Empty<object>() });

            // 2) Token (estándar de tu BaseController)
            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                //load bins

                var listbines = await _ASNService.GetAllActiveBinsByCompanyClient(companyid, companyclient, token, ct);

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

                var listconcepts = await _ASNService.GetAllActiveConceptsByCompanyClient(companyid, companyclient, token, ct);

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

                var listtransactions = await _ASNService.GetAllActiveInputTransactionsByCompanyClient(companyid, companyclient, token, ct);

                if (listtransactions.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not WMS input transactions created for this  company", MessageDisplay.Inline);
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


                //load ZEBRA inventory types

                var listinventorytypes = await _ASNService.GetAllActiveInventoryTypes(token, ct);

                if (listinventorytypes.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not inventory types created", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listinventorytypes.Data)
                {
                    InventoryTypeDto objdet = new InventoryTypeDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();

                    listinventorytypesresult.Add(objdet);
                }

                objinformation.listinventorytype = listinventorytypesresult;

                //load WMS inventory types

                var listwmsinventorytypes = await _ASNService.GetAllActiveWmsInventoryTypes(companyid, companyclient, token, ct);

                if (listwmsinventorytypes.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not WMS inventory types created for this  company", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listwmsinventorytypes.Data)
                {
                    WMSInventoryTypeDto objdet = new WMSInventoryTypeDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();

                    listwmsinventorytypesresult.Add(objdet);
                }

                objinformation.listwmsinventorytype = listwmsinventorytypesresult;


                //load WMS item status

                var listitemstatus = await _ASNService.GetAllActiveItemStatus(companyid, companyclient, token, ct);

                if (listitemstatus.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not WMS inventory types created for this  company", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listitemstatus.Data)
                {
                    itemstatusDto objdet = new itemstatusDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();

                    listitemstatusresult.Add(objdet);
                }

                objinformation.listitemstatus = listitemstatusresult;


                return Ok(new
                {
                    message = "OK",
                    error = (string)null,
                    data = objinformation
                });

            }, ct);



        }




        [HttpPost]
        public IActionResult addqtybin(int binid, int typeid, int qtybin, string partno, string tagname, string typename, int lineid, int statusid)


        {
            string errormessage = "OK";

            try
            {
                List<BinsToLoadTm> objlist = new List<BinsToLoadTm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadTm>>(HttpContext.Session.GetString("listbinesdistribution"));

                int totalqty = 0;

                if (lista.Count == 0)
                {
                    BinsToLoadTm objdet = new BinsToLoadTm();

                    objdet.Id = lineid;
                    objdet.tagname = tagname;
                    objdet.inventorytype = typeid;
                    objdet.partnumber = partno;
                    objdet.typename = typename;
                    objdet.qty = qtybin;
                    objdet.lineasnid = lineid;
                    objdet.statusid = statusid;

                    totalqty += objdet.qty;

                    lista.Add(objdet);
                }
                else
                {
                    var haydatos = false;

                    foreach (var item in lista)
                    {

                        if (item.Id == binid && item.inventorytype == typeid && item.partnumber == partno)
                        {
                            item.qty += qtybin;

                            haydatos = true;
                        }

                    }

                    if (!haydatos)
                    {
                        BinsToLoadTm objdet = new BinsToLoadTm();

                        objdet.Id = lineid;
                        objdet.tagname = tagname.Trim();
                        objdet.inventorytype = typeid;
                        objdet.partnumber = partno.Trim();
                        objdet.typename = typename.Trim();
                        objdet.qty = qtybin;
                        objdet.lineasnid = lineid;
                        objdet.statusid = statusid;

                        lista.Add(objdet);

                    }
                }

                totalqty = 0;

                bool thereisdata = false;

                foreach (var item in lista)
                {

                    if (item.partnumber == partno)
                    {

                        totalqty += item.qty;

                    }
                    if (item.qty > 0)
                    {
                        thereisdata = true;
                    }
                }


                var listaPartNumber = "";

                //var listaPartNumber = JsonConvert.DeserializeObject<List< els.TzebB2bAsnLineItemTblItemInbConsReq>>(HttpContext.Session.GetString("listPartNumberDetail"));

                //foreach (var detail in listaPartNumber)
                //{
                //    foreach (var item in lista)
                //    {
                //        if (item.lineasnid == detail.LineNum)
                //        {

                //            detail.qtypending = item.qty;
                //            detail.thereisdistribution = true;
                //            break;
                //        }

                //    }
                //}

                // HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(listaPartNumber));

                HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(lista));

                return new ObjectResult(new { status = errormessage, listbines = lista.Where(x => x.lineasnid == lineid), totalqty = totalqty, thereisdata = thereisdata, listapartnumber = listaPartNumber });



            }
            catch (Exception ex)
            {
                return new ObjectResult(new { status = errormessage });
            }
        }

    }
}

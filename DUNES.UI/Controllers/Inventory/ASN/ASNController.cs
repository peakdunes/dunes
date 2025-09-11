using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory.ASN;
using DUNES.UI.Services.Inventory.Common;
using DUNES.UI.WiewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace DUNES.UI.Controllers.Inventory.ASN
{
    public class AsnController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IASNService _ASNService;
        private readonly ICommonINVService _CommonINVService;
        public readonly IConfiguration _config;
        public readonly int _companyDefault;


        public AsnController(IHttpClientFactory httpClientFactory, IConfiguration config, IASNService ASNService,
            ICommonINVService CommonINVService)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _ASNService = ASNService;
            _CommonINVService = CommonINVService;
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


                var listclients = await _CommonINVService.GetClientCompanies(token, ct);

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

                if (infoasn.Data.itemDetail.Count() <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not part number detail for this ASN", MessageDisplay.Inline);
                    return View(objresult);
                }


                HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(infoasn.Data.itemDetail));


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

            List<WMSBinsDto> listbinesresult = new List<WMSBinsDto>();

            List<WMSConceptsDto> listconceptsresult = new List<WMSConceptsDto>();

            List<WMSInputTransactionsDto> listinputtransactionsresult = new List<WMSInputTransactionsDto>();

            List<InventoryTypeDto> listinventorytypesresult = new List<InventoryTypeDto>();

            List<itemstatusDto> listitemstatusresult = new List<itemstatusDto>();

            List<WMSInventoryTypeDto> listwmsinventorytypesresult = new List<WMSInventoryTypeDto>();

            if (_companyDefault <= 0 || string.IsNullOrWhiteSpace(companyclient))
                return BadRequest(new { message = "Parámetros inválidos.", error = "VALIDATION", data = Array.Empty<object>() });

            // 2) Token (estándar de tu BaseController)
            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                //load bins

                var listbines = await _CommonINVService.GetAllActiveBinsByCompanyClient(_companyDefault, companyclient, token, ct);

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

                var listconcepts = await _CommonINVService.GetAllActiveConceptsByCompanyClient(_companyDefault, companyclient, token, ct);

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

                var listtransactions = await _CommonINVService.GetAllActiveInputTransactionsByCompanyClient(_companyDefault, companyclient, token, ct);

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

                var listinventorytypes = await _CommonINVService.GetAllActiveInventoryTypes(token, ct);

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

                var listwmsinventorytypes = await _CommonINVService.GetAllActiveWmsInventoryTypes(_companyDefault, companyclient, token, ct);

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

                var listitemstatus = await _CommonINVService.GetAllActiveItemStatus(_companyDefault, companyclient, token, ct);

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


                //we check if the items list have a assigned bin in WMS system

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetail>>(HttpContext.Session.GetString("listPartNumberDetail"));


                var listItemsBinsByProcess = JsonConvert.DeserializeObject<List<BinsToLoadTm>>(HttpContext.Session.GetString("listbinesdistribution"));

                if (listaPartNumber!.Count > 0)
                {

                    List<BinsToLoadTm> listbintoload = new List<BinsToLoadTm>();



                    foreach (var item in listaPartNumber!)
                    {
                        string itemname = string.Empty;

                        if (!item.ItemNumber!.ToString().Contains("ZEBRA"))
                        {
                            itemname = "ZEBRA-" + item.ItemNumber.ToString();
                        }
                        else
                        {
                            itemname = item.ItemNumber.ToString();
                        }
                        var listdistribution = await _CommonINVService.GetInventoryByItem(_companyDefault, companyclient, itemname, token, ct);
                                          

                    }
                }

                return Ok(new
                {
                    message = "OK",
                    error = (string)null,
                    data = objinformation
                });

            }, ct);



        }




        [HttpPost]
        public IActionResult addqtybin(int binid, int typeid, int qtybin, string partno, string tagname, string typename, int lineid, int statusid, string statusname)


        {
            string errormessage = "OK";

            try
            {
                List<BinsToLoadTm> objlist = new List<BinsToLoadTm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadTm>>(HttpContext.Session.GetString("listbinesdistribution"));

                int totalqty = 0;
                int totalshipping = 0;

                if (!partno.ToString().Contains("ZEBRA-"))
                {
                    partno = "ZEBRA-" + partno.Trim();
                }

                if (lista.Count == 0)
                {
                    BinsToLoadTm objdet = new BinsToLoadTm();

                    objdet.Id = lineid;
                    objdet.tagname = tagname;
                    objdet.inventorytype = typeid;
                    objdet.partnumber = partno;
                    objdet.typename = typename;
                    objdet.qty = qtybin;
                    objdet.lineid = lineid;
                    objdet.statusid = statusid;
                    objdet.binid = binid;
                    objdet.statusname = statusname;

                    totalqty += objdet.qty;

                    lista.Add(objdet);
                }
                else
                {
                    var haydatos = false;

                    foreach (var item in lista)
                    {

                        if (item.binid == binid && item.inventorytype == typeid && item.statusid == statusid && item.partnumber == partno)
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
                        objdet.lineid = lineid;
                        objdet.statusid = statusid;
                        objdet.binid = binid;
                        objdet.statusname = statusname;

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


                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetail>>(HttpContext.Session.GetString("listPartNumberDetail"));

                foreach (var detail in listaPartNumber)
                {
                    detail.QuantityPending = 0;
                    detail.thereisdistribution = false;

                    foreach (var item in lista)
                    {
                        if (item.lineid == detail.LineId)
                        {

                            detail.QuantityPending += item.qty;
                            detail.thereisdistribution = true;





                        }

                    }

                    if (detail.LineId == lineid)
                    {
                        totalshipping = detail.QuantityShipped;
                    }

                }

                int contador = 1;
                foreach (var info in lista)
                {
                    info.Id = contador++;



                }

                int totalpendind = totalshipping - totalqty;

                HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(listaPartNumber));

                HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(lista));

                return new ObjectResult(new { status = errormessage, listbines = lista.Where(x => x.lineid == lineid), totalqty = totalqty, totalpendind = totalpendind, thereisdata = thereisdata, listapartnumber = listaPartNumber });



            }
            catch (Exception ex)
            {
                return new ObjectResult(new { status = errormessage });
            }
        }


        [HttpPost]
        public async Task<IActionResult> deleteqtybin(int id, CancellationToken ct)


        {
            string errormessage = "OK";


            return await HandleAsync(async ct =>
            {

                int lineid = 0;

                List<BinsToLoadTm> objlist = new List<BinsToLoadTm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadTm>>(HttpContext.Session.GetString("listbinesdistribution"));

                int totalqty = 0;
                int totalinbins = 0;

                foreach (var item in lista)
                {
                    if (item.Id != id)
                    {
                        lineid = item.lineid;
                        objlist.Add(item);
                    }
                }

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetail>>(HttpContext.Session.GetString("listPartNumberDetail"));

                foreach (var detail in listaPartNumber)
                {
                    detail.QuantityPending = 0;
                    detail.thereisdistribution = false;

                    foreach (var item in objlist)
                    {
                        if (item.lineid == detail.LineId)
                        {

                            detail.QuantityPending += item.qty;
                            detail.thereisdistribution = true;
                            //break;

                        }

                        if (detail.LineId == lineid)
                        {
                            totalqty = detail.QuantityShipped;
                        }
                    }
                }

                // totalqty = totalqty - totalinbins;


                int contador = 1;
                foreach (var info in objlist)
                {
                    info.Id = contador++;
                    if (info.lineid == lineid)
                    {
                        totalinbins += info.qty;
                    }
                }

                totalinbins = totalqty - totalinbins;

                HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(listaPartNumber));

                HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(objlist));

                return new ObjectResult(new { status = errormessage, listbines = objlist.Where(x => x.lineid == lineid), totalqty = totalqty, totalinbins = totalinbins, listapartnumber = listaPartNumber });

            }, ct);


        }


        [HttpPost]
        public async Task<IActionResult> deleteAllBinsDistribution(int lineid, CancellationToken ct)


        {
            string errormessage = "OK";


            return await HandleAsync(async ct =>
            {

               

                List<BinsToLoadTm> objlist = new List<BinsToLoadTm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadTm>>(HttpContext.Session.GetString("listbinesdistribution"));

                int totalqty = 0;
                int totalinbins = 0;

                foreach (var item in lista)
                {
                    if (item.lineid != lineid)
                    {
                       
                        objlist.Add(item);
                    }
                }

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetail>>(HttpContext.Session.GetString("listPartNumberDetail"));

                foreach (var detail in listaPartNumber)
                {
                    detail.QuantityPending = 0;
                    detail.thereisdistribution = false;

                    foreach (var item in objlist)
                    {
                        if (item.lineid == detail.LineId)
                        {
                            detail.QuantityPending += item.qty;
                            detail.thereisdistribution = true;
                        }

                        if (detail.LineId == lineid)
                        {
                            totalqty = detail.QuantityShipped;
                        }
                    }
                }


                int contador = 1;
                foreach (var info in objlist)
                {
                    info.Id = contador++;
                    if (info.lineid == lineid)
                    {
                        totalinbins += info.qty;
                    }
                }

                totalinbins = totalqty - totalinbins;

                HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(listaPartNumber));

                HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(objlist));

                return new ObjectResult(new { status = errormessage, listbines = objlist.Where(x => x.lineid == lineid), totalqty = totalqty, totalinbins = totalinbins, listapartnumber = listaPartNumber });

            }, ct);


        }


        /// <summary>
        /// Get current inventory for a client company and part number
        /// </summary>
        /// <param name="companyclient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>

        [HttpPost]

        public async Task<IActionResult> checkInventoryByPartNumber(string companyclient, string partnumber, int lineid, CancellationToken ct)
        {

            var token = GetToken();

            // partnumber = "ZEBRA-16H.062SC.0004";


            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadTm>>(HttpContext.Session.GetString("listbinesdistribution"));

                string partnumberzeb = partnumber.Contains("ZEBRA") ? "ZEBRA-" + partnumber.Trim() : partnumber.Trim();


                var listinventory = await _CommonINVService.GetInventoryByItem(_companyDefault, companyclient, partnumberzeb, token, ct);

                return new ObjectResult(new { status = listinventory.Error, listinventory = listinventory.Data, listbines = lista.Where(x => x.lineid == lineid) });

            }, ct);


        }


        [HttpPost]

        public async Task<IActionResult> BinDistributionDefault(string companyclientid, int typeid, string typename, int statusid, string statusname, CancellationToken ct)
        {

            List<BinsToLoadTm> newListDistribution = new List<BinsToLoadTm>();

            string errormessage = string.Empty;

            var token = GetToken();

            if (token == null)
                return RedirectToLogin();
            return await HandleAsync(async ct =>
            {

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetail>>(HttpContext.Session.GetString("listPartNumberDetail"));

                foreach (var detail in listaPartNumber!)
                {

                    detail.QuantityPending = 0;

                    string itemsel = string.Empty;

                    if (!itemsel.ToString().Contains("ZEBRA"))
                    {
                        itemsel = "ZEBRA-" + detail.ItemNumber!.Trim();
                    }

                    var listdist = await _CommonINVService.GetItemBinsDistribution(_companyDefault, companyclientid, itemsel, token, ct);

                    errormessage = listdist.Message.Trim();

                    if (listdist.Data.Count > 0)
                    {

                        foreach (var item in listdist.Data)
                        {

                            BinsToLoadTm objdet = new BinsToLoadTm();

                            objdet.Id = detail.Id;
                            objdet.tagname = item.TagName;
                            objdet.inventorytype = typeid;
                            objdet.partnumber = item.Itemid;
                            objdet.typename = typename;
                            objdet.qty = detail.QuantityShipped;
                            objdet.lineid = detail.LineId;
                            objdet.statusid = statusid;
                            objdet.binid = item.BinesId;
                            objdet.statusname = statusname;

                            newListDistribution.Add(objdet);

                            detail.thereisdistribution = true;


                        }

                        detail.QuantityPending = detail.QuantityShipped;
                    }

                }

                int contador = 1;
                foreach (var info in newListDistribution)
                {
                    info.Id = contador++;

                }

                HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(newListDistribution));

                HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(listaPartNumber));

                return new ObjectResult(new { status = errormessage, listapartnumber = listaPartNumber });

            }, ct);
            //HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(listaPartNumber));



            //   return new ObjectResult(new { status = errormessage, listbines = lista.Where(x => x.lineid == lineid), totalqty = totalqty, thereisdata = thereisdata, listapartnumber = listaPartNumber });

        }
    }
}

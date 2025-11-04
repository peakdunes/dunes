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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.UI.Controllers.Inventory.ASN
{
    public class ASNController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IASNService _ASNService;
        private readonly ICommonINVService _CommonINVService;
        private readonly IConfiguration _config;
        private readonly int _companyDefault;
        private readonly string _typeDocument = "ASN";


        public ASNController(IHttpClientFactory httpClientFactory, IConfiguration config, IASNService ASNService,
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

            List<BinsToLoadWm> listbinespartno = new List<BinsToLoadWm>();

            HttpContext.Session.SetString("listbinesdistribution", JsonConvert.SerializeObject(listbinespartno));



            return await HandleAsync(async ct =>
            {
                ASNWm objdto = new ASNWm { asnHdr = new ASNHdrDto(), itemDetail = new List<ASNItemDetailDto>() };


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

                if (infoasn == null || infoasn.Data.asnHdr == null)
                {
                    MessageHelper.SetMessage(this, "danger", "There is not information about this ASN", MessageDisplay.Inline);
                    return View(objresult);
                }


                if (infoasn.Data.itemDetail.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "There is not information detail about this ASN", MessageDisplay.Inline);
                    return View(objresult);
                }

                //check all calls


                //ASN process input and output calls

                var infocall = await _CommonINVService.GetAllCalls(asnnumber, _typeDocument, token, ct);

                if (infocall.Data != null)
                {
                    objresult.CallsRead = infocall.Data;
                }




                //get all active locations

                var locations = await _CommonINVService.GetAllActiveLocationsByCompany(_companyDefault, token, ct);

                if (locations.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", locations.Error, MessageDisplay.Inline);
                    return View(objresult);
                }

                if (locations.Data == null || locations.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not company locations actives", MessageDisplay.Inline);
                    return View(objresult);
                }



                ViewData["locations"] = new SelectList(locations.Data, "Id", "Name");
                //pick process Header and detail information



                //var infobines = await _ASNService.GetAllActiveBinsByCompanyClient(1, "ZEBRA PAR1", token, ct);

                //QUITAR EL 1, 1 PARA COMPANY Y LOCATON
                var listclients = await _CommonINVService.GetAllActiveClientCompaniesByLocation(1, 1, token, ct);

                if (listclients.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", listclients.Error, MessageDisplay.Inline);
                    return View(objresult);
                }

                if (listclients.Data == null || listclients.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not company clients registed", MessageDisplay.Inline);
                    return View(objresult);
                }

                if (infoasn.Data == null || infoasn.Data.itemDetail.Count() <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", "there is not part number detail for this ASN", MessageDisplay.Inline);
                    return View(objresult);
                }


                HttpContext.Session.SetString("listPartNumberDetail", JsonConvert.SerializeObject(infoasn.Data.itemDetail));


                ViewData["companies"] = new SelectList(listclients.Data, "CompanyId", "CompanyId");

                objresult.asdDto = infoasn.Data!;
                //objresult.listcompanyclients = listclients.Data!;

                foreach (var item in objresult.asdDto.itemDetail)
                {
                    item.QuantityPending = (item.QuantityShipped - item.QuantityReceived);

                }


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

            List<WMSTransactionsDto> listinputtransactionsresult = new List<WMSTransactionsDto>();

            List<InventoryTypeDto> listinventorytypesresult = new List<InventoryTypeDto>();

            List<itemstatusDto> listitemstatusresult = new List<itemstatusDto>();

            List<WMSInventoryTypeDto> listwmsinventorytypesresult = new List<WMSInventoryTypeDto>();

            if (_companyDefault <= 0 || string.IsNullOrWhiteSpace(companyclient))
            {
                MessageHelper.SetMessage(this, "danger", "there is not parameter for company defailt", MessageDisplay.Inline);
                return View(objinformation);
            }
            //return BadRequest(new { message = "Parámetros inválidos.", error = "VALIDATION", data = Array.Empty<object>() });

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
                    WMSTransactionsDto objdet = new WMSTransactionsDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();
                    objdet.match = b.match.Trim();
                    objdet.isInput = b.isInput;
                    objdet.isOutput = b.isOutput;

                    listinputtransactionsresult.Add(objdet);
                }

                objinformation.listinputtransactions = listinputtransactionsresult;


                //load ZEBRA inventory types

                var listinventorytypes = await _CommonINVService.GetAllActiveInventoryTypes(token, ct);

                if (listinventorytypes.Data == null || listinventorytypes.Data.Count <= 0)
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

                //load divisions

                var listdivision = await _CommonINVService.GetDivisionByCompanyClient(companyclient, token, ct);

                if (listdivision.Data == null || listdivision.Data.Count <= 0)
                {
                    MessageHelper.SetMessage(this, "danger", $"there is not Division for this for this  company client {companyclient}", MessageDisplay.Inline);
                    return View(listbines);
                }

                foreach (var b in listdivision.Data)
                {
                    TdivisionCompanyDto objdet = new TdivisionCompanyDto();

                    objdet.CompanyDsc = b.CompanyDsc;
                    objdet.DivisionDsc = b.DivisionDsc;

                    objinformation.listtdivisioncompany.Add(objdet);
                }



                //we check if the items list have a assigned bin in WMS system

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetailDto>>(HttpContext.Session.GetString("listPartNumberDetail"));


                var listItemsBinsByProcess = JsonConvert.DeserializeObject<List<BinsToLoadWm>>(HttpContext.Session.GetString("listbinesdistribution"));

                if (listaPartNumber!.Count > 0)
                {

                    List<BinsToLoadWm> listbintoload = new List<BinsToLoadWm>();



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
        public IActionResult addqtybin(int binid, int typeid, int qtybin, string partno, string tagname, string typename, int lineid, int statusid, string statusname, int asnlineid)


        {
            string errormessage = "OK";

            try
            {
                List<BinsToLoadWm> objlist = new List<BinsToLoadWm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadWm>>(HttpContext.Session.GetString("listbinesdistribution"));

                int totalqty = 0;
                int totalshipping = 0;

                if (!partno.ToString().Contains("ZEBRA-"))
                {
                    partno = "ZEBRA-" + partno.Trim();
                }

                if (lista.Count == 0)
                {
                    BinsToLoadWm objdet = new BinsToLoadWm();

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
                    objdet.asnlineid = asnlineid;

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
                        BinsToLoadWm objdet = new BinsToLoadWm();

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
                        objdet.asnlineid = asnlineid;

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


                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetailDto>>(HttpContext.Session.GetString("listPartNumberDetail"));

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

                List<BinsToLoadWm> objlist = new List<BinsToLoadWm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadWm>>(HttpContext.Session.GetString("listbinesdistribution"));

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

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetailDto>>(HttpContext.Session.GetString("listPartNumberDetail"));

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



                List<BinsToLoadWm> objlist = new List<BinsToLoadWm>();

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadWm>>(HttpContext.Session.GetString("listbinesdistribution"));

                int totalqty = 0;
                int totalinbins = 0;

                foreach (var item in lista)
                {
                    if (item.lineid != lineid)
                    {

                        objlist.Add(item);
                    }
                }

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetailDto>>(HttpContext.Session.GetString("listPartNumberDetail"));

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

                var lista = JsonConvert.DeserializeObject<List<BinsToLoadWm>>(HttpContext.Session.GetString("listbinesdistribution"));

                string partnumberzeb = partnumber.Contains("ZEBRA") ? partnumber.Trim() : "ZEBRA-" + partnumber.Trim();


                var listinventory = await _CommonINVService.GetInventoryByItem(_companyDefault, companyclient, partnumberzeb, token, ct);

                return new ObjectResult(new { status = listinventory.Error, listinventory = listinventory.Data, listbines = lista.Where(x => x.lineid == lineid) });

            }, ct);


        }


        [HttpPost]

        public async Task<IActionResult> BinDistributionDefault(string companyclientid, int typeid, string typename, int statusid, string statusname, CancellationToken ct)
        {

            List<BinsToLoadWm> newListDistribution = new List<BinsToLoadWm>();

            string errormessage = string.Empty;

            var token = GetToken();

            if (token == null)
                return RedirectToLogin();
            return await HandleAsync(async ct =>
            {

                var listaPartNumber = JsonConvert.DeserializeObject<List<ASNItemDetailDto>>(HttpContext.Session.GetString("listPartNumberDetail"));

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

                            BinsToLoadWm objdet = new BinsToLoadWm();

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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessASN(string AsnId, string companyclient, int conceptid,
                         int transactionid, string division, string trackingnumberid, string observations, int locationid, CancellationToken ct)
        {


            var listdist1 = JsonConvert.DeserializeObject<List<BinsToLoadWm>>(HttpContext.Session.GetString("listbinesdistribution"));


            if (listdist1 == null)
            {
                return Ok(new { status = $"There is not Bin distribution for this ASN process {AsnId} " });
            }


            var imad = string.Empty;

            return await HandleAsync(async ct =>
            {

                var token = GetToken();
                if (token == null)
                    return RedirectToLogin();

                int IdCompanyClient = 0;

                var infocompanyclient = await _CommonINVService.GetClientCompanyInformationByName(companyclient, token, ct);

                if (infocompanyclient.Data == null)
                {
                    return Ok(new { status = $"{companyclient} this Company client don't have information in our system " });
                }

                IdCompanyClient = infocompanyclient.Data.Id;

                var infotraninput = await _CommonINVService.GetTransactionsTypeById(_companyDefault, companyclient, transactionid, token, ct);

                if (infotraninput == null || infotraninput.Data is null)
                {
                    return Ok(new { status = $"Transction type Id {transactionid} not found " });
                }

                if (!infotraninput.Data.isInput)
                {
                    return Ok(new { status = $"Transction type Id {transactionid} is not valid (you need a input type transaction " });
                }


                //int OutPutTransactionId = 0;

                //var infotranoutput = await _CommonINVService.GetAllActiveTransferTransactionsOutputType(_companyDefault, companyclient, token, ct);

                //if (infotranoutput.Data == null || infotranoutput.Data.Count <= 0)
                //{
                //    return Ok(new { status = "Output Transfer transaction not found " });
                //}

                //foreach (var info in infotranoutput.Data)
                //{
                //    if (info.match.ToUpper().Trim() == infotraninput.Data!.match.ToUpper().Trim())
                //    {
                //        OutPutTransactionId = info.Id;

                //    }
                //}
                //if (OutPutTransactionId == 0)
                //{
                //    return Ok(new { status = "Output Transfer transaction match not found " });
                //}



                WMSCreateHeaderTransactionDTO hdr = new WMSCreateHeaderTransactionDTO();

                hdr.Idcompany = _companyDefault;
                hdr.Idtransactionconcept = conceptid;
                hdr.IdUser = string.Empty; //toma el usuario en el API al momento de consumir el endpoint
                hdr.IdUserprocess = string.Empty;
                hdr.Idcompanyclient = IdCompanyClient;
                hdr.Codecompanyclient = companyclient.Trim();
                hdr.Documentreference = AsnId.Trim();
                hdr.Observations = string.IsNullOrEmpty(observations) ? "" : observations.Trim();
                hdr.Iddivision = division;

                var organizationlist = await _CommonINVService.GetAllWareHouseOrganizationByCompanyClient(_companyDefault, companyclient, token, ct);

                if (organizationlist.Data == null || organizationlist.Data.Count <= 0)
                {
                    return Ok(new { status = $"There is not WareHouse Orgnanization for this company client {companyclient} " });

                }

                //var listdist = JsonConvert.DeserializeObject<List<BinPickWm>>(HttpContext.Session.GetString("distributiondetail"));

                //if (listdist == null)
                //{
                //    return Ok(new { status = $"There is not Bin distribution for this pick process {AsnId} " });
                //}

                List<WMSCreateDetailTransactionDTO> Listdetails = new List<WMSCreateDetailTransactionDTO>();

                foreach (var info in listdist1)
                {


                    int iditemsel = 0;
                    //search item information

                    string partnumberzeb = info.partnumber.Contains("ZEBRA") ? info.partnumber.Trim() : "ZEBRA-" + info.partnumber.Trim();

                    var itemInfo = await _CommonINVService.GetByPartNumber(partnumberzeb, token, ct);

                    if (itemInfo == null || itemInfo.Data == null)
                    {
                        return Ok(new { status = $"There is not information for this part number {info.partnumber} " });
                    }


                    iditemsel = itemInfo.Data.Id;

                    WMSCreateDetailTransactionDTO objdetinput = new WMSCreateDetailTransactionDTO();

                    //we need to create one output transaction another input transaction

                    int rackIdInput = 0;
                    int levelIdInput = 0;

                    int rackIdOutput = 0;
                    int levelIdOutput = 0;

                    foreach (var item in organizationlist.Data)
                    {
                        if (item.Idbin == info.binid)
                        {
                            rackIdInput = item.Idrack;
                            levelIdInput = item.Level;
                        }
                      
                    }

                    if (rackIdInput == 0 )
                    {
                        return Ok(new { status = $"There is not WareHouse Orgnanization for bin {info.binid} " });

                    }

                 


                    WMSCreateDetailTransactionDTO objdetoutput = new WMSCreateDetailTransactionDTO();


                    ////input transaction
                    objdetoutput.Idtypetransaction = transactionid;
                    objdetoutput.Idlocation = locationid;
                    objdetoutput.Idtype = info.inventorytype;
                    objdetoutput.Idrack = rackIdInput;
                    objdetoutput.Level = levelIdInput;
                    objdetoutput.Codeitem = partnumberzeb;
                    objdetoutput.Iditem = iditemsel;
                    objdetoutput.TotalQty = info.qty;
                    objdetoutput.Idbin = info.binid;
                    objdetoutput.Idstatus = info.statusid;
                    objdetoutput.Serialid = "";
                    objdetoutput.Idcompany = _companyDefault;
                    objdetoutput.Idcompanyclient = companyclient;
                    objdetoutput.Iddivision = division;
                    objdetoutput.Idenctransaction = 0;

                    Listdetails.Add(objdetoutput);

                }
                NewInventoryTransactionTm objInfoTran = new NewInventoryTransactionTm
                {
                    hdr = hdr,
                    Listdetails = Listdetails
                };

                ProcessAsnRequestTm objInfoASN = new ProcessAsnRequestTm
                {
                    wmsInfo = objInfoTran,
                    listdetail = listdist1 // new List<BinsToLoadWm>() //listdist

                };
                          

               

                var infotran = await _ASNService.ProcessASNTransaction(AsnId, objInfoASN, trackingnumberid, token, ct);


                if (infotran != null)
                {
                    if (infotran.Success)
                    {

                        return new ObjectResult(new { status = "OK" });
                    }
                    else
                    {
                        return new ObjectResult(new { status = $"ASN :{AsnId } do not processed. Error :{infotran.Message}"  });
                    }
                }
                else
                {
                    return new ObjectResult(new { status = $"ASN :{AsnId} do not processed. Error :{infotran.Message}" });


                }
               
            }, ct);


        }

    }
}

using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Helpers;
using DUNES.UI.Services.Inventory.ASN;
using DUNES.UI.Services.Inventory.Common;
using DUNES.UI.Services.Inventory.PickProcess;
using DUNES.UI.Services.Print;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.UI.Controllers.Inventory.PickProcess
{

    public class PickProcessController : BaseController
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IPickProcessUIService _service;
        private readonly ICommonINVUIService _CommonINVService;
        private readonly IPdfService _pdfService;

        public readonly IConfiguration _config;
        public readonly int _companyDefault;

        private readonly string _typeDocument = "PICKPROCESS";


        public PickProcessController(IHttpClientFactory httpClientFactory, IConfiguration config,
            IPickProcessUIService service, ICommonINVUIService CommonINVService
            ,
            IPdfService pdfService
            )
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _service = service;
            _companyDefault = _config.GetValue<int>("companyDefault", 1);
            _CommonINVService = CommonINVService;
            //_pdfService = pdfService;

        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> PickProcessReceiving(int companyid, string companyclientid, string? pickprocessnumber, CancellationToken ct)
        {

            return await HandleAsync(async ct =>
            {
                List<BinPickWm> listdistribution = new List<BinPickWm>();

                List<InputCallsDto> listinput = new List<InputCallsDto>();
                List<OutputCallsDto> listoutput = new List<OutputCallsDto>();

                PickProcessCallsReadDto allcalls = new PickProcessCallsReadDto();

                allcalls.outputCallsList = listoutput;
                allcalls.inputCallsList = listinput;

                PickProcessHdrDto objhdr = new PickProcessHdrDto();
                List<PickProcessItemDetail> objlist = new List<PickProcessItemDetail>();

                List<WMSHdrTransactionDTO> listenctran = new List<WMSHdrTransactionDTO>();
                List<WMSDetailTransactionDTO> listdetailtran = new List<WMSDetailTransactionDTO>();
                List<WMSInventoryMovementDTO> listmovtran = new List<WMSInventoryMovementDTO>();

                WMSTransactionTm wmsobj = new WMSTransactionTm
                {
                    ListHdr = listenctran,
                    ListDetail = listdetailtran,
                    ListInventoryMovement = listmovtran,
                };

                PickProcessRequestDto objresult = new PickProcessRequestDto
                {
                    PickProcessHdr = objhdr,
                    ListItems = objlist,
                    CallsRead = allcalls,
                    ListTransactions = wmsobj

                };


                if (string.IsNullOrWhiteSpace(pickprocessnumber))
                    return View(objresult);

                var token = GetToken();
                if (token == null)
                    return RedirectToLogin();

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

                var infopickProcess = await _service.GetPickProcessAllInfo(pickprocessnumber, token, ct);

                if (infopickProcess.Error != null)
                {
                    MessageHelper.SetMessage(this, "danger", infopickProcess.Error, MessageDisplay.Inline);
                    return View(objresult);
                }

                objresult.PickProcessHdr = infopickProcess.Data!.PickProcessHdr;
                objresult.ListItems = infopickProcess.Data.ListItems;

                //if (objresult.PickProcessHdr.OutConsReqsId > 0)
                //{

                //pick process input and output calls

                var infocall = await _CommonINVService.GetAllCalls(pickprocessnumber, _typeDocument, token, ct);

                if (infocall.Data != null)
                {
                    objresult.CallsRead = infocall.Data;
                }
                //}

                //pick process WMS Inventory transctions



                var infoWMSTransactions = await _service.GetAllTransactionByDocumentNumber(_companyDefault, "abc", pickprocessnumber, token, ct);

                if (infoWMSTransactions.Data != null)
                {

                    if (infoWMSTransactions.Data.ListHdr.Count > 0)
                    {
                        objresult.ListTransactions.ListHdr = infoWMSTransactions.Data.ListHdr;
                    }

                    if (infoWMSTransactions.Data.ListDetail.Count > 0)
                    {
                        objresult.ListTransactions.ListDetail = infoWMSTransactions.Data.ListDetail;
                    }
                    if (infoWMSTransactions.Data.ListInventoryMovement.Count > 0)
                    {
                        objresult.ListTransactions.ListInventoryMovement = infoWMSTransactions.Data.ListInventoryMovement;
                    }
                }

                //pick process ServTrack tables


                var infoServtrak = await _service.GetAllTablesOrderRepairCreatedByPickProcessAsync(Convert.ToString(infopickProcess.Data!.PickProcessHdr.ConsignRequestID), token, ct);

                if (infoServtrak.Data != null)
                {

                    objresult.OrderRepair = infoServtrak.Data;
                }


                //ZEBRA inventory movement

                DateTime DateMovInventory = DateTime.Now;

                if (infopickProcess.Data.PickProcessHdr.OutConsReqsId > 0)
                {
                    if (infopickProcess.Data.PickProcessHdr.DateTimeConfirmed != null)
                    {
                        DateMovInventory = infopickProcess.Data.PickProcessHdr.DateTimeConfirmed;

                        DateMovInventory = Convert.ToDateTime("09/20/2025");
                    }

                    var infozebramov = await _CommonINVService.GetAllInventoryTransactionsByDocumentStartDate(pickprocessnumber, DateMovInventory, token, ct);

                    if (infozebramov.Data != null)
                    {
                        objresult.ListInvMovZebra = infozebramov.Data;
                    }


                    //ZEBRA Pick Response CALL



                }
                HttpContext.Session.SetString("pickProcessDetail", JsonConvert.SerializeObject(infopickProcess.Data.ListItems));

                HttpContext.Session.SetString("distributiondetail", JsonConvert.SerializeObject(listdistribution));


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

                var listinventory = await _CommonINVService.GetInventoryByItem(_companyDefault, companyclient, partnumberzeb, token, ct);

                return new ObjectResult(new { status = listinventory.Error, listinventory = listinventory.Data, });

            }, ct);


        }


        [HttpPost]

        public async Task<IActionResult> checkInventoryByPartNumberInvType(string companyclient, string partnumber, int lineid, string typeidst, CancellationToken ct)
        {

            int typeid = 0;

            var token = GetToken();


            List<WMSInventoryTypeDto> listInvNoHand = new List<WMSInventoryTypeDto>();

            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {

                var infotype = await _CommonINVService.GetAllActiveWmsInventoryTypes(_companyDefault, companyclient, token, ct);


                if (infotype == null)
                {

                    return new ObjectResult(new { status = "Inventory types not found", continueprocess = false });
                }
                else
                {
                    foreach (var info in infotype.Data)
                    {
                        if (info.Name.ToUpper() == typeidst.ToUpper())
                        {
                            typeid = info.Id;
                        }

                        WMSInventoryTypeDto objdet = new WMSInventoryTypeDto();

                        if (!info.isOnHand)
                        {

                            objdet.Id = info.Id;
                            objdet.Name = info.Name;
                            objdet.isOnHand = info.isOnHand;


                            listInvNoHand.Add(objdet);
                        }
                    }
                }


                if (listInvNoHand.Count <= 0)
                {
                    return new ObjectResult(new { status = "There is not No on-Hand inventory type", continueprocess = false });
                }


                string partnumberzeb = partnumber.Contains("ZEBRA") ? partnumber.Trim() : "ZEBRA-" + partnumber.Trim();

                var listinventory = await _CommonINVService.GetInventoryByItemInventoryType(_companyDefault, companyclient, partnumberzeb, typeid, token, ct);

                var listdist = JsonConvert.DeserializeObject<List<BinPickWm>>(HttpContext.Session.GetString("distributiondetail"));


                //           Idcompany { get; set; }
                //companyclientid { get; set; }
                //locationid { get; set; }


                //binid { get; set; }

                //inventorytypeid { get; set; }

                //statusid { get; set; }

                //rackid { get; set; }
                //rackname { get; set; }
                //qtyreserved { get; set; } = 0;

                foreach (var data in listinventory.Data)
                {

                    foreach (var item in listdist)
                    {

                        //binid { get; set; }

                        //inventorytypeid { get; set; }

                        //statusid { get; set; }


                        if (item.lineid == lineid)
                        {
                            if (data.binid == item.binidout && data.statusid == item.statusid && data.inventorytypeid == item.inventorytypeid)
                            {
                                data.qtyreserved = item.qty;
                            }

                        }

                    }
                }

                if (listinventory.Success && typeid != 0)
                {
                    return new ObjectResult(new { status = listinventory.Error, listinventory = listinventory.Data, continueprocess = true, listinvnohand = listInvNoHand });

                }
                else
                {
                    if (!listinventory.Success)
                    {
                        return new ObjectResult(new { status = listinventory.Error, listinventory = listinventory.Data, continueprocess = false });

                    }
                    else
                    {
                        return new ObjectResult(new { status = "There is not inventory type for this type :" + typeidst, listinventory = listinventory.Data, continueprocess = true });
                    }
                }


            }, ct);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> getAllCompanyActiveByLocation(int locationid, CancellationToken ct)
        {


            if (_companyDefault <= 0 || locationid <= 0)
                return BadRequest(new { message = "Parámetros inválidos.", error = "VALIDATION", data = Array.Empty<object>() });

            // 2) Token (estándar de tu BaseController)
            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                //load company clients by location

                var listcompanies = await _CommonINVService.GetAllActiveClientCompaniesByLocation(_companyDefault, locationid, token, ct);

                if (listcompanies.Data == null || listcompanies.Data.Count <= 0)
                {
                    return Ok(new
                    {
                        message = "There is not company clients for this location",
                        data = ""
                    });

                }
                return Ok(new
                {
                    message = "OK",
                    data = listcompanies.Data
                });

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

                if (listbines.Data == null || listbines.Data.Count <= 0)
                {
                    return Ok(new
                    {
                        status = $"there is not bins created for this  company {companyclient}",
                        data = string.Empty
                    });
                }

                foreach (var b in listbines.Data)
                {
                    WMSBinsDto objdet = new WMSBinsDto
                    {
                       Id = b.Id,
                        Name = b.Name,
                       Idcompany = b.Idcompany,
                       LocationsId = b.LocationsId,
                      
                    };

                  

                    listbinesresult.Add(objdet);
                }

                objinformation.listbines = listbinesresult;

                //load concepts

                var listconcepts = await _CommonINVService.GetAllActiveConceptsByCompanyClient(_companyDefault, companyclient, token, ct);

                if (listconcepts.Data == null || listconcepts.Data.Count <= 0)
                {
                    return Ok(new
                    {
                        status = $"there is not WMS transactions concept created for this company {companyclient}",
                        data = string.Empty
                    });
                }

                foreach (var b in listconcepts.Data)
                {
                    WMSConceptsDto objdet = new WMSConceptsDto();

                    objdet.Id = b.Id;
                    objdet.Name = b.Name.Trim();

                    listconceptsresult.Add(objdet);
                }

                objinformation.listconcepts = listconceptsresult;

                //load divisions

                var listdivision = await _CommonINVService.GetDivisionByCompanyClient(companyclient, token, ct);

                if (listdivision.Data == null || listdivision.Data.Count <= 0)
                {
                    return Ok(new
                    {
                        status = $"there is not Division for this for this  company client {companyclient}",
                        data = string.Empty
                    });

                    //return View(listbines);
                }

                foreach (var b in listdivision.Data)
                {
                    TdivisionCompanyDto objdet = new TdivisionCompanyDto();

                    objdet.CompanyDsc = b.CompanyDsc;
                    objdet.DivisionDsc = b.DivisionDsc;

                    objinformation.listtdivisioncompany.Add(objdet);
                }


                //load input transactions

                var listtransactions = await _CommonINVService.GetAllActiveTransferTransactionsInputType(_companyDefault, companyclient, token, ct);

                if (listtransactions.Data == null || listtransactions.Data.Count <= 0)
                {
                    return Ok(new
                    {
                        status = $"there is not WMS output transactions created for this  company {companyclient}",
                        data = string.Empty
                    });
                }

                objinformation.listinputtransactions = listtransactions.Data;

                return Ok(new
                {
                    status = "OK",
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
                            list.QuantityProcess = 0;
                        }
                    }
                }

                //se cargar a lista de distribucion

                var listdist = JsonConvert.DeserializeObject<List<BinPickWm>>(HttpContext.Session.GetString("distributiondetail"));

                List<BinPickWm> newlistdist = new List<BinPickWm>();

                //elimina toda la distribucion para la linea que va ha adicionar
                //para no duplicar valores

                foreach (var list in listdist!)
                {
                    if (list.lineid != req.lineid)
                    {
                        newlistdist.Add(list);
                    }
                }

                //se insertan las nuevas lineas
                foreach (var info2 in req!.itemslist)
                {
                    newlistdist.Add(info2);

                }

                //se actualiza la cantidad recibida

                foreach (var info in lista)
                {
                    foreach (var info2 in req!.itemslist)
                    {
                        if (Convert.ToInt32(info.LindId) == info2.lineid)
                        {
                            info.QuantityProcess += info2.qty;
                        }
                    }
                }

                HttpContext.Session.SetString("pickProcessDetail", JsonConvert.SerializeObject(lista));

                HttpContext.Session.SetString("distributiondetail", JsonConvert.SerializeObject(newlistdist));

                return Ok(new { status = "OK", lista = lista });
            }, ct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPick(string DeliveryId, string companyclient, int conceptid,
                           int transactionid, string division, string lpnid, string observations, int locationid, CancellationToken ct)
        {

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

                if (infotraninput == null)
                {
                    return Ok(new { status = $"Transction type Id {transactionid} not found " });
                }

                int OutPutTransactionId = 0;

                var infotranoutput = await _CommonINVService.GetAllActiveTransferTransactionsOutputType(_companyDefault, companyclient, token, ct);

                if (infotranoutput.Data == null || infotranoutput.Data.Count <= 0)
                {
                    return Ok(new { status = "Output Transfer transaction not found " });
                }

                foreach (var info in infotranoutput.Data)
                {
                    if (info.match.ToUpper().Trim() == infotraninput.Data!.match.ToUpper().Trim())
                    {
                        OutPutTransactionId = info.Id;

                    }
                }
                if (OutPutTransactionId == 0)
                {
                    return Ok(new { status = "Output Transfer transaction match not found " });
                }



                WMSCreateHeaderTransactionDTO hdr = new WMSCreateHeaderTransactionDTO();

                hdr.Idcompany = _companyDefault;
                hdr.Idtransactionconcept = conceptid;
                hdr.IdUser = string.Empty; //toma el usuario en el API al momento de consumir el endpoint
                hdr.IdUserprocess = string.Empty;
                hdr.Idcompanyclient = IdCompanyClient;
                hdr.Codecompanyclient = companyclient.Trim();
                hdr.Documentreference = DeliveryId.Trim();
                hdr.Observations = string.IsNullOrEmpty(observations) ? "" : observations.Trim();
                hdr.Iddivision = division;

                var organizationlist = await _CommonINVService.GetAllWareHouseOrganizationByCompanyClient(_companyDefault, companyclient, token, ct);

                if (organizationlist.Data == null || organizationlist.Data.Count <= 0)
                {
                    return Ok(new { status = $"There is not WareHouse Orgnanization for this company client {companyclient} " });

                }

                var listdist = JsonConvert.DeserializeObject<List<BinPickWm>>(HttpContext.Session.GetString("distributiondetail"));

                if (listdist == null)
                {
                    return Ok(new { status = $"There is not Bin distribution for this pick process {DeliveryId} " });

                }

                List<WMSCreateDetailTransactionDTO> Listdetails = new List<WMSCreateDetailTransactionDTO>();

                foreach (var info in listdist)
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
                        if (item.Idbin == info.binidin)
                        {
                            rackIdInput = item.Idrack;
                            levelIdInput = item.Level;
                        }
                        if (item.Idbin == info.binidout)
                        {
                            rackIdOutput = item.Idrack;
                            levelIdOutput = item.Level;
                        }
                    }

                    if (rackIdInput == 0 || rackIdOutput == 0)
                    {
                        return Ok(new { status = $"There is not WareHouse Orgnanization for bin {info.binidout} " });

                    }

                    ////ouput transaction
                    objdetinput.Idtypetransaction = OutPutTransactionId;
                    objdetinput.Idlocation = locationid;
                    objdetinput.Idtype = info.typereserveid;
                    objdetinput.Idrack = rackIdInput;
                    objdetinput.Level = levelIdInput;
                    objdetinput.Codeitem = partnumberzeb;
                    objdetinput.Iditem = iditemsel;
                    objdetinput.TotalQty = info.qty;
                    objdetinput.Idbin = info.binidout;
                    objdetinput.Idstatus = info.statusid;
                    objdetinput.Serialid = "";
                    objdetinput.Idcompany = _companyDefault;
                    objdetinput.Idcompanyclient = companyclient;
                    objdetinput.Iddivision = division;
                    objdetinput.Idenctransaction = 0;

                    Listdetails.Add(objdetinput);


                    WMSCreateDetailTransactionDTO objdetoutput = new WMSCreateDetailTransactionDTO();


                    ////input transaction
                    objdetoutput.Idtypetransaction = transactionid;
                    objdetoutput.Idlocation = locationid;
                    objdetoutput.Idtype = info.inventorytypeid;
                    objdetoutput.Idrack = rackIdOutput;
                    objdetoutput.Level = levelIdOutput;
                    objdetoutput.Codeitem = partnumberzeb;
                    objdetoutput.Iditem = iditemsel;
                    objdetoutput.TotalQty = info.qty;
                    objdetoutput.Idbin = info.binidin;
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



                var infotran = await _service.CreatePickProccessTransaction(DeliveryId, objInfoTran, lpnid, token, ct);


                if (!infotran.Success)
                {
                    return Ok(new { status = infotran.Error, infotran = infotran });
                }
                else
                {
                    return Ok(new { status = "OK", infotran = infotran });
                }
            }, ct);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> deleteAllBinsDistribution(int lineid, CancellationToken ct)
        {

            return await HandleAsync(async ct =>
            {

                var lista = JsonConvert.DeserializeObject<List<PickProcessItemDetail>>(HttpContext.Session.GetString("pickProcessDetail"));

                foreach (var list in lista!)
                {
                    if (Convert.ToInt32(list.LindId) == lineid)
                    {
                        list.QuantityProcess = 0;
                    }
                }

                //se cargar a lista de distribucion

                var listdist = JsonConvert.DeserializeObject<List<BinPickWm>>(HttpContext.Session.GetString("distributiondetail"));

                List<BinPickWm> newlistdist = new List<BinPickWm>();

                foreach (var list in listdist!)
                {
                    if (list.lineid != lineid)
                    {
                        newlistdist.Add(list);
                    }
                }

                HttpContext.Session.SetString("pickProcessDetail", JsonConvert.SerializeObject(lista));

                HttpContext.Session.SetString("distributiondetail", JsonConvert.SerializeObject(newlistdist));

                return Ok(new { status = "OK", lista = lista });
            }, ct);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> searchAllPickProcess(DateTime dateSearch, CancellationToken ct)
        {

            var token = GetToken();
            if (token == null)
                return RedirectToLogin();

            return await HandleAsync(async ct =>
            {
                dateSearch = DateTime.Now.AddDays(-300);

                var infolist = await _service.GetAllPickProcessByStartDate(dateSearch,token, ct);

                return Ok(new { status = "OK", lista = infolist.Data });
            }, ct);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> printPackingList(string infocustref, CancellationToken ct)
        //{

        //    var token = GetToken();
        //    if (token == null)
        //        return RedirectToLogin();


        //    return await HandleAsync(async ct =>
        //    {
        //        var info = await _service.GetAllTablesOrderRepairCreatedByPickProcessAsync(infocustref, token, ct);

        //        if (info.Error != null)
        //        {

        //            return new ObjectResult(new { status = info.Error });
        //        }

        //        if (info.Data == null)
        //        {
        //            return new ObjectResult(new { status = "Information not found" });
        //        }

        //        var infoprint = await _pdfService.GeneratePackingList("hlopez", info.Data, ct);

        //        return Ok(new { status = "OK" });
        //    }, ct);
        //}

    }
}

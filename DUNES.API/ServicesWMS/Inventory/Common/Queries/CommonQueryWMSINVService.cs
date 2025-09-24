using DUNES.API.DTOs.B2B;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.ReadModels.WMS;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

namespace DUNES.API.ServicesWMS.Inventory.Common.Queries
{
    /// <summary>
    /// Get all information used for WMS inventory transactions (general queries)
    /// </summary>
    public class CommonQueryWMSINVService : ICommonQueryWMSINVService
    {

        private readonly ICommonQueryWMSINVRepository _repository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>

        public CommonQueryWMSINVService(ICommonQueryWMSINVRepository repository)
        {

            _repository = repository;
        }


        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveBinsByCompanyClient(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Bines>>($"Active bins not found for this company client {companyClient} ");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllBinsByCompanyClient(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Bines>>($"there is not bins for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }

        /// <summary>
        /// get all active transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<Transactionconcepts>>> GetAllActiveTransactionsConcept(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveTransactionsConcept(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Transactionconcepts>>($"there is not active concepts for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// get all transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<Transactionconcepts>>> GetAllTransactionsConcept(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllTransactionsConcept(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Transactionconcepts>>($"there is not active concepts for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }

        /// <summary>
        /// get all active input type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveTransactionsInputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not active input transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach(var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match??"";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;
                    
                objlist.Add(objdet);
            }

            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// get all input type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllTransactionsInputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not input transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }

        /// <summary>
        /// get all active output type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveTransactionsOutputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not active output transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// get all output type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllTransactionsOutputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not output transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveInventoryType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<InventoryTypes>>($"there is not active inventory types for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }


        /// <summary>
        /// Get all On-hand active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllOnHandActiveInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllOnHandActiveInventoryType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<InventoryTypes>>($"there is not active inventory types for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }

        /// <summary>
        /// Get all Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllInventoryType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<InventoryTypes>>($"there is not inventory types for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }

        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatus(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveItemStatus(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Itemstatus>>($"there is not item status actives for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }

        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemstatus>>> GetAllItemStatus(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllItemStatus(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Itemstatus>>($"there is not item status for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetOnHandInventoryByItem(int companyid, string companyClient, string partnumber, CancellationToken ct)
        {

            List<WMSInventoryDetailByPartNumberDto> objlist = new List<WMSInventoryDetailByPartNumberDto>();

            var info = await _repository.GetOnHandInventoryByItem(companyid, companyClient, partnumber, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSInventoryDetailByPartNumberDto>>($"there is not inventory for this company client {companyClient} and this part number {partnumber}");
            }

            foreach (var item in info)
            {
                WMSInventoryDetailByPartNumberDto objdet = new WMSInventoryDetailByPartNumberDto();

                objdet.Idcompany = item.Idcompany;
                objdet.companyclientid = item.Idcompanyclient!;
                objdet.locationid = item.Idlocation;
                objdet.locationname = item.IdlocationNavigation.Name!;
                objdet.qty = item.TotalQty;
                objdet.binid = item.Idbin;
                objdet.binname = item.IdbinNavigation.TagName!;
                objdet.inventorytypeid = item.Idtype;
                objdet.inventoryname = item.IdtypeNavigation.Name!;
                objdet.statusid = item.Idstatus;
                objdet.statusname = item.IdstatusNavigation.Name!;
                objdet.rackid = item.Idrack;
                objdet.rackname = item.IdrackNavigation.Name!;

                objlist.Add(objdet);

            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }

        /// <summary>
        /// get current inventory for a client company, part number, inventoy type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetOnHandInventoryByItemInventoryType(int companyid, string companyClient, string partnumber, int typeid, CancellationToken ct)
        {

            List<WMSInventoryDetailByPartNumberDto> objlist = new List<WMSInventoryDetailByPartNumberDto>();

            var info = await _repository.GetOnHandInventoryByItemInventoryType(companyid, companyClient, partnumber, typeid, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSInventoryDetailByPartNumberDto>>($"there is not inventory for this company client {companyClient} and this part number {partnumber}");
            }

            foreach (var item in info)
            {
                WMSInventoryDetailByPartNumberDto objdet = new WMSInventoryDetailByPartNumberDto();

                objdet.Idcompany = item.Idcompany;
                objdet.companyclientid = item.Idcompanyclient!;
                objdet.locationid = item.Idlocation;
                objdet.locationname = item.IdlocationNavigation.Name!;
                objdet.qty = item.TotalQty;
                objdet.binid = item.Idbin;
                objdet.binname = item.IdbinNavigation.TagName!;
                objdet.inventorytypeid = item.Idtype;
                objdet.inventoryname = item.IdtypeNavigation.Name!;
                objdet.statusid = item.Idstatus;
                objdet.statusname = item.IdstatusNavigation.Name!;
                objdet.rackid = item.Idrack;
                objdet.rackname = item.IdrackNavigation.Name!;

                objlist.Add(objdet);

            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }


        /// <summary>
        /// Gell Item Bins distribution
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Itemsbybin>>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber, CancellationToken ct)
        {

            var info = await _repository.GetItemBinsDistribution(companyid, companyClient, partnumber, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Itemsbybin>>($"there is not distribution for this company client {companyClient} and this part number {partnumber}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// Get all transaction associated to Document Number (ASN, Pick Process, Repair ID)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public async Task<ApiResponse<WMSTransactionTm>> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, CancellationToken ct)
        {
            var info = await _repository.GetAllTransactionByDocumentNumber(companyid, companyClient, DocumentNumber, ct);

            if (info == null)
                return ApiResponseFactory.NotFound<WMSTransactionTm>($"there is not WMS transaction for this company {companyClient} and this document {DocumentNumber}");

          

            WMSTransactionTm objresponse = new WMSTransactionTm();

            foreach (var item in info.ListHdr)
            {
                WMSHdrTransactionDTO objhdr = new WMSHdrTransactionDTO();

                objhdr.Id = item.Id;
                objhdr.Idcompany = item.Idcompany;
                objhdr.CompanyName = string.IsNullOrEmpty(item.IdcompanyNavigation.Name) ? "" : item.IdcompanyNavigation.Name.Trim();
                objhdr.Idtransactionconcept = item.Idtransactionconcept;
                objhdr.conceptName = string.IsNullOrEmpty(item.IdtransactionconceptNavigation.Name) ? "" : item.IdtransactionconceptNavigation.Name.Trim();
                objhdr.IdUser = item.IdUser;
                objhdr.Datecreated = item.Datecreated;
                objhdr.Processed = item.Processed;
                objhdr.IdUserprocess = item.IdUserprocess;
                objhdr.Idcompanyclient = item.Idcompanyclient;
                objhdr.Dateprocessed = item.Dateprocessed;
                objhdr.Documentreference = item.Documentreference;
                objhdr.Observations = item.Observations;
                objhdr.Iddivision = item.Iddivision;

                objresponse.ListHdr.Add(objhdr);

            }

            foreach (var item in info.ListDetail)
            {
                WMSDetailTransactionDTO objdet = new WMSDetailTransactionDTO();

                objdet.Id = item.Id;
                objdet.Idtypetransaction = item.Idtypetransaction;
                objdet.typetransactionName = string.IsNullOrEmpty(item.IdtypetransactionNavigation.Name) ? "" : item.IdtypetransactionNavigation.Name.Trim();
                objdet.Idlocation = item.Idlocation;
                objdet.locationName = string.IsNullOrEmpty(item.IdlocationNavigation.Name) ? "" : item.IdlocationNavigation.Name.Trim();
                objdet.Idtype = item.Idtype;
                objdet.typeName = string.IsNullOrEmpty(item.IdtypeNavigation.Name) ? "" : item.IdtypeNavigation.Name.Trim();
                objdet.Idrack = item.Idrack;
                objdet.rackName = string.IsNullOrEmpty(item.IdrackNavigation.Name) ? "" : item.IdrackNavigation.Name.Trim();
                objdet.Level = item.Level;
                objdet.Iditem = item.Iditem;
                objdet.TotalQty = item.TotalQty;
                objdet.Idbin = item.Idbin;
                objdet.binName = string.IsNullOrEmpty(item.IdbinNavigation.TagName) ? "" : item.IdbinNavigation.TagName.Trim();
                objdet.Idstatus = item.Idstatus;
                objdet.statusName = string.IsNullOrEmpty(item.IdstatusNavigation.Name) ? "" : item.IdstatusNavigation.Name.Trim();
                objdet.Serialid = item.Serialid;
                objdet.Idcompany = item.Idcompany;
                objdet.companyName = string.IsNullOrEmpty(item.IdcompanyNavigation.Name) ? "" : item.IdcompanyNavigation.Name.Trim();
                objdet.Idcompanyclient = item.Idcompanyclient;
                objdet.Iddivision = item.Iddivision;
                objdet.Idenctransaction = item.Idenctransaction;

                objresponse.ListDetail.Add(objdet);
            }

            foreach (var item in info.ListMovement)
            {
                WMSInventoryMovementDTO objmov = new WMSInventoryMovementDTO();

                objmov.Id = item.Id;
                objmov.Idtransactiontype = item.Idtransactiontype;
                objmov.transactionTypeName = string.IsNullOrEmpty(item.IdtransactiontypeNavigation.Name) ? "" : item.IdtransactiontypeNavigation.Name.Trim();
                objmov.Idlocation = item.Idlocation;
                objmov.locationName = string.IsNullOrEmpty(item.IdlocationNavigation.Name) ? "" : item.IdlocationNavigation.Name.Trim();
                objmov.Idtype = item.Idtype;
                objmov.inventoryTypeName = string.IsNullOrEmpty(item.IdtypeNavigation.Name) ? "" : item.IdtypeNavigation.Name.Trim();
                objmov.Idrack = item.Idrack;
                objmov.Level = item.Level;
                objmov.Idbin = item.Idbin;
                objmov.binName = string.IsNullOrEmpty(item.IdbinNavigation.TagName) ? "" : item.IdbinNavigation.TagName.Trim();
                objmov.Iditem = item.Iditem;
                objmov.Idstatus = item.Idstatus;
                objmov.statusName = string.IsNullOrEmpty(item.IdstatusNavigation.Name) ? "" : item.IdstatusNavigation.Name.Trim();
                objmov.Serialid = item.Serialid;
                objmov.Datecreated = item.Datecreated;
                objmov.Qtyinput = item.Qtyinput;
                objmov.Qtyoutput = item.Qtyoutput;
                objmov.Qtybalance = item.Qtybalance;
                objmov.Idcompany = item.Idcompany;
                objmov.companyName = string.IsNullOrEmpty(item.IdcompanyNavigation.Name) ? "" : item.IdcompanyNavigation.Name.Trim();
                objmov.Idcompanyclient = item.Idcompanyclient;
                objmov.IdtransactionHead = item.IdtransactionHead;
                objmov.IdtransactionDetail = item.IdtransactionDetail;
                objmov.Iddivision = item.Iddivision;
                objmov.Createdby = item.Createdby;
                objmov.Idtransactionconcept = item.Idtransactionconcept;
                objmov.conceptName = string.IsNullOrEmpty(item.IdtransactionconceptNavigation.Name) ? "" : item.IdtransactionconceptNavigation.Name.Trim();

                objresponse.ListInventoryMovement.Add(objmov);
            }

            return ApiResponseFactory.Ok(objresponse, "OK");
        }

        /// <summary>
        /// Get All Active Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveTransferTransactionsInputType (companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not input transfer transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// Get All Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllTransferTransactionsInputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not input transfer transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// Get All Active Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllActiveTransferTransactionsOutputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not output transfer transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// Get All Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var info = await _repository.GetAllTransferTransactionsOutputType(companyid, companyClient, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionsDto>>($"there is not output transfer transactions for this company client {companyClient}");
            }

            List<WMSTransactionsDto> objlist = new List<WMSTransactionsDto>();

            foreach (var det in info)
            {

                WMSTransactionsDto objdet = new WMSTransactionsDto();

                objdet.Id = det.Id;
                objdet.Name = det.Name!;
                objdet.match = det.Match ?? "";
                objdet.isInput = det.Isinput;
                objdet.isOutput = det.Isoutput;

                objlist.Add(objdet);
            }


            return ApiResponseFactory.Ok(objlist, "OK");
        }
        /// <summary>
        /// Get one transaction type by ID
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSTransactionsDto>> GetTransactionsTypeById(int companyid, string companyClient, int id, CancellationToken ct)
        {
            var info = await _repository.GetTransactionsTypeById(companyid, companyClient, id, ct);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionsDto>($"there is not transaction for this company client {companyClient} and this id {id}");
            }

            WMSTransactionsDto objdet = new WMSTransactionsDto();

            objdet.Id = info.Id;
            objdet.Name = info.Name!;
            objdet.match = info.Match ?? "";
            objdet.isInput = info.Isinput;
            objdet.isOutput = info.Isoutput;


            return ApiResponseFactory.Ok(objdet, "OK");
        }
    }
}

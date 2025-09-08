using DUNES.API.DTOs.B2B;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
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
        public async Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveBinsByCompanyClient(companyid, companyClient);

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
        public async Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {
            var info = await _repository.GetAllBinsByCompanyClient(companyid, companyClient);

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
        public async Task<ApiResponse<List<Transactionconcepts>>> GetAllActiveTransactionsConcept(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveTransactionsConcept(companyid, companyClient);

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
        public async Task<ApiResponse<List<Transactionconcepts>>> GetAllTransactionsConcept(int companyid, string companyClient)
        {
            var info = await _repository.GetAllTransactionsConcept(companyid, companyClient);

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
        public async Task<ApiResponse<List<Transactiontypes>>> GetAllActiveTransactionsInputType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveTransactionsInputType(companyid, companyClient);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Transactiontypes>>($"there is not active input transactions for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// get all input type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<Transactiontypes>>> GetAllTransactionsInputType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllTransactionsInputType(companyid, companyClient);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Transactiontypes>>($"there is not input transactions for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }

        /// <summary>
        /// get all active output type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<Transactiontypes>>> GetAllActiveTransactionsOutputType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveTransactionsOutputType(companyid, companyClient);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Transactiontypes>>($"there is not active output transactions for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// get all output type transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<Transactiontypes>>> GetAllTransactionsOutputType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllTransactionsOutputType(companyid, companyClient);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Transactiontypes>>($"there is not output transactions for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveInventoryType(companyid, companyClient);

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
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllOnHandActiveInventoryType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllOnHandActiveInventoryType(companyid, companyClient);

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
        public async Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryType(int companyid, string companyClient)
        {
            var info = await _repository.GetAllInventoryType(companyid, companyClient);

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
        public async Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatus(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveItemStatus(companyid, companyClient);

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
        public async Task<ApiResponse<List<Itemstatus>>> GetAllItemStatus(int companyid, string companyClient)
        {
            var info = await _repository.GetAllItemStatus(companyid, companyClient);

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
        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetOnHandInventoryByItem(int companyid, string companyClient, string partnumber)
        {

            List<WMSInventoryDetailByPartNumberDto> objlist = new List<WMSInventoryDetailByPartNumberDto>();

            var info = await _repository.GetOnHandInventoryByItem(companyid, companyClient, partnumber);

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
        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetOnHandInventoryByItemInventoryType(int companyid, string companyClient, string partnumber, int typeid)
        {

            List<WMSInventoryDetailByPartNumberDto> objlist = new List<WMSInventoryDetailByPartNumberDto>();

            var info = await _repository.GetOnHandInventoryByItemInventoryType(companyid, companyClient, partnumber, typeid);

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
        public async Task<ApiResponse<List<Itemsbybin>>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber)
        {
     
            var info = await _repository.GetItemBinsDistribution(companyid, companyClient, partnumber);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Itemsbybin>>($"there is not distribution for this company client {companyClient} and this part number {partnumber}");
            }
            
            return ApiResponseFactory.Ok(info, "OK");
        }
    }
}

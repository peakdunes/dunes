using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;
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

    }
}

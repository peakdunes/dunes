using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Inventory.Common.Queries
{

    /// <summary>
    /// ALL common WMS queries
    /// </summary>
    public interface ICommonQueryWMSINVService
    {
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient);

        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient);




        /// <summary>
        /// Get all active Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactionconcepts>>> GetAllActiveTransactionsConcept(int companyid, string companyClient);



        /// <summary>
        /// Get all Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactionconcepts>>> GetAllTransactionsConcept(int companyid, string companyClient);



        /// <summary>
        /// Get all active Transactions input transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactiontypes>>> GetAllActiveTransactionsInputType(int companyid, string companyClient);



        /// <summary>
        /// Get all Transactions input for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactiontypes>>> GetAllTransactionsInputType(int companyid, string companyClient);



        /// <summary>
        /// Get all active Transactions output transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactiontypes>>> GetAllActiveTransactionsOutputType(int companyid, string companyClient);



        /// <summary>
        /// Get all Transactions output for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactiontypes>>> GetAllTransactionsOutputType(int companyid, string companyClient);



        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryType(int companyid, string companyClient);



        /// <summary>
        /// Get all Inventory Types for a client company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryType(int companyid, string companyClient);



        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatus(int companyid, string companyClient);



        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemstatus>>> GetAllItemStatus(int companyid, string companyClient);

    }
}

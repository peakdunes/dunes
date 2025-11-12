using DUNES.API.DTOs.B2B;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.ReadModels.WMS;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

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
        Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct);

        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct);




        /// <summary>
        /// Get all active Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactionconcepts>>> GetAllActiveTransactionsConcept(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Transactionconcepts>>> GetAllTransactionsConcept(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Transactions input transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransactionsInputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Transactions input for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransactionsInputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Transactions output transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Transactions output for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllActiveInventoryType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Inventory Types for a client company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllInventoryType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all Inventory Types for a client company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<InventoryTypes>>> GetAllOnHandActiveInventoryType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemstatus>>> GetAllActiveItemStatus(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemstatus>>> GetAllItemStatus(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetOnHandInventoryByItem(int companyid, string companyClient, string partnumber, CancellationToken ct);




        /// <summary>
        /// get current inventory for a client company part number inventory type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetOnHandInventoryByItemInventoryType(int companyid, string companyClient, string partnumber, int typeid, CancellationToken ct);




        /// <summary>
        /// Get Item Bins Distribution
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Itemsbybin>>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber, CancellationToken ct);



        /// <summary>
        /// Get all transaction associated to Document Number (ASN, Pick Process, Repair ID)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSTransactionTm>> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, CancellationToken ct);




        /// <summary>
        /// Get All Active Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get All Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get All Active Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);

        /// <summary>
        /// Get All Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get one transaction type by ID
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<ApiResponse<WMSTransactionsDto>> GetTransactionsTypeById(int companyid, string companyClient, int id, CancellationToken ct);



        /// <summary>
        /// Get a transaction by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="transactionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsTransactionsRead>> GetInventoryTransactionById(int companyid, string companyClient, int transactionId, CancellationToken ct);




    }
}

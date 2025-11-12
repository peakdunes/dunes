using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.ReadModels.WMS;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.RepositoriesWMS.Inventory.Common.Queries
{


    /// <summary>
    /// Get all information used for WMS inventory transactions (general queries)
    /// </summary>
    public interface ICommonQueryWMSINVRepository
    {
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactionconcepts>> GetAllActiveTransactionsConcept(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactionconcepts>> GetAllTransactionsConcept(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllActiveInventoryType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Inventory Types for a client company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllInventoryType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllOnHandActiveInventoryType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all active Transactions input transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllActiveTransactionsInputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Transactions input for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllTransactionsInputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all active Transactions output transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllActiveTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all Transactions output for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllTransactionsOutputType(int companyid, string companyClient , CancellationToken ct);


        /// <summary>
        /// Get all active Input Transfer Transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all Input Transfer Transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all active Output Transfer Transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// Get all Output Transfer Transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct);




        /// <summary>
        /// Get Transactions for a client company by Id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<Transactiontypes> GetTransactionsTypeById(int companyid, string companyClient,int id, CancellationToken ct);


        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Itemstatus>> GetAllActiveItemStatus(int companyid, string companyClient, CancellationToken ct);



        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Itemstatus>> GetAllItemStatus(int companyid, string companyClient, CancellationToken ct);


        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<List<Inventorydetail>> GetOnHandInventoryByItem(int companyid, string companyClient, string partnumber , CancellationToken ct);


        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<List<Inventorydetail>> GetOnHandInventoryByItemInventoryType(int companyid, string companyClient, string partnumber, int typeid, CancellationToken ct);



        /// <summary>
        /// get actual item bin distribution 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<List<Itemsbybin>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber, CancellationToken ct);


        /// <summary>
        /// Get all transaction associated to Document Number (ASN, Pick Process, Repair ID)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        Task<WmsTransactionsRead?> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, CancellationToken ct);


        /// <summary>
        /// get a inventory transaction by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="transactionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WmsTransactionsRead?> GetInventoryTransactionById(int companyid, string companyClient, int transactionId, CancellationToken ct);



    }
}

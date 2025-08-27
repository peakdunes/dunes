using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;

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
        Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient);


        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient);



        /// <summary>
        /// Get all active Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactionconcepts>> GetAllActiveTransactionsConcept(int companyid, string companyClient);



        /// <summary>
        /// Get all Transactions concepts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactionconcepts>> GetAllTransactionsConcept(int companyid, string companyClient);



        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllActiveInventoryType(int companyid, string companyClient);



        /// <summary>
        /// Get all Inventory Types for a client company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllInventoryType(int companyid, string companyClient);




        /// <summary>
        /// Get all active Transactions input transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllActiveTransactionsInputType(int companyid, string companyClient);



        /// <summary>
        /// Get all Transactions input for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllTransactionsInputType(int companyid, string companyClient);



        /// <summary>
        /// Get all active Transactions output transactions for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllActiveTransactionsOutputType(int companyid, string companyClient);



        /// <summary>
        /// Get all Transactions output for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Transactiontypes>> GetAllTransactionsOutputType(int companyid, string companyClient);



        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Itemstatus>> GetAllActiveItemStatus(int companyid, string companyClient);



        /// <summary>
        /// Get all item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Itemstatus>> GetAllItemStatus(int companyid, string companyClient);


        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<List<Inventorydetail>> GetInventoryByItem(int companyid, string companyClient, string partnumber);
        

        /// <summary>
        /// get actual item bin distribution 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        Task<List<Itemsbybin>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber);


    }
}

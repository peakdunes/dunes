using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Inventory.Common.Queries
{
    /// <summary>
    /// Get all information used for WMS inventory transactions (general queries)
    /// </summary>
    public class CommonQueryWMSINVRepository : ICommonQueryWMSINVRepository
    {


        private readonly appWmsDbContext _wmscontext;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="wmscontext"></param>
        public CommonQueryWMSINVRepository(appWmsDbContext wmscontext)
        {
            _wmscontext = wmscontext;
        }

        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            var listbines = await _wmscontext.Bines
                .Where(x => x.Idcompany == companyid 
                && x.Idcompanyclient == companyClient 
                && x.Active == true).ToListAsync();

            return listbines;
        }
        

        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {
            var listbines = await _wmscontext.Bines.Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient).ToListAsync();

            return listbines;
        }

        /// <summary>
        /// Get all active transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactionconcepts>> GetAllActiveTransactionsConcept(int companyid, string companyClient)
        {
            var listconcepts = await _wmscontext.Transactionconcepts
               .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               && x.Active == true).ToListAsync();

            return listconcepts ;
        }

        /// <summary>
        /// Get all transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactionconcepts>> GetAllTransactionsConcept(int companyid, string companyClient)
        {
            var listconcepts = await _wmscontext.Transactionconcepts
               .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               ).ToListAsync();

            return listconcepts;
        }
        /// <summary>
        /// Get all input transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllTransactionsInputType(int companyid, string companyClient)
        {
            var listtransactions = await _wmscontext.Transactiontypes
              .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Isinput == true
              ).ToListAsync();

            return listtransactions;
        }
        /// <summary>
        /// Get all active input transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllActiveTransactionsInputType(int companyid, string companyClient)
        {
            var listtransactions = await _wmscontext.Transactiontypes
             .Where(x => x.Idcompany == companyid
             && x.Idcompanyclient == companyClient
             && x.Isinput == true && x.Active == true
             ).ToListAsync();

            return listtransactions;
        }

        /// <summary>
        /// Get all output transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllTransactionsOutputType(int companyid, string companyClient)
        {
            var listtransactions = await _wmscontext.Transactiontypes
            .Where(x => x.Idcompany == companyid
            && x.Idcompanyclient == companyClient
            && x.Isoutput == true
            ).ToListAsync();

            return listtransactions;
        }
        /// <summary>
        /// Get all active output transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllActiveTransactionsOutputType(int companyid, string companyClient)
        {
            var listtransactions = await _wmscontext.Transactiontypes
            .Where(x => x.Idcompany == companyid
            && x.Idcompanyclient == companyClient
            && x.Isoutput == true && x.Active == true
            ).ToListAsync();

            return listtransactions;
        }
        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllActiveInventoryType(int companyid, string companyClient)
        {
            var listinventorytype = await _wmscontext.InventoryTypes
          .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Active == true
          ).ToListAsync();

            return listinventorytype;
        }
        /// <summary>
        /// Get all Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllInventoryType(int companyid, string companyClient)
        {
            var listinventorytype = await _wmscontext.InventoryTypes
               .Where(x => x.Idcompany == companyid
                   && x.Idcompanyclient == companyClient
                   
               ).ToListAsync();

                    return listinventorytype;
        }


        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllActiveItemStatus(int companyid, string companyClient)
        {
            var listitemstatus = await _wmscontext.Itemstatus
               .Where(x => x.Idcompany == companyid
                   && x.Idcompanyclient == companyClient
                   && x.Active == true
               ).ToListAsync();

            return listitemstatus;
        }


        /// <summary>
        /// Get all item stauts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllItemStatus(int companyid, string companyClient)
        {
            var listitemstatus = await _wmscontext.Itemstatus
               .Where(x => x.Idcompany == companyid
                   && x.Idcompanyclient == companyClient

               ).ToListAsync();

            return listitemstatus;
        }
        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        public async Task<List<Inventorydetail>> GetInventoryByItem(int companyid, string companyClient, string partnumber)
        {
            var listinventory = await _wmscontext.Inventorydetail
               .Include(x => x.IdbinNavigation)
               .Include(x => x.IdstatusNavigation)
               .Include(x => x.IdtypeNavigation)
               .Include(x => x.IdlocationNavigation)
               .Include(x => x.IdrackNavigation)
               .Include(x => x.IdlocationNavigation)
              .Where(x => x.Idcompany == companyid
                  && x.Idcompanyclient == companyClient
                  && x.Iditem == partnumber

              ).ToListAsync();

            return listinventory;
        }
        /// <summary>
        /// Items bin distribution
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        public async Task<List<Itemsbybin>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber)
        {
            var listdistribution = await _wmscontext.Itemsbybin
             .Where(x => x.CompanyId == companyid
                 && x.Idcompanyclient == companyClient
                 && x.Itemid == partnumber

             ).ToListAsync();

            return listdistribution;
        }
    }
}

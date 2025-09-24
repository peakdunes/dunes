using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.ModelsWMS.Transactions;
using DUNES.API.ReadModels.WMS;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        public async Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {

            var listbines = await _wmscontext.Bines
            .Where(x => x.Idcompany == companyid
            && x.Idcompanyclient == companyClient
            && x.Active == true).ToListAsync(ct);

            return listbines;


        }


        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var listbines = await _wmscontext.Bines.Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient).ToListAsync(ct);

            return listbines;
        }

        /// <summary>
        /// Get all active transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactionconcepts>> GetAllActiveTransactionsConcept(int companyid, string companyClient, CancellationToken ct)
        {
            var listconcepts = await _wmscontext.Transactionconcepts
               .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               && x.Active == true).ToListAsync(ct);

            return listconcepts;
        }

        /// <summary>
        /// Get all transactions concepts
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactionconcepts>> GetAllTransactionsConcept(int companyid, string companyClient, CancellationToken ct)
        {
            var listconcepts = await _wmscontext.Transactionconcepts
               .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               ).ToListAsync(ct);

            return listconcepts;
        }
        /// <summary>
        /// Get all input transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
              .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Isinput == true
              ).ToListAsync(ct);

            return listtransactions;
        }
        /// <summary>
        /// Get all active input transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllActiveTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
             .Where(x => x.Idcompany == companyid
             && x.Idcompanyclient == companyClient
             && x.Isinput == true && x.Active == true
             ).ToListAsync(ct);

            return listtransactions;
        }

        /// <summary>
        /// Get all output transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
            .Where(x => x.Idcompany == companyid
            && x.Idcompanyclient == companyClient
            && x.Isoutput == true
            ).ToListAsync(ct);

            return listtransactions;
        }
        /// <summary>
        /// Get all active output transactions type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Transactiontypes>> GetAllActiveTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
            .Where(x => x.Idcompany == companyid
            && x.Idcompanyclient == companyClient
            && x.Isoutput == true && x.Active == true
            ).ToListAsync(ct);

            return listtransactions;
        }
        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllActiveInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            var listinventorytype = await _wmscontext.InventoryTypes
          .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Active == true
          ).ToListAsync(ct);

            return listinventorytype;
        }

        /// <summary>
        /// Get all active Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllOnHandActiveInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            var listinventorytype = await _wmscontext.InventoryTypes
          .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Active == true
              && x.IsOnHand == true
          ).ToListAsync(ct);

            return listinventorytype;
        }


        /// <summary>
        /// Get all Inventory Types for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllInventoryType(int companyid, string companyClient, CancellationToken ct)
        {
            var listinventorytype = await _wmscontext.InventoryTypes
               .Where(x => x.Idcompany == companyid
                   && x.Idcompanyclient == companyClient

               ).ToListAsync(ct);

            return listinventorytype;
        }


        /// <summary>
        /// Get all active item status for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllActiveItemStatus(int companyid, string companyClient, CancellationToken ct)
        {
            var listitemstatus = await _wmscontext.Itemstatus
               .Where(x => x.Idcompany == companyid
                   && x.Idcompanyclient == companyClient
                   && x.Active == true
               ).ToListAsync(ct);

            return listitemstatus;
        }


        /// <summary>
        /// Get all item stauts for a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllItemStatus(int companyid, string companyClient, CancellationToken ct)
        {
            var listitemstatus = await _wmscontext.Itemstatus
               .Where(x => x.Idcompany == companyid
                   && x.Idcompanyclient == companyClient

               ).ToListAsync(ct);

            return listitemstatus;
        }
        /// <summary>
        /// get current inventory for a client company part number
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        public async Task<List<Inventorydetail>> GetOnHandInventoryByItem(int companyid, string companyClient, string partnumber, CancellationToken ct)
        {

            var listinventory = await (from enc in _wmscontext.Inventorydetail
            .Include(x => x.IdbinNavigation)
            .Include(x => x.IdstatusNavigation)
            .Include(x => x.IdtypeNavigation)
            .Include(x => x.IdlocationNavigation)
            .Include(x => x.IdrackNavigation)
            .Include(x => x.IdlocationNavigation)
           .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               && x.Iditem == partnumber
           )
                                       join det in _wmscontext.InventoryTypes.Where(x => x.IsOnHand == true) on enc.Idtype equals det.Id
                                       select (enc)).ToListAsync(ct);


            return listinventory;
        }

        /// <summary>
        /// get current inventory for a client company part number inventory type
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>

        public async Task<List<Inventorydetail>> GetOnHandInventoryByItemInventoryType(int companyid, string companyClient, string partnumber, int typeid, CancellationToken ct)
        {

            var listinventory = await (from enc in _wmscontext.Inventorydetail
           .Include(x => x.IdbinNavigation)
           .Include(x => x.IdstatusNavigation)
           .Include(x => x.IdtypeNavigation)
           .Include(x => x.IdlocationNavigation)
           .Include(x => x.IdrackNavigation)
           .Include(x => x.IdlocationNavigation)
          .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Iditem == partnumber
              && x.Idtype == typeid
          )
                                       join det in _wmscontext.InventoryTypes
                                       .Where(x => x.IsOnHand == true) on enc.Idtype equals det.Id
                                       select (enc)).ToListAsync(ct);




            return listinventory;
        }

        /// <summary>
        /// Items bin distribution
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="partnumber"></param>
        /// <returns></returns>
        public async Task<List<Itemsbybin>> GetItemBinsDistribution(int companyid, string companyClient, string partnumber, CancellationToken ct)
        {
            var listdistribution = await (from enc in _wmscontext.Itemsbybin.Where(x => x.CompanyId == companyid
                 && x.Idcompanyclient == companyClient
                 && x.Itemid == partnumber)
                                          join det in _wmscontext.Bines on enc.BinesId equals det.Id
                                          select new Itemsbybin
                                          {
                                              Id = enc.Id,
                                              CompanyId = companyid,
                                              Idcompanyclient = companyClient,
                                              BinesId = enc.BinesId,
                                              Itemid = enc.Itemid,
                                              tagName = det.TagName!
                                          }).ToListAsync(ct);

            return listdistribution;
        }
        /// <summary>
        /// Get all transaction associated to Document Number (ASN, Pick Process, Repair ID)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public async Task<WmsTransactionsRead?> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, CancellationToken ct)
        {


            WmsTransactionsRead objresponse = new WmsTransactionsRead();

            var infoenctran = await _wmscontext.InventorytransactionHdr
                .Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient &&
                x.Documentreference == DocumentNumber).ToListAsync(ct);


            if (infoenctran.Count == 0)
                return null;

            List<int> listenc = new List<int>();


            if (infoenctran.Count > 0)
            {
                objresponse.ListHdr = infoenctran;

                foreach (var item in objresponse.ListHdr)
                {
                    listenc.Add(item.Id);
                }


                var infodetail = await _wmscontext.InventorytransactionDetail.Where(x => listenc.Contains(x.Idenctransaction)).ToListAsync();

                if (infodetail.Count > 0)
                {
                    objresponse.ListDetail = infodetail;
                }

                var infomov = await _wmscontext.Inventorymovement.Where(x => listenc.Contains(x.IdtransactionHead)).ToListAsync();

                if (infomov.Count > 0)
                {
                    objresponse.ListMovement = infomov;
                }
            }

            return objresponse;
        }
        /// <summary>
        /// Get All Active Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Transactiontypes>> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
               .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               && x.Isinput == true && x.Active == true && x.Match != "" && x.Match != null
               ).ToListAsync(ct);
            return listtransactions;
        }

        /// <summary>
        /// Get All Input Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Transactiontypes>> GetAllTransferTransactionsInputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
              .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Isinput == true && x.Match != "" && x.Match != null
              ).ToListAsync(ct);
            return listtransactions;
        }


        /// <summary>
        /// Get All Active Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Transactiontypes>> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
               .Where(x => x.Idcompany == companyid
               && x.Idcompanyclient == companyClient
               && x.Isoutput == true && x.Active == true && x.Match != "" && x.Match != null
               ).ToListAsync(ct);
            return listtransactions;
        }

        /// <summary>
        /// Get All Output Type Transfer transactions
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<Transactiontypes>> GetAllTransferTransactionsOutputType(int companyid, string companyClient, CancellationToken ct)
        {
            var listtransactions = await _wmscontext.Transactiontypes
              .Where(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient
              && x.Isoutput == true && x.Match != "" && x.Match != null
              ).ToListAsync(ct);
            return listtransactions;
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
        public async Task<Transactiontypes> GetTransactionsTypeById(int companyid, string companyClient, int id, CancellationToken ct)
        {
            var uniquetransaction = await _wmscontext.Transactiontypes
              .FirstOrDefaultAsync(x => x.Idcompany == companyid
              && x.Idcompanyclient == companyClient && x.Id == id, ct);


            return uniquetransaction;
        }
    }
}

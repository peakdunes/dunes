using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.TemporalModels;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Inventory.Transactions
{
    /// <summary>
    /// All WMS Inventory transactions
    /// </summary>
    public class TransactionsWMSINVRepository : ITransactionsWMSINVRepository
    {

        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="wmscontext"></param>
        public TransactionsWMSINVRepository(appWmsDbContext wmscontext)
        {
            _wmscontext = wmscontext;
        }
        /// <summary>
        /// Create a WMS inventory transaction (header and details)
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        public async Task<int> CreateInventoryTransaction(NewInventoryTransactionTm objdata)
        {
            await using var transaction = await _wmscontext.Database.BeginTransactionAsync();
            try
            {
                InventorytransactionHdr objhdr = new InventorytransactionHdr();

                objhdr.Id = 0;
                objhdr.Idcompany = objdata.hdr.Idcompany;
                objhdr.Idtransactionconcept = objdata.hdr.Idcompany;
                objhdr.IdUser = objdata.hdr.IdUser;
                objhdr.Datecreated = DateTime.Now;
                objhdr.Processed = false;
                objhdr.IdUserprocess = null;
                objhdr.Idcompanyclient = objdata.hdr.Codecompanyclient;
                objhdr.Dateprocessed = DateTime.MinValue;
                objhdr.Documentreference = string.IsNullOrEmpty(objdata.hdr.Documentreference) ? "" : objdata.hdr.Documentreference;
                objhdr.Observations = string.IsNullOrEmpty(objdata.hdr.Observations) ? "" : objdata.hdr.Observations;
                objhdr.Iddivision = objdata.hdr.Iddivision;

                _wmscontext.InventorytransactionHdr.Add(objhdr);
                await _wmscontext.SaveChangesAsync();

                foreach (var det in objdata.Listdetails)
                {
                    InventorytransactionDetail objdetail = new InventorytransactionDetail();

                    objdetail.Id = 0;
                    objdetail.Idtypetransaction = det.Idtypetransaction;
                    objdetail.Idlocation = det.Idlocation;
                    objdetail.Idtype = det.Idtype;
                    objdetail.Idrack = det.Idrack;
                    objdetail.Level = det.Level;
                    objdetail.Iditem = det.Codeitem;
                    objdetail.TotalQty = det.TotalQty;
                    objdetail.Idbin = det.Idbin;
                    objdetail.Idstatus = det.Idstatus;
                    objdetail.Serialid = det.Serialid;
                    objdetail.Idcompany = det.Idcompany;
                    objdetail.Idcompanyclient = det.Idcompanyclient;
                    objdetail.Iddivision = det.Iddivision;
                    objdetail.Idenctransaction = objhdr.Id;

                    _wmscontext.InventorytransactionDetail.Add(objdetail);
                    await _wmscontext.SaveChangesAsync();
                }

                await transaction.CommitAsync(); 

                return objhdr.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw; // se lo pasas al middleware
            }
        }


    }
}

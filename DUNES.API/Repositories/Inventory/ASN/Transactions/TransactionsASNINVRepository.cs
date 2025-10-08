using DUNES.API.Data;
using DUNES.API.Models.Inventory.ASN;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Services.Auth;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Data;
using System.Linq;

namespace DUNES.API.Repositories.Inventory.ASN.Transactions
{

    /// <summary>
    /// ASN transactions
    /// </summary>
    public class TransactionsASNINVRepository : ITransactionsASNINVRepository
    {


        private readonly AppDbContext _context;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>

        public TransactionsASNINVRepository(AppDbContext context)
        {
            _context = context;

        }

        /// <summary>
        /// UPdate IrReceiptHdr table (_TZEB_B2B_IR_RECEIPT_OUT_HDR_DET_ITEM_Inb_Cons_Reqs_Log) with the Id output call id
        /// </summary>
        /// <param name="Consign_DBKRequestID"></param>
        /// <param name="IRReceiptHDR"></param>
        /// <returns></returns>
        public async Task<bool> UpdateIrReceiptWithOutPutCallId(int Consign_DBKRequestID, int IRReceiptHDR)
        {
            var infotable = await _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.FirstOrDefaultAsync(x => x.Id == IRReceiptHDR);


            if (infotable == null)
            {
                return false;
            }
            else
            {
                infotable.ConsignDbkrequestId = Consign_DBKRequestID;

                _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.Update(infotable);
                await _context.SaveChangesAsync();

                return true;

            }
        }





        /// <summary>
        /// Create the first transaction in receiving ASN Process
        /// </summary>
        /// <param name="DataLog"></param>
        /// <param name="detaillist"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<int> CreateIrReceiptHdrAndDetailLog(AsnReceivedHdrLogRead DataLog, List<BinsToLoadWm> detaillist, CancellationToken ct)
        {

            var header = new TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog
            {
                ConsignDbkrequestId = 0,
                OrgSystemId3pl = DataLog.org3pl,
                TransactionType = DataLog.TransactionType,
                ShipmentNum = DataLog.asnNumber,
                DateTimeInserted = DateTime.Now,
            };

            _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.Add(header);


            var grouped = detaillist
               .GroupBy(x => x.asnlineid)
               .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) })
               .ToList();

            var lineIds = grouped.Select(g => g.lineid).ToList();

            var infoByLine = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
                .Where(x => lineIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.LineNum, ct);

            foreach (var g in grouped)
            {
                if (!infoByLine.TryGetValue(g.lineid, out var infoline)) continue;

                var det = new TzebB2bIrReceiptLineItemTblItemInbConsReqsLog
                {
                    // Relación por navegación (si la tienes):
                    // Header = header,

                    // O por FK (EF resuelve el Id real en SaveChanges):
                    IrReceiptOutHdrDetItemId = header.Id,

                    ShipmentLineId = infoline.ShipmentLineId,
                    LineNum = infoline.LineNum,
                    Quantity = g.qty,
                    UnitOfMeasure = infoline.UnitOfMeasure,
                    InventoryItemId = infoline.InventoryItemId,
                    ItemNumber = infoline.ItemNumber,
                    ReceiptDate = DateOnly.FromDateTime(DateTime.Now),
                    TransactionDate = DateOnly.FromDateTime(DateTime.Now),
                    DateTimeInserted = DateTime.Now,
                    To3plLocatorStatus = DataLog.locator3pl,
                    IsRtvPart = DataLog.IsRtvPart,
                    IsCePart = DataLog.IsCePart
                };

                _context.TzebB2bIrReceiptLineItemTblItemInbConsReqsLog.Add(det);
            }

            await _context.SaveChangesAsync(ct); // EF usa una transacción interna
            return header.Id;

           

        }




        /// <summary>
        /// Record for each receive transaction item detail
        /// </summary>
        /// <param name="detaillist"></param>
        /// <param name="userid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> InsertQtyDetailByAsn(List<BinsToLoadWm> detaillist, string userid, CancellationToken ct)
        {

            var listgroup = detaillist.GroupBy(x => x.asnlineid)
                .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) }).ToList();


            foreach (var item in listgroup)
            {
                TzebB2bAsnLineItemTblItemPartialInbConsReqs objdet = new TzebB2bAsnLineItemTblItemPartialInbConsReqs();

                objdet.Id = 0;
                objdet.AsnLineItemTblItemId = item.lineid;
                objdet.QtyPartial = item.qty;
                objdet.DateTimeSent = DateTime.Now;
                objdet.CallId = 11;
                objdet.Username = userid;

                _context.TzebB2bAsnLineItemTblItemPartialInbConsReqs.Add(objdet);
                await _context.SaveChangesAsync();

            }

            return true;
        }


        /// <summary>
        /// Update ASN detail line by line with a qty received
        /// </summary>
        /// <param name="detaillist"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> UpdateASNDetail(List<BinsToLoadWm> detaillist, CancellationToken ct)
        {
            var listgroup = detaillist.GroupBy(x => x.asnlineid)
              .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) }).ToList();

            foreach (var item in listgroup)
            {
                var infodetline = _context.TzebB2bAsnLineItemTblItemInbConsReqs.FirstOrDefault(x => x.LineNum == item.lineid);

                if (infodetline != null)
                {

                    infodetline.QuantityReceived = item.qty;
                    infodetline.Processed = true;

                    _context.TzebB2bAsnLineItemTblItemInbConsReqs.Update(infodetline);
                    await _context.SaveChangesAsync();
                }

            }

            return true;
        }


    }
}

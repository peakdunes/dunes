using DUNES.API.Data;
using DUNES.API.Models.Inventory.ASN;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Services.Auth;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
        /// Create a start Receiving process for a ASN and return record id
        /// </summary>
        /// <param name="dataLog"></param>
        /// <param name="ct"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        /// 

        //public async Task<int> CreateIrReceiptHdrAndDetailLog(AsnReceivedHdrLogRead dataLog, List<BinsToLoadWm> detailList, CancellationToken ct)
        //{
        //    1) Transacción
        //   await using var tx = await _context.Database.BeginTransactionAsync(ct);

        //    try
        //    {
        //        2) Header
        //       var header = new TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog
        //       {
        //             No asignes Id, EF lo llena
        //            ConsignDbkrequestId = 0,
        //            OrgSystemId3pl = dataLog.org3pl,
        //            TransactionType = dataLog.TransactionType,
        //            ShipmentNum = dataLog.ShipmentNum,  // <--- FIX: antes te auto-asignabas
        //            DateTimeInserted = DateTime.Now
        //       };

        //        _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.Add(header);
        //        await _context.SaveChangesAsync(ct); // Necesario si vas a usar header.Id como FK (si no usas navegación)

        //        3) Prepara detalles(sin SaveChanges dentro del loop)
        //        var grouped = detailList
        //            .GroupBy(x => x.asnlineid)
        //            .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) })
        //            .ToList();

        //        Evita N+1: trae todas las líneas necesarias de una vez
        //        var lineIds = grouped.Select(g => g.lineid).Distinct().ToList();
        //        var infoLines = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
        //            .Where(x => lineIds.Contains(x.LineNum))
        //            .ToListAsync(ct);

        //        var infoByLine = infoLines.ToDictionary(x => x.LineNum);

        //        var detailEntities = new List<TzebB2bIrReceiptLineItemTblItemInbConsReqsLog>();

        //        foreach (var g in grouped)
        //        {
        //            if (!infoByLine.TryGetValue(g.lineid, out var infoline)) continue;

        //            var det = new TzebB2bIrReceiptLineItemTblItemInbConsReqsLog
        //            {
        //                IrReceiptOutHdrDetItemId = header.Id, // FK al header
        //                ShipmentLineId = infoline.ShipmentLineId,
        //                LineNum = infoline.LineNum,
        //                Quantity = g.qty,
        //                UnitOfMeasure = infoline.UnitOfMeasure,
        //                InventoryItemId = infoline.InventoryItemId,
        //                ItemNumber = infoline.ItemNumber,
        //                ReceiptDate = DateOnly.FromDateTime(DateTime.Now),
        //                TransactionDate = DateOnly.FromDateTime(DateTime.Now),
        //                DateTimeInserted = DateTime.Now,
        //                To3plLocatorStatus = dataLog.locator3pl,
        //                IsRtvPart = dataLog.IsRtvPart,
        //                IsCePart = dataLog.IsCePart
        //            };

        //            detailEntities.Add(det);
        //        }

        //        _context.TzebB2bIrReceiptLineItemTblItemInbConsReqsLog.AddRange(detailEntities);

        //        4) Un solo guardado para todos los detalles
        //       await _context.SaveChangesAsync(ct);

        //        5) Commit
        //       await tx.CommitAsync(ct);

        //        return header.Id; // Devuelve el Id del maestro creado
        //    }
        //    catch
        //    {
        //        await tx.RollbackAsync(ct);
        //        throw;
        //    }
        //}

        public async Task<int> CreateIrReceiptHdrAndDetailLog(AsnReceivedHdrLogRead DataLog, List<BinsToLoadWm> detaillist, CancellationToken ct)
        {
            //_TZEB_B2B_IR_RECEIPT_OUT_HDR_DET_ITEM_Inb_Cons_Reqs_Log

            TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog objlog = new TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog();


            objlog.Id = 0;
            objlog.ConsignDbkrequestId = 0;
            objlog.OrgSystemId3pl = DataLog.org3pl;
            objlog.TransactionType = DataLog.TransactionType;
            objlog.ShipmentNum = objlog.ShipmentNum;
            objlog.DateTimeInserted = DateTime.Now;


            _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.Add(objlog);
            await _context.SaveChangesAsync();

            //receipt detail

            var listgroup = detaillist.GroupBy(x => x.asnlineid)
           .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) }).ToList();

            foreach (var item in listgroup)
            {

                var infoline = await _context.TzebB2bAsnLineItemTblItemInbConsReqs.FirstOrDefaultAsync(x => x.LineNum == item.lineid);


                if (infoline != null)
                {

                    TzebB2bIrReceiptLineItemTblItemInbConsReqsLog objlogdet = new TzebB2bIrReceiptLineItemTblItemInbConsReqsLog();

                    objlog.Id = 0;

                    objlogdet.IrReceiptOutHdrDetItemId = objlog.Id;
                    objlogdet.ShipmentLineId = infoline.ShipmentLineId;
                    objlogdet.LineNum = infoline.LineNum;
                    objlogdet.Quantity = item.qty;
                    objlogdet.UnitOfMeasure = infoline.UnitOfMeasure;
                    objlogdet.InventoryItemId = infoline.InventoryItemId;
                    objlogdet.ItemNumber = infoline.ItemNumber;
                    objlogdet.ReceiptDate = DateOnly.FromDateTime(DateTime.Now);
                    objlogdet.TransactionDate = DateOnly.FromDateTime(DateTime.Now);
                    objlogdet.DateTimeInserted = DateTime.Now;
                    objlogdet.To3plLocatorStatus = DataLog.locator3pl;
                    objlogdet.IsRtvPart = DataLog.IsRtvPart;
                    objlogdet.IsCePart = DataLog.IsCePart;

                    _context.TzebB2bIrReceiptLineItemTblItemInbConsReqsLog.Add(objlogdet);
                    await _context.SaveChangesAsync();

                }

            }



            return objlog.Id;

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

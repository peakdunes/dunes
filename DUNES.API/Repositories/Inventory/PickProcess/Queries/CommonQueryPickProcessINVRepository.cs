using DUNES.API.Data;
using DUNES.API.Models.Inventory;
using DUNES.API.ReadModels.B2B;
using DUNES.API.ReadModels.Inventory;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace DUNES.API.Repositories.Inventory.PickProcess.Queries
{
    /// <summary>
    /// All inventory pick process queries
    /// </summary>
    public class CommonQueryPickProcessINVRepository : ICommonQueryPickProcessINVRepository
    {

        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// initialize dbcontext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        public CommonQueryPickProcessINVRepository(AppDbContext context, appWmsDbContext wmscontext)
        {
            _context = context;
            _wmscontext = wmscontext;
        }

        /// <summary>
        /// Displays the 4 tables associated with an Pick Process in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="ConsignRequestId">This parameter is a field in the Pick Process Header</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<OrderRepairFourTablesRead?> GetAllTablesOrderRepairCreatedByPickProcessAsync(string ConsignRequestId, CancellationToken ct)
        {
            OrderRepairFourTablesRead objdto = new OrderRepairFourTablesRead();

            objdto.OrHdr = await _context.TorderRepairHdr.FirstOrDefaultAsync(x => x.CustRef == ConsignRequestId);

            if (objdto.OrHdr == null)
            {
                return null;
            }

            objdto.ItemList = await _context.TorderRepairItems.Where(x => x.RefNo == objdto.OrHdr.RefNo).ToListAsync();
            objdto.ReceivingList = await _context.TorderRepairItemsSerialsReceiving.Where(x => x.RefNo.Equals(objdto.OrHdr.RefNo)).ToListAsync();
            objdto.ShippingList = await _context.TorderRepairItemsSerialsShipping.Where(x => x.RefNo.Equals(objdto.OrHdr.RefNo)).ToListAsync();


            return objdto;
        }

     
        /// <summary>
        /// Get ALl information about Pick Process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<PickProcessRead?> GetPickProcessAllInfo(string DeliveryId, CancellationToken ct)
        {
            var infoHdr = _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog.FirstOrDefault(x => x.DeliveryId == DeliveryId);

            if (infoHdr == null)
            {
                return null;
            }

            var infodetail = await _context.TzebB2bPSoLineItemTblItemInbConsReqsLog.Where(x => x.PSoWoHdrTblItemId == infoHdr.Id).ToListAsync();

            if (infodetail.Count() <= 0)
            {
                return null;
            }

            PickProcessRead objdet = new PickProcessRead
            {
                pickHdr = infoHdr,
                pickdetails = infodetail

            };


            return objdet;
        }
    }
}

using DUNES.API.Data;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Repositories.Inventory.PickProcess.Transactions
{

    /// <summary>
    /// All pick-process transactions
    /// </summary>
    public class TransactionsPickProcessINVRepository : ITransactionsPickProcessINVRepository
    {


        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        /// <summary>
        /// initialize dbcontext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        public TransactionsPickProcessINVRepository(AppDbContext context, appWmsDbContext wmscontext)
        {
            _context = context;
            _wmscontext = wmscontext;
        }


        /// <summary>
        /// create ServTrack Order from pick process delivery
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ServTrackReferenceCreatedDto> CreateServTrackOrderFromPickProcess(string DeliveryId)
        {
            var param = new SqlParameter("@DeliveryId", DeliveryId);


            //var result = await _context.ServTrackReferences
            //    .FromSqlRaw("EXEC [dbo].[_SP_CREATE_SERVTRACK_ORDERS_FROM_DELIVERYID] @DeliveryId", param)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync();

            var result = (await _context.ServTrackReferences
                .FromSqlInterpolated($"EXEC [dbo].[_SP_CREATE_SERVTRACK_ORDERS_FROM_DELIVERYID] {DeliveryId}")
                .AsNoTracking()
                .ToListAsync())
                .FirstOrDefault();

            return result;
        }

        /// <summary>
        /// create a pick process call a return call id
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        public async Task<int> CreatePickProcessCall(string DeliveryId)
        {

            //we need to create this new SP en production database

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "_SPZEB_B2B_Insert_Stage_Call_For_DeliveryID_ReturnID";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var param = command.CreateParameter();
            param.ParameterName = "@DELIVERYid";
            param.Value = DeliveryId;
            command.Parameters.Add(param);

            await _context.Database.OpenConnectionAsync();

            var result = await command.ExecuteScalarAsync();

            return result != null ? Convert.ToInt32(result) : -1;
        }
        /// <summary>
        /// update pick process tables from pick or confirm process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="call13id"></param>
        /// <param name="LPNNumber"></param>
        /// <returns></returns>
        public async Task<int> UpdatePickProcessTables(string DeliveryId, int call13id, string LPNNumber)
        {
            var infoEncPickProcess = await _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog
        .FirstOrDefaultAsync(x => x.DeliveryId == DeliveryId);

            if (infoEncPickProcess == null)
                return -1;

            var infoDetPickProcess = await _context.TzebB2bPSoLineItemTblItemInbConsReqsLog
                .Where(x => x.PSoWoHdrTblItemId == infoEncPickProcess.Id)
                .ToListAsync();

            if (infoDetPickProcess == null || !infoDetPickProcess.Any())
                return -2;

            infoEncPickProcess.DateTimeProcessed = DateTime.Now;
            infoEncPickProcess.OutConsReqsId = call13id;
            _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog.Update(infoEncPickProcess);
            await _context.SaveChangesAsync();

            foreach (var infodet in infoDetPickProcess)
            {
                infodet.DateTimeProcessed = DateTime.Now;
                infodet.PickLpn = LPNNumber.Trim();
                infodet.QtyOnHand = Convert.ToInt32(infodet.RequestedQuantity);

                _context.TzebB2bPSoLineItemTblItemInbConsReqsLog.Update(infodet);
            }

            await _context.SaveChangesAsync();
            return 1; // éxito
        }
    }
}

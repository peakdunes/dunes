
using DUNES.API.Data;
using DUNES.API.DTOs.Inventory;
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
    }
}

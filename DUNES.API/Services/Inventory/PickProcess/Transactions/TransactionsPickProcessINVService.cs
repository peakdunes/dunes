using DUNES.API.Data;
using DUNES.API.DTOs.Inventory;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.Services.Inventory.PickProcess.Transactions
{
    /// <summary>
    /// All pick-process transactions
    /// </summary>
    public class TransactionsPickProcessINVService : ITransactionsPickProcessINVService
    {


        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        private readonly ITransactionsPickProcessINVRepository _transactionRepository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        /// <param name="transactionRepository"></param>
        public TransactionsPickProcessINVService(AppDbContext context, appWmsDbContext wmscontext,
            ITransactionsPickProcessINVRepository transactionRepository)
        {
            _context = context;
            _wmscontext = wmscontext;
            _transactionRepository = transactionRepository;
        }

        /// <summary>
        /// Create a servtrack order from delivery id
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<ServTrackReferenceCreatedDto>> CreateServTrackOrderFromPickProcess(string DeliveryId)
        {
            //we validate that the PickProcess Exist and it is available to process


            var infopick = await _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog.FirstOrDefaultAsync(x => x.DeliveryId == DeliveryId);

            if (infopick == null)
            {
                return ApiResponseFactory.NotFound<ServTrackReferenceCreatedDto>(
                  $"Pick Process {DeliveryId} does not exist in the system .");
            }

            //if (infopick.DateTimeProcessed != null || infopick.OutConsReqsId != null )
            //{
            //    return ApiResponseFactory.NotFound<ServTrackReferenceCreatedDto>(
            //     $"Pick Process {DeliveryId} already processed.");
            //}

            var result = await _transactionRepository.CreateServTrackOrderFromPickProcess(DeliveryId);

            return ApiResponseFactory.Ok(result, "OK");

        }
    }
}

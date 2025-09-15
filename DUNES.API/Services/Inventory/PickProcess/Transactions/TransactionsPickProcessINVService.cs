using DUNES.API.Data;
using DUNES.API.DTOs.Inventory;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
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

        private readonly ITransactionsPickProcessINVRepository _transactionPickProcessINVRepository;
        private readonly ITransactionsWMSINVService _transactionsWMSINVService;
       

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        /// <param name="transactionPickProcessINVRepository"></param>
        /// <param name="transactionsWMSINVService"></param>
        
        public TransactionsPickProcessINVService(AppDbContext context, appWmsDbContext wmscontext,
            ITransactionsPickProcessINVRepository transactionPickProcessINVRepository,
            ITransactionsWMSINVService transactionsWMSINVService)
        {
            _context = context;
            _wmscontext = wmscontext;
            _transactionPickProcessINVRepository = transactionPickProcessINVRepository;
            _transactionsWMSINVService = transactionsWMSINVService;
          
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

            var result = await _transactionPickProcessINVRepository.CreateServTrackOrderFromPickProcess(DeliveryId);

            return ApiResponseFactory.Ok(result, "OK");

        }

        /// <summary>
        /// Perform pick process processing
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="objInvData"></param>
        /// <param name="lpnid"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(string DeliveryId, NewInventoryTransactionTm objInvData,string lpnid)
        {
            // throw new NotImplementedException();

            PickProcessResponseDto objresponse = new PickProcessResponseDto();

            await using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1. Order Repair ServTrack (4 tablas)
                var servTrackResult = await CreateServTrackOrderFromPickProcess(DeliveryId);

                if (!servTrackResult.Success)
                {
                    await tx.RollbackAsync();
                    return ApiResponseFactory.NotFound<PickProcessResponseDto>(
                  $"Pick Process {DeliveryId} does not exist in the system .");
                   
                }

                objresponse.ServTrackOrder = servTrackResult.Data.RefNum;



                //Create WMS Transaction

                var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData);


                if (!wmsTransaction.Success)
                {
                    await tx.RollbackAsync();
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                  $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error} .");
                }

                objresponse.WMSTransactionNumber = wmsTransaction.Data;

                var call13info = await CreatePickProcessCall(DeliveryId);

                if (!call13info.Success)
                {
                    await tx.RollbackAsync();
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                  $"Error creating Pick Process call(10). Error {wmsTransaction.Message} .");
                }


                objresponse.Call13Number =  call13info.Data; 


                //Update pick process tables

                var UpdateTablesOk = await UpdatePickProcessTables(DeliveryId, objresponse.Call13Number, lpnid);

               if (!UpdateTablesOk.Success)
                {
                    await tx.RollbackAsync();
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                $"Error updatein Pick Process tables. Error {wmsTransaction.Message} .");

                }
              

                 return ApiResponseFactory.Ok(objresponse, "Pick process completed successfully");
            }
            catch (Exception ex)
            {
                // ❌ Rollback
                await tx.RollbackAsync();
                return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.Message}");
            }
        }
        /// <summary>
        /// create call (13) for pick process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<int>> CreatePickProcessCall(string DeliveryId)
        {
            var callId = await _transactionPickProcessINVRepository.CreatePickProcessCall(DeliveryId);

            if (callId <= 0)
            {
                return ApiResponseFactory.BadRequest<int>(
                    $"[CALL] Failed: SP returned invalid Id for delivery {DeliveryId}");
            }

            return ApiResponseFactory.Ok(callId, "Call created successfully");
        }
        /// <summary>
        /// Update pickprocess table for pick and confirm process 
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="call13id"></param>
        /// <param name="LPNNumber"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> UpdatePickProcessTables(string DeliveryId, int call13id, string LPNNumber)
        {
            var result = await _transactionPickProcessINVRepository.UpdatePickProcessTables(DeliveryId, call13id, LPNNumber);

            return result switch
            {
                -1 => ApiResponseFactory.NotFound<bool>($"[PICK_PROCESS] Header not found for delivery {LPNNumber}"),
                -2 => ApiResponseFactory.NotFound<bool>($"[PICK_PROCESS] Details not found for delivery {LPNNumber}"),
                1 => ApiResponseFactory.Ok(true, "Pick process updated successfully"),
                _ => ApiResponseFactory.BadRequest<bool>($"[PICK_PROCESS] Unknown error updating delivery {LPNNumber}")
            };
        }
    }
}

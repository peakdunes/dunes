using DUNES.API.Data;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Net;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /// Ejecuta el proceso de Pick Process de manera transaccional.
        /// </summary>
        /// <param name="DeliveryId">Identificador de la orden de Pick Process.</param>
        /// <param name="objInvData">Objeto con los datos de la transacción de inventario WMS.</param>
        /// <param name="lpnid">Identificador del LPN asociado al Pick Process.</param>
        /// <param name="ct">Token de cancelación para abortar la operación si es necesario.</param>
        /// <returns>
        /// Retorna un <see cref="ApiResponse{T}"/> con un objeto <see cref="PickProcessResponseDto"/> 
        /// que contiene los números de referencia generados en cada etapa del proceso (ServTrack, 
        /// WMS Transaction y Pick Process Call).
        /// </returns>
        /// <remarks>
        /// Este método coordina múltiples operaciones que afectan diferentes bases de datos 
        /// (DB1 y DB2) dentro de la misma instancia de SQL Server.  
        /// 
        /// Flujo principal:
        /// 1. Crear la orden en ServTrack (DB1).
        /// 2. Crear la transacción en WMS (DB2).
        /// 3. Crear la llamada de Pick Process (DB1).
        /// 4. Actualizar las tablas relacionadas en DB1.
        /// 
        /// Todo se ejecuta bajo la misma transacción.  
        /// - Si alguna operación falla, se hace <c>Rollback</c> y no se persiste nada en ninguna base.  
        /// - Si todas las operaciones son exitosas, se hace <c>Commit</c> confirmando todos los cambios.  
        /// </remarks>

        public async Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(string DeliveryId,NewInventoryTransactionTm objInvData,
                                                string lpnid, CancellationToken ct)
        {
            var objresponse = new PickProcessResponseDto();

            // Paso 1: Transacción en WMS (header + detail)
            var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData, ct);
            if (!wmsTransaction.Success)
            {
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                    $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error}.");
            }
            objresponse.WMSTransactionNumber = wmsTransaction.Data;

            // Paso 2: Transacción en Zebra
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync(ct);

                var servTrackResult = await CreateServTrackOrderFromPickProcess(DeliveryId);
                if (!servTrackResult.Success)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.NotFound<PickProcessResponseDto>($"Pick Process {DeliveryId} does not exist.");
                }

                var call13info = await CreatePickProcessCall(DeliveryId, ct);
                if (!call13info.Success)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error creating Pick Process call(10). Error {call13info.Message}.");
                }

                var updateTablesOk = await UpdatePickProcessTables(DeliveryId, call13info.Data, lpnid);
                if (!updateTablesOk.Success)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error updating Pick Process tables. Error {updateTablesOk.Message}.");
                }

                await tx.CommitAsync(ct);

                objresponse.ServTrackOrder = servTrackResult.Data.RefNum;
                objresponse.Call13Number = call13info.Data;

                return ApiResponseFactory.Ok(objresponse, "Pick process completed successfully");
            }
            catch (Exception ex)
            {
                // rollback zebra + compensar wms
                await _transactionsWMSINVService.DeleteInventoryTransaction(objresponse.WMSTransactionNumber, ct);
                return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.Message}");
            }
        }


        //public async Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(string DeliveryId, NewInventoryTransactionTm objInvData, string lpnid, CancellationToken ct)
        //{
        //    // throw new NotImplementedException();

        //    bool TransactionNumber = false;

        //    PickProcessResponseDto objresponse = new PickProcessResponseDto();


        //    await using var txwms = await _context.Database.BeginTransactionAsync();

        //    try 
        //    {
        //        //Create WMS Transaction

        //        var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData, ct);


        //        if (!wmsTransaction.Success)
        //        {
        //            await txwms.RollbackAsync();
        //            return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //          $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error} .");
        //        }

        //        objresponse.WMSTransactionNumber = wmsTransaction.Data;

        //        TransactionNumber = true;

        //    }
        //    catch (Exception ex)
        //    {
        //        // ❌ Rollback
        //        await txwms.RollbackAsync();
        //        return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.Message}");
        //    }


        //    if (TransactionNumber)
        //    {
        //        await using var tx = await _context.Database.BeginTransactionAsync();

        //        try
        //        {


        //            // 1. Order Repair ServTrack (4 tablas)
        //            var servTrackResult = await CreateServTrackOrderFromPickProcess(DeliveryId);

        //            if (!servTrackResult.Success)
        //            {
        //                await tx.RollbackAsync();
        //                return ApiResponseFactory.NotFound<PickProcessResponseDto>(
        //              $"Pick Process {DeliveryId} does not exist in the system .");

        //            }

        //            objresponse.ServTrackOrder = servTrackResult.Data.RefNum;


        //            var call13info = await CreatePickProcessCall(DeliveryId);

        //            if (!call13info.Success)
        //            {
        //                await tx.RollbackAsync();
        //                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //              $"Error creating Pick Process call(10). Error {wmsTransaction.Message} .");
        //            }


        //            objresponse.Call13Number = call13info.Data;


        //            //Update pick process tables

        //            var UpdateTablesOk = await UpdatePickProcessTables(DeliveryId, objresponse.Call13Number, lpnid);

        //            if (!UpdateTablesOk.Success)
        //            {
        //                await tx.RollbackAsync();
        //                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //            $"Error updatein Pick Process tables. Error {UpdateTablesOk.Message} .");

        //            }



        //        }
        //        catch (Exception ex)
        //        {
        //            // ❌ Rollback
        //            await tx.RollbackAsync();

        //            //eliminar la transaccion de WMS

        //            return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.Message}");
        //        }

        //    }

        //    return ApiResponseFactory.Ok(objresponse, "Pick process completed successfully");
        //}
        /////// <summary>
        ///// create call (13) for pick process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        public async Task<ApiResponse<int>> CreatePickProcessCall(string DeliveryId, CancellationToken ct)
        {
            var callId = await _transactionPickProcessINVRepository.CreatePickProcessCall(DeliveryId, ct);

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

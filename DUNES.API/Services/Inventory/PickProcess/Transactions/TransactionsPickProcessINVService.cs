using DUNES.API.Data;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.Common.Transactions;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.Services.Auth;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.Utils.Logging;

using DUNES.API.Utils.TraceProvider;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Interfaces.RequestInfo;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.Utils.Reponse;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using System;
using System.Diagnostics;
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

        private readonly LogHelper _logHelper;
        private readonly IRequestInfo _requestInfo;
        private readonly ITraceProvider _trace;
        private readonly ICurrentUser _currentUser;

        private readonly AppDbContext _context;
        private readonly appWmsDbContext _wmscontext;

        private readonly ITransactionsPickProcessINVRepository _transactionPickProcessINVRepository;
        private readonly ITransactionsWMSINVService _transactionsWMSINVService;
        private readonly ICommonQueryWMSINVService _commonQueryWMSINVService;
        private readonly ITransactionsCommonINVRepository _transactionsCommonINVRepository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        /// <param name="transactionPickProcessINVRepository"></param>
        /// <param name="transactionsWMSINVService"></param>
        /// <param name="requestInfo"></param>
        /// <param name="logHelper"></param>
        /// <param name="trace"></param>
        /// <param name="currentUser"></param>
        /// <param name="transactionsCommonINVRepository"></param>
        /// <param name="commonQueryWMSINVService"></param>
        public TransactionsPickProcessINVService(AppDbContext context, appWmsDbContext wmscontext,
            ITransactionsPickProcessINVRepository transactionPickProcessINVRepository,
            ITransactionsWMSINVService transactionsWMSINVService, IRequestInfo requestInfo,
            LogHelper logHelper, ITraceProvider trace, ICurrentUser currentUser,
            ITransactionsCommonINVRepository transactionsCommonINVRepository, ICommonQueryWMSINVService commonQueryWMSINVService)
        {
            _context = context;
            _wmscontext = wmscontext;
            _transactionPickProcessINVRepository = transactionPickProcessINVRepository;
            _transactionsWMSINVService = transactionsWMSINVService;
            _requestInfo = requestInfo;
            _logHelper = logHelper;
            _trace = trace;
            _currentUser = currentUser;
            _transactionsCommonINVRepository = transactionsCommonINVRepository;
            _commonQueryWMSINVService = commonQueryWMSINVService;
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

        public async Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(string DeliveryId, NewInventoryTransactionTm objInvData,
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

                if (servTrackResult.Data == null || servTrackResult.Data.RefNum <= 0)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.NotFound<PickProcessResponseDto>($"Pick Process {DeliveryId} Error :{servTrackResult.Data.ErrorMessage}");

                }

                if (!servTrackResult.Success)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.NotFound<PickProcessResponseDto>($"Pick Process {DeliveryId} does not exist.");
                }

                var call13info = await CreatePickProcessCall(DeliveryId, ct);
                if (!call13info.Success)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error creating Pick Process call(10). Error {call13info.Message}.");
                }

                var updateTablesOk = await UpdatePickProcessTables(DeliveryId, call13info.Data, lpnid);
                if (!updateTablesOk.Success)
                {
                    await tx.RollbackAsync(ct);
                    await _transactionsWMSINVService.DeleteInventoryTransaction(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,objresponse.WMSTransactionNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error updating Pick Process tables. Error {updateTablesOk.Message}.");
                }

                //ZEBRA Inventory transactionbs


                List<PickProcessWm> listInvProcess = new List<PickProcessWm>();


                var infoTransacInput = await _commonQueryWMSINVService.GetAllActiveTransactionsInputType(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient, ct);

                if (infoTransacInput == null || infoTransacInput.Data == null)
                {
                    await tx.RollbackAsync(ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                       $"Error active input transactions type not found. Error {updateTablesOk.Message}.");
                }

                var infoTransacOutput = await _commonQueryWMSINVService.GetAllActiveTransactionsOutputType(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient, ct);

                if (infoTransacOutput == null || infoTransacOutput.Data == null)
                {
                    await tx.RollbackAsync(ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                       $"Error active output transactions type not found. Error {updateTablesOk.Message}.");
                }



                var infoInvType = await _commonQueryWMSINVService.GetAllActiveInventoryType(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient, ct);

                if (infoInvType == null || infoInvType.Data == null)
                {
                    await tx.RollbackAsync(ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                       $"Error active inventory type not found. Error {updateTablesOk.Message}.");
                }
                //input transactions
                foreach (var tran in infoTransacInput.Data)
                {
                    


                    foreach (var info in objInvData.Listdetails)
                    {
                        if (tran.Id == info.Idtypetransaction)
                        {
                            PickProcessWm objdet = new PickProcessWm();

                            if (tran.Isinput)
                            {
                                foreach (var type in infoInvType.Data)
                                {
                                    if (type.Id == info.Idtype)
                                    {
                                        objdet.InvSource = type.Zebrainvassociated;
                                        objdet.sourcename = type.Name;


                                    }
                                }
                            }

                            objdet.itemid = info.Iditem;
                            objdet.itemname = info.Codeitem;
                           
                            objdet.InvDest = 0;
                            objdet.qty = info.TotalQty;
                            objdet.typetransactionid = info.Idtypetransaction;
                            objdet.match = tran.Match;
                            objdet.notes = DeliveryId.ToString().Trim();
                            objdet.serialnumber = info.Serialid ?? "";
                            listInvProcess.Add(objdet);
                        }

                    }
                }


                //output transactions

                foreach (var listp in listInvProcess)
                {
                    foreach (var tran in infoTransacOutput.Data)
                    {
                        if (tran.Match == listp.match)
                        {
                            foreach (var info in objInvData.Listdetails)
                            {
                                if (tran.Id == info.Idtypetransaction)
                                {
                                    foreach (var type in infoInvType.Data)
                                    {
                                        if (type.Id == info.Idtype)
                                        {
                                            listp.InvDest = type.Zebrainvassociated;
                                            listp.destname = type.Name;
                                            continue;
                                        }
                                    }
                                    continue;
                                }
                            }
                        }
                    }
                }

                List<TzebB2bReplacementPartsInventoryLogDto> listlogs = new List<TzebB2bReplacementPartsInventoryLogDto>();

                foreach (var listp in listInvProcess)
                {

                    TzebB2bReplacementPartsInventoryLogDto infodetmov = new TzebB2bReplacementPartsInventoryLogDto();


                    infodetmov.PartDefinitionId = listp.itemid;
                    infodetmov.PartNumberName = listp.itemname;
                    infodetmov.InventoryTypeIdSource = listp.InvSource;
                    infodetmov.InvSourceName = listp.sourcename;
                    infodetmov.InventoryTypeIdDest = listp.InvDest;
                    infodetmov.InvDestName = listp.destname;
                    infodetmov.SerialNo = listp.serialnumber;
                    infodetmov.Qty = listp.qty;
                    infodetmov.Notes = listp.notes.Trim();
                    infodetmov.RepairNo = 0; ;
                    infodetmov.DateInserted = DateTime.Now;

                    listlogs.Add(infodetmov);

                    
                }


                //esto lo crea automaticamente el retorno de la llamada 13

                //var infotranInv = await _transactionsCommonINVRepository
                //        .createInventoryTransactionLog(listlogs, _currentUser.UserId!, ct);

                //if (!infotranInv)
                //{
                //    await tx.RollbackAsync(ct);
                //    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                //       $"Error creating ZEBRA Inventory transaction log. Error {updateTablesOk.Message}.");
                //}
                //fin llamada 13

                await tx.CommitAsync(ct);

                objresponse.ServTrackOrder = servTrackResult.Data.RefNum;
                objresponse.Call13Number = call13info.Data;

                return ApiResponseFactory.Ok(objresponse, "Pick process completed successfully");
            }
            catch (Exception ex)
            {

                // Compensación WMS si algo explota en Zebra
                await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!, objresponse.WMSTransactionNumber, ct);
                return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.GetBaseException().Message}");

            }
        }

        /// <summary>
        /// cuando un proceso implica dos bases de datos se hace primero el proceso mas corto el la base de datos
        /// correspondiente y luego todo el resto se hace en una transaccion, si esta transaccion falla
        /// este metodo hace rollback al proceso mas corto inicial
        ///
        /// </summary>
        /// <param name="wmsTxNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private async Task TryCompensateWmsAsync(int companyid, string companyclientid, int wmsTxNumber, CancellationToken ct)
        {
            if (wmsTxNumber == 0) return;
            try
            {
                await _transactionsWMSINVService.DeleteInventoryTransaction(companyid, companyclientid, wmsTxNumber, ct);
            }
            catch (Exception ex)
            {

                var svcName = GetType().Name;


                //_requestInfo?.Method is { } m significa: “si _requestInfo?.Method no es null, asígnalo a la variable m”
                //_requestInfo?.Path is { } p significa: “si _requestInfo?.Method no es null, asígnalo a la variable p”
                // && exige que ambos (Method y Path) existan.
                //? $"{m} {p}" : null → si ambos existen, arma la cadena "METHOD PATH"; si no, deja null.
                var httpRoute = (_requestInfo?.Method is { } m && _requestInfo?.Path is { } p) ? $"{m} {p}" : null;
                var targetOp = $"WMSINV.{nameof(ITransactionsWMSINVService.DeleteInventoryTransaction)}(txId={wmsTxNumber})";
                var ruta = httpRoute ?? targetOp;


                await _logHelper.SaveLogAsync(
                       traceId: _trace.TraceId,
                       message: $"WMS compensation failed for WMS Transaction Number {wmsTxNumber}",
                       exception: ex.GetBaseException().Message,
                       level: "Error",
                       usuario: _currentUser.UserId,
                       origen: GetType().Name,
                       ruta: ruta
                       );

            }
        }


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

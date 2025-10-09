using DUNES.API.Data;
using DUNES.API.Models.Inventory.Common;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.ASN.Transactions;
using DUNES.API.Repositories.Inventory.Common.Transactions;
using DUNES.API.Services.Auth;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.Utils.Logging;
using DUNES.API.Utils.Responses;
using DUNES.API.Utils.TraceProvider;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Interfaces.RequestInfo;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DUNES.API.Services.Inventory.ASN.Transactions
{
    /// <summary>
    /// ASN Transactions
    /// </summary>
    public class TransactionsASNINVService : ITransactionsASNINVService
    {



        private readonly appWmsDbContext _wmscontext;
        private readonly AppDbContext _context;
        private readonly ICurrentUser _currentUser;
        private readonly ITransactionsWMSINVService _transactionsWMSINVService;
        private readonly ITransactionsASNINVRepository _transactionsASNINVRepository;
        private readonly ITransactionsCommonINVRepository _transactionsCommonINVRepository;
        private readonly ITraceProvider _trace;
        private readonly IRequestInfo _requestInfo;

        /// <summary>
        ///  dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        /// <param name="currentUser"></param>
        /// <param name="transactionsWMSINVService"></param>
        /// <param name="transactionsASNINVRepository"></param>
        /// <param name="transactionsCommonINVRepository"></param>
        /// <param name="trace"></param>
        /// <param name="requestInfo"></param>
        public TransactionsASNINVService(appWmsDbContext wmscontext, ITransactionsWMSINVService transactionsWMSINVService,
            ITransactionsASNINVRepository transactionsASNINVRepository, AppDbContext context,
            ITransactionsCommonINVRepository transactionsCommonINVRepository,
            ICurrentUser currentUser, ITraceProvider trace, IRequestInfo requestInfo)
        {
            _context = context;
            _transactionsWMSINVService = transactionsWMSINVService;
            _wmscontext = wmscontext;
            _transactionsASNINVRepository = transactionsASNINVRepository;
            _transactionsCommonINVRepository = transactionsCommonINVRepository;
            _currentUser = currentUser;
            _trace = trace;
            _requestInfo = requestInfo;

        }
        /// <summary>
        /// Perform ASN Receiving Transaction
        /// </summary>
        /// <param name="AsnId"></param>
        /// <param name="objInvData"></param>
        /// <param name="trackingNumber"></param>
        /// <param name="detaillist"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 
        public async Task<ApiResponse<PickProcessResponseDto>> CreateASNReceivingTransaction(
    string AsnId,
    NewInventoryTransactionTm objInvData,
    string trackingNumber,
    List<BinsToLoadWm> detaillist,
    CancellationToken ct)
        {
            // 0) Autenticación
            if (!_currentUser.IsAuthenticated)
                return ApiResponseFactory.Unauthorized<PickProcessResponseDto>("User Unauthorized");

            // 1) Carga y validaciones previas (solo lectura)
            var infoAsn = await _context.TzebB2bAsnOutHdrDetItemInbConsReqs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShipmentNum == AsnId, ct);

            if (infoAsn == null)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>($"ASN Information not found {AsnId}.");

            if (infoAsn.Processed == true)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>($"ASN {AsnId} already processed.");

            var infocallreceiving = await _context.TzebB2bInbConsReqs
                .FirstOrDefaultAsync(x => x.Id == infoAsn.ConsignRequestId, ct);

            if (infocallreceiving == null)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>($"This ASN {AsnId} has not an input call.");

            // Agrupar cantidades por línea
            var grouped = detaillist
                .GroupBy(x => x.asnlineid)
                .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) })
                .ToList();

            var lineIds = grouped.Select(g => g.lineid).ToList();

            // Validar que existan las líneas
            var linesFound = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
                .Where(x => lineIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(ct);

            var missingLineIds = lineIds.Except(linesFound).ToList();
            if (missingLineIds.Count > 0)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                    $"ASN lines not found: {string.Join(", ", missingLineIds)}");

            // Validación Item Master (evitar N+1 con join)
            var itemPartNumbers = detaillist
                .Where(d => !string.IsNullOrWhiteSpace(d.partnumber))
                .Select(d => "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant())
                .Distinct()
                .ToList();

            var partsExisting = await _context.TzebB2bMasterPartDefinition
                .Where(i => itemPartNumbers.Contains(i.PartNo))
                .Select(i => i.PartNo)
                .ToListAsync(ct);

            var missingParts = itemPartNumbers.Except(partsExisting, StringComparer.OrdinalIgnoreCase).ToList();
            if (missingParts.Count > 0)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                    $"Not all items are in Item Master Table. Missing: {string.Join(", ", missingParts)}");

            // Parámetros de inventario
            var infoparam = await _context.MvcGeneralParameters
                .Where(x => x.ParameterNumber <= 4)
                .ToListAsync(ct);

            // BUG original: chequeabas "!= null". Debe validar null o cantidad insuficiente
            if (infoparam == null || infoparam.Count < 4)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>("There are no Inventory Transactions parameters.");

            int InvSource = 0, InvDest = 0, TypeCallId = 0;
            string TransactionCode = string.Empty;

            foreach (var p in infoparam)
            {
                switch (p.ParameterNumber)
                {
                    case 1: InvSource = Convert.ToInt32(p.ParameterValue); break;
                    case 2: InvDest = Convert.ToInt32(p.ParameterValue); break;
                    case 3: TransactionCode = string.IsNullOrWhiteSpace(p.ParameterValue) ? "" : p.ParameterValue.Trim(); break;
                    case 4: TypeCallId = Convert.ToInt32(p.ParameterValue); break;
                }
            }

            if (InvSource == 0 || InvDest == 0 || string.IsNullOrEmpty(TransactionCode) || TypeCallId == 0)
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>("There are no Inventory Transactions parameters.");

            // Build del header para logs
            var RequestASNData = new AsnReceivedHdrLogRead
            {
                asnNumber = AsnId,
                TransactionCode = "SRA",
                TransactionType = "IR RECEIPT",
                ProcessName = "Receiving Tool",
                org3pl = infoAsn.OrgSystemId3pl,
                locator3pl = string.Empty,
                IsRtvPart = false,
                IsCePart = false
            };

            // 2) Paso WMS (EXTERNO) —> luego compensamos si falla Zebra
            string wmsTxNumber = null!;
            {
                var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData, ct);
                if (!wmsTransaction.Success)
                {
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error}.");
                }
                wmsTxNumber = wmsTransaction.Data?.TransactionNumber ?? wmsTransaction.Message; // ajusta según tu DTO de WMS
            }

            // 3) Transacción local (Zebra) — TODO lo de EF dentro del BeginTransaction
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, ct);

                // 3.1) Crear IR Receipt (HDR + DET) en log / tablas
                var infoIrReceipt = await _transactionsASNINVRepository
                    .CreateIrReceiptHdrAndDetailLog(RequestASNData, detaillist, ct);

                if (infoIrReceipt <= 0)
                {
                    await tx.RollbackAsync(ct);
                    // Compensar WMS
                    await TryCompensateWmsAsync(wmsTxNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error creating Receipt Transaction for this ASN: {AsnId}.");
                }

                // 3.2) Agrupar por Part+Serial y mapear a IDs
                var groupedByItemSerial = detaillist
                    .Where(d => !string.IsNullOrWhiteSpace(d.partnumber) && !string.IsNullOrWhiteSpace(d.serialnumber))
                    .GroupBy(d => new
                    {
                        Part = "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant(),
                        Serial = d.serialnumber.Trim().ToUpperInvariant()
                    })
                    .Select(g => new { PartNumber = g.Key.Part, SerialNumber = g.Key.Serial, Qty = g.Sum(x => x.qty) })
                    .ToList();

                var items = await _context.TzebB2bMasterPartDefinition
                    .Where(i => itemPartNumbers.Contains(i.PartNo))
                    .Select(i => new { i.Id, i.PartNo })
                    .ToListAsync(ct);

                var idByPart = items.ToDictionary(x => x.PartNo, x => x.Id, StringComparer.OrdinalIgnoreCase);

                var movementLogs = new List<TzebB2bReplacementPartsInventoryLogDto>(groupedByItemSerial.Count);
                foreach (var it in groupedByItemSerial)
                {
                    if (!idByPart.TryGetValue(it.PartNumber, out var itemId))
                    {
                        await tx.RollbackAsync(ct);
                        await TryCompensateWmsAsync(wmsTxNumber, ct);
                        return ApiResponseFactory.BadRequest<PickProcessResponseDto>($"Part not found: {it.PartNumber}.");
                    }

                    movementLogs.Add(new TzebB2bReplacementPartsInventoryLogDto
                    {
                        PartDefinitionId = itemId,
                        InventoryTypeIdSource = InvSource,
                        InventoryTypeIdDest = InvDest,
                        SerialNo = it.SerialNumber,
                        Qty = it.Qty,
                        Notes = AsnId,
                        RepairNo = null,
                        DateInserted = DateTime.UtcNow
                    });
                }

                // 3.3) Crear movimiento de inventario (log + update tabla inventario)
                var invLogOk = await _transactionsCommonINVRepository
                    .createInventoryTransactionLog(movementLogs, _currentUser.UserId!, ct);

                if (!invLogOk)
                {
                    await tx.RollbackAsync(ct);
                    await TryCompensateWmsAsync(wmsTxNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error creating Inventory movement log for ASN: {AsnId}.");
                }

                // 3.4) Marcar ASN como procesado
                infoAsn.Processed = true;
                _context.TzebB2bAsnOutHdrDetItemInbConsReqs.Update(infoAsn);
                await _context.SaveChangesAsync(ct);

                // 3.5) Marcar input call como procesado
                infocallreceiving.Processed = true;
                _context.TzebB2bInbConsReqs.Update(infocallreceiving);
                await _context.SaveChangesAsync(ct);

                // 3.6) Crear salida (output consignment call)
                var objcallout = new TzebB2bOutConsReqsInsertDto
                {
                    TypeOfCallId = TypeCallId,
                    TransactionCode = TransactionCode
                };

                var outputCallNumber = await _transactionsCommonINVRepository.createConsOutPutCall(objcallout, ct);
                if (outputCallNumber <= 0)
                {
                    await tx.RollbackAsync(ct);
                    await TryCompensateWmsAsync(wmsTxNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error creating ASN OutPut Call Transaction for this ASN: {AsnId}.");
                }

                // 3.7) Vincular el IR Receipt con el output call
                var infoRECEIPT = await _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog
                    .FirstOrDefaultAsync(x => x.Id == infoIrReceipt, ct);

                if (infoRECEIPT == null)
                {
                    await tx.RollbackAsync(ct);
                    await TryCompensateWmsAsync(wmsTxNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Receipt Transaction for this ASN: {AsnId} not found.");
                }

                infoRECEIPT.ConsignDbkrequestId = outputCallNumber;
                _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.Update(infoRECEIPT);
                await _context.SaveChangesAsync(ct);

                // 3.8) Poner el output call "ReadyToProcess"
                var updatecall = await _transactionsCommonINVRepository
                    .updateConsOutPutCallReadyToProcess(outputCallNumber, ct);

                if (!updatecall)
                {
                    await tx.RollbackAsync(ct);
                    await TryCompensateWmsAsync(wmsTxNumber, ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                        $"Error updating Output call {outputCallNumber} for ASN: {AsnId}.");
                }

                // 3.9) Commit final
                await tx.CommitAsync(ct);

                // 4) OK
                var objres = new PickProcessResponseDto
                {
                    // llena aquí lo que necesite el front (IDs, wmsTxNumber, etc.)
                };

                return ApiResponseFactory.Ok(objres, "Pick process completed successfully");
            }
            catch (Exception ex)
            {
                // Compensación WMS si algo explota en Zebra
                await TryCompensateWmsAsync(wmsTxNumber, ct);
                return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.GetBaseException().Message}");
            }
        }

        // Helper de compensación (best-effort)
        private async Task TryCompensateWmsAsync(string? wmsTxNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(wmsTxNumber)) return;
            try
            {
                await _transactionsWMSINVService.DeleteInventoryTransaction(Convert.ToInt32(wmsTxNumber), ct);
            }
            catch (Exception ex)
            {

                //var path = _requestInfo.Path ?? $"{nameof(AsnService)}.{nameof(TryCompensateWmsAsync)}";
                //var method = _requestInfo.Method ?? "N/A";
                //var fullRoute = $"{method} {path}";

                // No relanzar: solo loguea; podrías guardar en tabla Outbox/CompensationPending
                await LogHelper.SaveLogAsync(
                    traceId: _trace.TraceId,
                    message: $"WMS compensation failed for {wmsTxNumber}",
                    exception: ex.ToString(),
                    level: "Error",
                    usuario: _currentUser.UserId,
                    origen: "GlobalExceptionMiddleware",
                    ruta: _requestInfo.Path.Request.Path
                );

            }
        }


        //public async Task<ApiResponse<PickProcessResponseDto>> CreateASNReceivingTransaction(string AsnId,
        //    NewInventoryTransactionTm objInvData, string trackingNumber, List<BinsToLoadWm> detaillist, CancellationToken ct)
        //{

        //    AsnReceivedHdrLogRead RequestASNData = new AsnReceivedHdrLogRead();


        //    if (!_currentUser.IsAuthenticated)
        //        return ApiResponseFactory.Unauthorized<PickProcessResponseDto>(
        //            $"User Unauthorized");

        //    //check ASN Head info
        //    var infoAsn = await _context.TzebB2bAsnOutHdrDetItemInbConsReqs.FirstOrDefaultAsync(x => x.ShipmentNum == AsnId);

        //    if (infoAsn == null)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //           $"ASN Information not found {AsnId}.");
        //    }

        //    //check if this ASN already processed

        //    if (infoAsn.Processed == true)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //          $"ASN  {AsnId} already processed.");
        //    }


        //    //check if exit input call 

        //    var infocallreceiving = await _context.TzebB2bInbConsReqs.FirstOrDefaultAsync(x => x.Id == infoAsn.ConsignRequestId);

        //    if (infocallreceiving == null)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //        $"This ASN  {AsnId} have not a input call.");
        //    }

        //    RequestASNData.asnNumber = AsnId;
        //    RequestASNData.TransactionCode = "SRA";
        //    RequestASNData.TransactionType = "IR RECEIPT";
        //    RequestASNData.ProcessName = "Receiving Tool";
        //    RequestASNData.org3pl = infoAsn.OrgSystemId3pl;
        //    RequestASNData.locator3pl = string.Empty;
        //    RequestASNData.IsRtvPart = false;
        //    RequestASNData.IsCePart = false;


        //    //check ASN Detail info

        //    var listgroup = detaillist.GroupBy(x => x.asnlineid)
        //  .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) }).ToList();

        //    foreach (var item in listgroup)
        //    {

        //        var infoline = _context.TzebB2bAsnLineItemTblItemPartialInbConsReqs.FirstOrDefaultAsync(x => x.Id == item.lineid);

        //        if (infoline == null)
        //        {
        //            return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //                              $"ASN line {item.lineid} not found.");
        //        }

        //    }


        //    var grouped = detaillist
        //      .GroupBy(x => x.asnlineid)
        //      .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) })
        //      .ToList();

        //    var lineIds = grouped.Select(g => g.lineid).ToList();

        //    var infoByLine = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
        //        .Where(x => lineIds.Contains(x.Id)).ToListAsync();

        //    //validating all items exist in item master

        //    bool IsMissing = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
        //    .Where(l => lineIds.Contains(l.Id))
        //    .AnyAsync(l => !_context.TzebB2bMasterPartDefinition.Any(i => i.PartNo == "ZEBRA-" + l.ItemNumber), ct);

        //    if (IsMissing)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //                             $"Not all items are in Item Master Table.");
        //    }

        //    //validating parameters

        //    var infoparam = await _context.MvcGeneralParameters.Where(x => x.ParameterNumber <= 4).ToListAsync();

        //    if (infoparam != null)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //            $"There is not Inventory Transactions parameters.");
        //    }

        //    int InvSource = 0;
        //    int InvDest = 0;
        //    string TransactionCode = string.Empty;
        //    int TypeCallId = 0;

        //    foreach (var info in infoparam!)
        //    {
        //        if (info.ParameterNumber == 1)
        //        {
        //            InvSource = Convert.ToInt32(info.ParameterValue);
        //        }
        //        if (info.ParameterNumber == 2)
        //        {
        //            InvDest = Convert.ToInt32(info.ParameterValue);
        //        }
        //        if (info.ParameterNumber == 3)
        //        {
        //            TransactionCode = string.IsNullOrEmpty(info.ParameterValue) ? "" : info.ParameterValue.Trim();
        //        }
        //        if (info.ParameterNumber == 4)
        //        {
        //            TypeCallId = Convert.ToInt32(info.ParameterValue);
        //        }


        //    }

        //    if (InvSource == 0 || InvDest == 0 || string.IsNullOrEmpty(TransactionCode) || TypeCallId == 0)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //            $"There is not Inventory Transactions parameters.");
        //    }

        //    // Paso 1: Transacción en WMS (header + detail)
        //    var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData, ct);
        //    if (!wmsTransaction.Success)
        //    {
        //        return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //            $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error}.");
        //    }
        //    // Paso 2: Transacción en Zebra
        //    try
        //    {
        //        await using var tx = await _context.Database.BeginTransactionAsync(ct);

        //        //create IrReceip hdr and detail transction

        //        var infoIrReceipt = await _transactionsASNINVRepository.CreateIrReceiptHdrAndDetailLog(RequestASNData, detaillist, ct);

        //        if (infoIrReceipt <= 0)
        //        {
        //            await tx.RollbackAsync(ct);
        //            return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //           $"Error creating Receipt Transaction for this ASN :{AsnId}.");
        //        }

        //        var groupedByItemSerial = detaillist
        //           .Where(d => !string.IsNullOrWhiteSpace(d.partnumber)
        //                    && !string.IsNullOrWhiteSpace(d.serialnumber))
        //           .GroupBy(d => new
        //           {
        //               Part = "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant(),
        //               Serial = d.serialnumber.Trim().ToUpperInvariant()
        //           })
        //           .Select(g => new
        //           {
        //               PartNumber = g.Key.Part,
        //               SerialNumber = g.Key.Serial,
        //               Qty = g.Sum(x => x.qty)
        //           })
        //           .ToList();


        //        var partNumbers = groupedByItemSerial.Select(x => x.PartNumber).Distinct().ToList();


        //        var items = await _context.TzebB2bMasterPartDefinition
        //                .Where(i => partNumbers.Contains(i.PartNo))
        //                .Select(i => new { i.Id, i.PartNo })
        //                .ToListAsync(ct);

        //        var idByPart = items.ToDictionary(x => x.PartNo, x => x.Id, StringComparer.OrdinalIgnoreCase);

        //        var result = (
        //            from g in groupedByItemSerial                   // { PartNumber, SerialNumber, Qty }
        //            join i in items on g.PartNumber equals i.PartNo // items: { Id, PartNo }
        //            select new
        //            {
        //                ItemId = i.Id,
        //                PartNumber = g.PartNumber,   // con "ZEBRA-"
        //                SerialNumber = g.SerialNumber,
        //                Qty = g.Qty
        //            }
        //        ).ToList(); // List<anon>



        //        List<TzebB2bReplacementPartsInventoryLogDto> listItemDetail = new List<TzebB2bReplacementPartsInventoryLogDto>();

        //        foreach (var item in result)
        //        {

        //            var datalog = new TzebB2bReplacementPartsInventoryLogDto
        //            {

        //                PartDefinitionId = item.ItemId,
        //                InventoryTypeIdSource = InvSource,
        //                InventoryTypeIdDest = InvDest,
        //                SerialNo = item.SerialNumber,
        //                Qty = item.Qty,
        //                Notes = AsnId,
        //                RepairNo = null,
        //                DateInserted = DateTime.Now

        //            };

        //            listItemDetail.Add(datalog);

        //        }

        //        //create inventory movement log and update inventory table 

        //        var infotran = _transactionsCommonINVRepository.createInventoryTransactionLog(listItemDetail, _currentUser.UserId!, ct);


        //        //update HDR ASN table with processed true

        //        infoAsn.Processed = true;

        //        _context.TzebB2bAsnOutHdrDetItemInbConsReqs.Update(infoAsn);
        //        await _context.SaveChangesAsync(ct);

        //        //update receive ASN call with process true

        //        infocallreceiving.Processed = true;

        //        _context.TzebB2bInbConsReqs.Update(infocallreceiving);
        //        await _context.SaveChangesAsync(ct);

        //        //create out put consigment call

        //        TzebB2bOutConsReqsInsertDto objcallout = new TzebB2bOutConsReqsInsertDto();

        //        objcallout.TypeOfCallId = TypeCallId;
        //        objcallout.TransactionCode = TransactionCode;


        //        var outputCallNUmber = await _transactionsCommonINVRepository.createConsOutPutCall(objcallout, ct);

        //        if (outputCallNUmber <= 0)
        //        {
        //            await tx.RollbackAsync(ct);
        //            return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //           $"Error creating ASN OutPut Call Transaction for this ASN :{AsnId}.");
        //        }

        //        //update IRecepit Hdr with ID output call

        //        var infoRECEIPT = await _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.FirstOrDefaultAsync(x => x.Id == infoIrReceipt);

        //        if (infoRECEIPT == null) {
        //            await tx.RollbackAsync(ct);
        //            return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //           $"Receipt Transaction for this ASN :{AsnId} not found .");
        //        }


        //        infoRECEIPT.ConsignDbkrequestId = outputCallNUmber;

        //        _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog.Update(infoRECEIPT);
        //        await _context.SaveChangesAsync(ct);

        //        var updatecall = await _transactionsCommonINVRepository.updateConsOutPutCallReadyToProcess(outputCallNUmber, ct);


        //        if (!updatecall)
        //        {
        //            await tx.RollbackAsync(ct);
        //            return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
        //           $"Error updating Output call {outputCallNUmber} for this ASN :{AsnId}.");

        //        }


        //        await tx.CommitAsync(ct);

        //        PickProcessResponseDto objres = new PickProcessResponseDto();


        //        return ApiResponseFactory.Ok(objres, "Pick process completed successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        // rollback zebra + compensar wms
        //        //await _transactionsWMSINVService.DeleteInventoryTransaction(objresponse.WMSTransactionNumber, ct);
        //        return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.Message}");
        //    }
        //}


    }
}

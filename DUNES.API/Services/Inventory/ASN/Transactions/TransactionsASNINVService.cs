using DUNES.API.Data;
using DUNES.API.Models.Inventory.ASN;
using DUNES.API.Models.Inventory.Common;
using DUNES.API.Models.Masters;
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
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DUNES.API.Services.Inventory.ASN.Transactions
{
    /// <summary>
    /// ASN Transactions
    /// </summary>
    public class TransactionsASNINVService : ITransactionsASNINVService
    {


        private readonly LogHelper _logHelper;
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
        /// <param name="logHelper"></param>
        public TransactionsASNINVService(appWmsDbContext wmscontext, ITransactionsWMSINVService transactionsWMSINVService,
            ITransactionsASNINVRepository transactionsASNINVRepository, AppDbContext context,
            ITransactionsCommonINVRepository transactionsCommonINVRepository,
            ICurrentUser currentUser, ITraceProvider trace, IRequestInfo requestInfo, LogHelper logHelper)
        {
            _context = context;
            _transactionsWMSINVService = transactionsWMSINVService;
            _wmscontext = wmscontext;
            _transactionsASNINVRepository = transactionsASNINVRepository;
            _transactionsCommonINVRepository = transactionsCommonINVRepository;
            _currentUser = currentUser;
            _trace = trace;
            _requestInfo = requestInfo;
            _logHelper = logHelper;
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
        public async Task<ApiResponse<ASNResponseDto>> CreateASNReceivingTransaction(
                    string AsnId,
                    NewInventoryTransactionTm objInvData,
                    string trackingNumber,
                    List<BinsToLoadWm> detaillist,
                    CancellationToken ct)
        {
            // 0) Autenticación
            if (!_currentUser.IsAuthenticated)
                return ApiResponseFactory.Unauthorized<ASNResponseDto>("User Unauthorized");

            // 1) Carga y validaciones previas (solo lectura)
            var infoAsn = await _context.TzebB2bAsnOutHdrDetItemInbConsReqs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ShipmentNum == AsnId, ct);

            if (infoAsn == null)
                return ApiResponseFactory.BadRequest<ASNResponseDto>($"ASN Information not found {AsnId}.");

            if (infoAsn.Processed == true)
                return ApiResponseFactory.BadRequest<ASNResponseDto>($"ASN {AsnId} already processed.");

            var infocallreceiving = await _context.TzebB2bInbConsReqs
                .FirstOrDefaultAsync(x => x.Id == infoAsn.ConsignRequestId, ct);

            if (infocallreceiving == null)
                return ApiResponseFactory.BadRequest<ASNResponseDto>($"This ASN {AsnId} has not an input call.");

            // Agrupar cantidades por línea
            var grouped = detaillist
                .GroupBy(x => x.asnlineid)
                .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) })
                .ToList();

            var lineIds = grouped.Select(g => g.lineid).ToList();

            // Validar que existan las líneas

            //este instruccion (var linesFound= ....) produce este error Microsoft.Data.SqlClient.SqlException: 'Incorrect syntax near '$'.
            //se debe a la compatibilidad de bd debe estar en 160 y la nuestra este en 100
            //Ese “160” se llama oficialmente el Database Compatibility Level
            //(nivel de compatibilidad de la base de datos).

            //var linesFound = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
            //    .Where(x => lineIds.Contains(x.Id))
            //    .Select(x => x.Id)
            //    .ToListAsync(ct);


            var parms = lineIds.Select((id, i) => new SqlParameter($"@p{i}", id)).ToArray();
            var values = string.Join(",", lineIds.Select((_, i) => $"(@p{i})"));

            var sql = $@"
                    SELECT t.* 
                    FROM dbo._TZEB_B2B_ASN_LINE_ITEM_TBL_ITEM_Inb_Cons_Reqs AS t
                    JOIN (VALUES {values}) AS v(Id) ON v.Id = t.Id";   // <-- un solo SELECT composable

            var linesFound = await _context
                .Set<TzebB2bAsnLineItemTblItemInbConsReqs>()
                .FromSqlRaw(sql, parms)           // composable
                .AsNoTracking()
                .Select(x => x.Id)                // ya puedes componer
                .ToListAsync(ct);




            var missingLineIds = lineIds.Except(linesFound).ToList();
            if (missingLineIds.Count > 0)
                return ApiResponseFactory.BadRequest<ASNResponseDto>(
                    $"ASN lines not found: {string.Join(", ", missingLineIds)}");



            // Validación Item Master (evitar N+1 con join)
            var itemPartNumbers = detaillist
                .Where(d => !string.IsNullOrWhiteSpace(d.partnumber))
                .Select(d => d.partnumber.ToString().Contains("ZEBRA") ? d.partnumber.Trim().ToUpperInvariant() : "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant())
                .Distinct()
                .ToList();


            var parms1 = itemPartNumbers
                .Select((val, i) =>
                {
                    var p = new SqlParameter($"@p{i}", SqlDbType.NVarChar, 100); // 100 o el largo real de PartNo
                    p.Value = val;
                    return p;
                })
                .ToArray();

            // 3) Crea el bloque VALUES (@p0),(@p1)...
            var values1 = string.Join(",", itemPartNumbers.Select((_, i) => $"(@p{i})"));

            // 4) SQL de un solo SELECT (componible). Selecciona t.* para que EF lo pueda mapear a la entidad.
            var sql1 = $@"
                    SELECT t.*
                    FROM dbo._TZEB_B2B_Master_Part_Definition AS t
                    JOIN (VALUES {values1}) AS v(PartNo) ON v.PartNo = t.PartNo";

            // 5) Ejecuta y ahora sí puedes componer el .Select(...)
            var partsExisting = await _context.TzebB2bMasterPartDefinition
                .FromSqlRaw(sql1, parms1)    // <- composable
                .AsNoTracking()
                .Select(i => i.PartNo)
                .ToListAsync(ct);

            //var partsExisting = await _context.TzebB2bMasterPartDefinition
            //    .Where(i => itemPartNumbers.Contains(i.PartNo))
            //    .Select(i => i.PartNo)
            //    .ToListAsync(ct);

            var missingParts = itemPartNumbers.Except(partsExisting, StringComparer.OrdinalIgnoreCase).ToList();
            if (missingParts.Count > 0)
                return ApiResponseFactory.BadRequest<ASNResponseDto>(
                    $"Not all items are in Item Master Table. Missing: {string.Join(", ", missingParts)}");

            // Parámetros de inventario
            var infoparam = await _context.MvcGeneralParameters
                .Where(x => x.ParameterNumber <= 4)
                .ToListAsync(ct);

            // BUG original: chequeabas "!= null". Debe validar null o cantidad insuficiente
            if (infoparam == null || infoparam.Count < 4)
                return ApiResponseFactory.BadRequest<ASNResponseDto>("There are no Inventory Transactions parameters.");

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
                return ApiResponseFactory.BadRequest<ASNResponseDto>("There are no Inventory Transactions parameters.");

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
            int wmsTxNumber = 0;
            {
                var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData, ct);
                if (!wmsTransaction.Success)
                {
                    return ApiResponseFactory.BadRequest<ASNResponseDto>(
                        $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error}.");
                }
                wmsTxNumber = Convert.ToInt32(wmsTransaction.Data);
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
                    await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,  wmsTxNumber, ct);
                    return ApiResponseFactory.BadRequest<ASNResponseDto>(
                        $"Error creating Receipt Transaction for this ASN: {AsnId}.");
                }

                // 3.2) Agrupar por Part+Serial y mapear a IDs
                //var groupedByItemSerial = detaillist
                //    .Where(d => !string.IsNullOrWhiteSpace(d.partnumber) && !string.IsNullOrWhiteSpace(d.serialnumber))
                //    .GroupBy(d => new
                //    {
                //        Part = "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant(),
                //        Serial = d.serialnumber.Trim().ToUpperInvariant()
                //    })
                //    .Select(g => new { PartNumber = g.Key.Part, SerialNumber = g.Key.Serial, Qty = g.Sum(x => x.qty) })
                //    .ToList();




                //var items = await _context.TzebB2bMasterPartDefinition
                //    .Where(i => itemPartNumbers.Contains(i.PartNo))
                //    .Select(i => new { i.Id, i.PartNo })
                //    .ToListAsync(ct);

                // 3.2) Agrupar por Part+Serial y normalizar

                //var groupedByItemSerial = detaillist
                //    .Where(d => !string.IsNullOrWhiteSpace(d.partnumber) && !string.IsNullOrWhiteSpace(d.serialnumber))
                //    .GroupBy(d => new
                //    {
                //        Part = d.partnumber.ToString().Contains("ZEBRA-")? d.partnumber.Trim() : "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant(),
                //        Serial = d.serialnumber.Trim().ToUpperInvariant()
                //    })
                //    .Select(g => new { PartNumber = g.Key.Part, SerialNumber = g.Key.Serial, Qty = g.Sum(x => x.qty) })
                //    .ToList();


                var groupedByItemSerial = detaillist
                .Where(d => !string.IsNullOrWhiteSpace(d.partnumber))
                .Select(d => new {
                    Part = (d.partnumber ?? "").Trim(),
                    Serial = (d.serialnumber ?? "").Trim(),
                    Qty = d.qty
                })
                .Select(x => new {
                    Part = x.Part.StartsWith("ZEBRA-", StringComparison.OrdinalIgnoreCase)
                           ? x.Part.ToUpperInvariant()
                           : "ZEBRA-" + x.Part.ToUpperInvariant(),
                    Serial = string.IsNullOrWhiteSpace(x.Serial) ? "" : x.Serial.ToUpperInvariant(),
                    Qty = x.Qty
                })
                .GroupBy(x => new { x.Part, x.Serial })
                .Select(g => new { PartNumber = g.Key.Part, SerialNumber = g.Key.Serial, Qty = g.Sum(i => i.Qty) })
                .ToList();



                // Distintos PartNumbers (≤ 50 según lo que comentaste)
                var partNos = groupedByItemSerial
                    .Select(x => x.PartNumber)
                    .Distinct()
                    .ToList();



                // Si no hay parts, evita armar SQL inválido
                if (partNos.Count == 0)
                {
                    // No hay nada que buscar
                    var items = new List<(long Id, string PartNo)>();
                    // ... sigue tu flujo
                }
                else
                {
                    // Parámetros tipados (ajusta el length al de tu columna PartNo)
                    var parms2 = partNos
                        .Select((val, i) =>
                        {
                            var p = new SqlParameter($"@p{i}", SqlDbType.NVarChar, 100);
                            p.Value = val;
                            return p;
                        })
                        .ToArray();

                    // Bloque VALUES (@p0),(@p1),...
                    var values2 = string.Join(",", partNos.Select((_, i) => $"(@p{i})"));

                    // Un solo SELECT composable (NO usa OPENJSON)
                    var sql2 = $@"
                        SELECT t.*
                        FROM dbo._TZEB_B2B_Master_Part_Definition AS t
                        JOIN (VALUES {values2}) AS v(PartNo) ON v.PartNo = t.PartNo";

                    // Traes las entidades y puedes componer si quieres
                    var itemsRows = await _context.TzebB2bMasterPartDefinition
                        .FromSqlRaw(sql2, parms2)
                        .AsNoTracking()
                        .Select(i => new { i.Id, i.PartNo })
                        .ToListAsync(ct);

                    // Diccionario útil: PartNo -> Id
                    var partNoToId = itemsRows.ToDictionary(x => x.PartNo, x => x.Id);

                    //// (Opcional) Unir con tu groupedByItemSerial para quedarte con los que existen en master:
                    //var groupedWithIds = groupedByItemSerial
                    //    .Where(g => partNoToId.ContainsKey(g.PartNumber))
                    //    .Select(g => new
                    //    {
                    //        PartId = partNoToId[g.PartNumber],
                    //        g.PartNumber,
                    //        g.SerialNumber,
                    //        g.Qty
                    //    })
                    //    .ToList();




                    // 'itemsRows' equivale a lo que querías en tu query original:
                    // var items = await _context.TzebB2bMasterPartDefinition
                    //     .Where(i => itemPartNumbers.Contains(i.PartNo))
                    //     .Select(i => new { i.Id, i.PartNo })
                    //     .ToListAsync(ct);





                    //////////////

                    var idByPart = itemsRows.ToDictionary(x => x.PartNo, x => x.Id, StringComparer.OrdinalIgnoreCase);

                    var movementLogs = new List<TzebB2bReplacementPartsInventoryLogDto>(groupedByItemSerial.Count);


                    foreach (var it in groupedByItemSerial)
                    {
                        if (!idByPart.TryGetValue(it.PartNumber, out var itemId))
                        {
                            await tx.RollbackAsync(ct);
                            await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!, wmsTxNumber, ct);
                            return ApiResponseFactory.BadRequest<ASNResponseDto>($"Part not found: {it.PartNumber}.");
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
                        await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,wmsTxNumber, ct);
                        return ApiResponseFactory.BadRequest<ASNResponseDto>(
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
                        await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,wmsTxNumber, ct);
                        return ApiResponseFactory.BadRequest<ASNResponseDto>(
                            $"Error creating ASN OutPut Call Transaction for this ASN: {AsnId}.");
                    }

                    // 3.7) Vincular el IR Receipt con el output call
                    var infoRECEIPT = await _context.TzebB2bIrReceiptOutHdrDetItemInbConsReqsLog
                        .FirstOrDefaultAsync(x => x.Id == infoIrReceipt, ct);

                    if (infoRECEIPT == null)
                    {
                        await tx.RollbackAsync(ct);
                        await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,wmsTxNumber, ct);
                        return ApiResponseFactory.BadRequest<ASNResponseDto>(
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
                        await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,wmsTxNumber, ct);
                        return ApiResponseFactory.BadRequest<ASNResponseDto>(
                            $"Error updating Output call {outputCallNumber} for ASN: {AsnId}.");
                    }

                }

                // 3.9) Commit final
                await tx.CommitAsync(ct);

                // 4) OK
                var objres = new ASNResponseDto
                {
                    // llena aquí lo que necesite el front (IDs, wmsTxNumber, etc.)
                };

                return ApiResponseFactory.Ok(objres, "Pick process completed successfully");
            }
            catch (Exception ex)
            {
                // Compensación WMS si algo explota en Zebra
                await TryCompensateWmsAsync(objInvData.hdr.Idcompany, objInvData.hdr.Codecompanyclient!,wmsTxNumber, ct);
                return ApiResponseFactory.Error<ASNResponseDto>($"ASN process failed: {ex.GetBaseException().Message}");
            }
        }

        // Helper de compensación (best-effort)
        private async Task TryCompensateWmsAsync(int companyid, string companyClientId, int wmsTxNumber, CancellationToken ct)
        {
            if (wmsTxNumber == 0) return;
            try
            {
                await _transactionsWMSINVService.DeleteInventoryTransaction(companyid, companyClientId, Convert.ToInt32(wmsTxNumber), ct);
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




    }
}

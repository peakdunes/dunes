using DUNES.API.Data;
using DUNES.API.Models.Inventory.Common;
using DUNES.API.ReadModels.Inventory;
using DUNES.API.Repositories.Inventory.ASN.Transactions;
using DUNES.API.Repositories.Inventory.Common.Transactions;
using DUNES.API.Services.Auth;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        /// <summary>
        ///  dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="wmscontext"></param>
        /// <param name="currentUser"></param>
        /// <param name="transactionsWMSINVService"></param>
        /// <param name="transactionsASNINVRepository"></param>
        /// <param name="transactionsCommonINVRepository"></param>
        public TransactionsASNINVService(appWmsDbContext wmscontext, ITransactionsWMSINVService transactionsWMSINVService,
            ITransactionsASNINVRepository transactionsASNINVRepository, AppDbContext context,
            ITransactionsCommonINVRepository transactionsCommonINVRepository,
            ICurrentUser currentUser)
        {
            _context = context;
            _transactionsWMSINVService = transactionsWMSINVService;
            _wmscontext = wmscontext;
            _transactionsASNINVRepository = transactionsASNINVRepository;
            _transactionsCommonINVRepository = transactionsCommonINVRepository;
            _currentUser = currentUser;

        }
        /// <summary>
        /// Perform ASN Receiving Transaction
        /// </summary>
        /// <param name="AsnId"></param>
        /// <param name="objInvData"></param>
        /// <param name="trackingNumber"></param>
        /// <param name="detaillist"></param>
        /// <param name="User"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// 


        public async Task<ApiResponse<PickProcessResponseDto>> CreateASNReceivingTransaction(string AsnId,
            NewInventoryTransactionTm objInvData, string trackingNumber, List<BinsToLoadWm> detaillist,  CancellationToken ct)
        {

            AsnReceivedHdrLogRead RequestASNData = new AsnReceivedHdrLogRead();

            if (!_currentUser.IsAuthenticated)
                return ApiResponseFactory.Unauthorized<PickProcessResponseDto>(
                    $"User Unauthorized");

            //check ASN Head info
            var infoAsn = await _context.TzebB2bAsnOutHdrDetItemInbConsReqs.FirstOrDefaultAsync(x => x.ShipmentNum == AsnId);

            if (infoAsn == null)
            {
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                   $"ASN Information not found {AsnId}.");
            }

            RequestASNData.asnNumber = AsnId;
            RequestASNData.TransactionCode = "SRA";
            RequestASNData.TransactionType = "IR RECEIPT";
            RequestASNData.ProcessName = "Receiving Tool";
            RequestASNData.org3pl = infoAsn.OrgSystemId3pl;
            RequestASNData.locator3pl = string.Empty;
            RequestASNData.IsRtvPart = false;
            RequestASNData.IsCePart = false;


            //check ASN Detail info

            var listgroup = detaillist.GroupBy(x => x.asnlineid)
          .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) }).ToList();

            foreach (var item in listgroup)
            {

                var infoline = _context.TzebB2bAsnLineItemTblItemPartialInbConsReqs.FirstOrDefaultAsync(x => x.Id == item.lineid);

                if (infoline == null)
                {
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                                      $"ASN line {item.lineid} not found.");
                }

            }


            var grouped = detaillist
              .GroupBy(x => x.asnlineid)
              .Select(g => new { lineid = g.Key, qty = g.Sum(x => x.qty) })
              .ToList();

            var lineIds = grouped.Select(g => g.lineid).ToList();

            var infoByLine = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
                .Where(x => lineIds.Contains(x.Id)).ToListAsync();

            //validating all items exist in item master

            bool IsMissing = await _context.TzebB2bAsnLineItemTblItemInbConsReqs
            .Where(l => lineIds.Contains(l.Id))
            .AnyAsync(l => !_context.TzebB2bMasterPartDefinition.Any(i => i.PartNo == "ZEBRA-" + l.ItemNumber), ct);

            if (IsMissing)
            {
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                                     $"Not all items are in Item Master Table.");
            }

            //validating parameters

            var infoparam = await _context.MvcGeneralParameters.Where(x => x.ParameterNumber <= 2).ToListAsync();

            if (infoparam != null)
            {
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                    $"There is not Inventory Transactions parameters.");
            }

            int InvSource = 0;
            int InvDest = 0;

            foreach (var info in infoparam!)
            {
                if (info.ParameterNumber == 1)
                {
                    InvSource = Convert.ToInt32(info.ParameterValue);
                }
                if (info.ParameterNumber == 2)
                {
                    InvDest = Convert.ToInt32(info.ParameterValue);
                }
            }

            if (InvSource == 0 || InvDest == 0)
            {
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                    $"There is not Inventory Transactions parameters.");
            }

            // Paso 1: Transacción en WMS (header + detail)
            var wmsTransaction = await _transactionsWMSINVService.CreateInventoryTransaction(objInvData, ct);
            if (!wmsTransaction.Success)
            {
                return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                    $"Error creating WMS Inventory transaction. Error {wmsTransaction.Error}.");
            }
            // Paso 2: Transacción en Zebra
            try
            {
                await using var tx = await _context.Database.BeginTransactionAsync(ct);

                var infoIrReceipt = await _transactionsASNINVRepository.CreateIrReceiptHdrAndDetailLog(RequestASNData, detaillist, ct);

                if (infoIrReceipt <= 0)
                {
                    await tx.RollbackAsync(ct);
                    return ApiResponseFactory.BadRequest<PickProcessResponseDto>(
                   $"Error creating Receipt Transaction for this ASN :{AsnId}.");
                }

                var groupedByItemSerial = detaillist
                   .Where(d => !string.IsNullOrWhiteSpace(d.partnumber)
                            && !string.IsNullOrWhiteSpace(d.serialnumber))
                   .GroupBy(d => new
                   {
                       Part = "ZEBRA-" + d.partnumber.Trim().ToUpperInvariant(),
                       Serial = d.serialnumber.Trim().ToUpperInvariant()
                   })
                   .Select(g => new
                   {
                       PartNumber = g.Key.Part,
                       SerialNumber = g.Key.Serial,
                       Qty = g.Sum(x => x.qty)
                   })
                   .ToList();


                var partNumbers = groupedByItemSerial.Select(x => x.PartNumber).Distinct().ToList();


                var items = await _context.TzebB2bMasterPartDefinition
                        .Where(i => partNumbers.Contains(i.PartNo))
                        .Select(i => new { i.Id, i.PartNo })
                        .ToListAsync(ct);

                var idByPart = items.ToDictionary(x => x.PartNo, x => x.Id, StringComparer.OrdinalIgnoreCase);

                var result = (
                    from g in groupedByItemSerial                   // { PartNumber, SerialNumber, Qty }
                    join i in items on g.PartNumber equals i.PartNo // items: { Id, PartNo }
                    select new
                    {
                        ItemId = i.Id,
                        PartNumber = g.PartNumber,   // con "ZEBRA-"
                        SerialNumber = g.SerialNumber,
                        Qty = g.Qty
                    }
                ).ToList(); // List<anon>



                List<TzebB2bReplacementPartsInventoryLogDto> listItemDetail = new List<TzebB2bReplacementPartsInventoryLogDto>();

                foreach (var item in result)
                {

                    var datalog = new TzebB2bReplacementPartsInventoryLogDto
                    {

                        PartDefinitionId = item.ItemId,
                        InventoryTypeIdSource = InvSource,
                        InventoryTypeIdDest = InvDest,
                        SerialNo = item.SerialNumber,
                        Qty = item.Qty,
                        Notes = AsnId,
                        RepairNo = null,
                        DateInserted = DateTime.Now

                    };

                    listItemDetail.Add(datalog);

                }

                var infotran = _transactionsCommonINVRepository.createInventoryTransactionLog(listItemDetail, _currentUser.UserId!, ct);



                await tx.CommitAsync(ct);

                PickProcessResponseDto objres = new PickProcessResponseDto();


                return ApiResponseFactory.Ok(objres, "Pick process completed successfully");
            }
            catch (Exception ex)
            {
                // rollback zebra + compensar wms
                //await _transactionsWMSINVService.DeleteInventoryTransaction(objresponse.WMSTransactionNumber, ct);
                return ApiResponseFactory.Error<PickProcessResponseDto>($"Pick process failed: {ex.Message}");
            }
        }

        public Task<ApiResponse<PickProcessResponseDto>> CreateASNReceivingTransaction(string AsnId, NewInventoryTransactionTm objInvData, string trackingNumber, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

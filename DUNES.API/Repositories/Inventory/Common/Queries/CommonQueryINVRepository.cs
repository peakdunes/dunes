using AutoMapper;
using DUNES.API.Data;
using DUNES.API.Models.Inventory;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{
    /// <summary>
    /// All common inventory queries
    /// </summary>
    public class CommonQueryINVRepository : ICommonQueryINVRepository
    {

        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public CommonQueryINVRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all inventory transactions for a Document Number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        public async Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct)
        {
            var infotran = await (from log in _context.TzebB2bReplacementPartsInventoryLog
                                  join masterInv in _context.TzebB2bMasterPartDefinition
                                  on log.PartDefinitionId equals masterInv.Id
                                  join masterInvSource in _context.TzebB2bInventoryType
                                  on log.InventoryTypeIdSource equals masterInvSource.Id
                                  join masterInvDest in _context.TzebB2bInventoryType
                                on log.InventoryTypeIdDest equals masterInvDest.Id
                                  where log.Notes.Contains(DocumentNumber)
                                  select new TzebB2bReplacementPartsInventoryLogDto
                                  {
                                      Id = log.Id,
                                      PartDefinitionId = log.PartDefinitionId,
                                      PartNumberName = masterInv.PartNo,
                                      InventoryTypeIdSource = log.InventoryTypeIdSource,
                                      InvSourceName = masterInvSource.Name,
                                      InventoryTypeIdDest = log.InventoryTypeIdDest,
                                      InvDestName = masterInvDest.Name,
                                      SerialNo = log.SerialNo,
                                      Qty = log.Qty,
                                      Notes = string.IsNullOrEmpty(log.Notes) ? "" : log.Notes.Trim(),
                                      RepairNo = log.RepairNo,
                                      DateInserted = log.DateInserted
                                  }).ToListAsync(ct);

            return infotran;
        }


        /// <summary>
        /// Get all inventory transactions for a Document Number and a search Start Date
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate, CancellationToken ct)
        {
            var infotran = await (from log in _context.TzebB2bReplacementPartsInventoryLog
                                  join masterInv in _context.TzebB2bMasterPartDefinition
                                  on log.PartDefinitionId equals masterInv.Id
                                  join masterInvSource in _context.TzebB2bInventoryType
                                  on log.InventoryTypeIdSource equals masterInvSource.Id
                                  join masterInvDest in _context.TzebB2bInventoryType
                                on log.InventoryTypeIdDest equals masterInvDest.Id
                                  where log.Notes.Contains(DocumentNumber) && log.DateInserted >= startDate
                                  select new TzebB2bReplacementPartsInventoryLogDto
                                  {
                                      Id = log.Id,
                                      PartDefinitionId = log.PartDefinitionId,
                                      PartNumberName = masterInv.PartNo,
                                      InventoryTypeIdSource = log.InventoryTypeIdSource,
                                      InvSourceName = masterInvSource.Name,
                                      InventoryTypeIdDest = log.InventoryTypeIdDest,
                                      InvDestName = masterInvDest.Name,
                                      SerialNo = log.SerialNo,
                                      Qty = log.Qty,
                                      Notes = string.IsNullOrEmpty(log.Notes) ? "" : log.Notes.Trim(),
                                      RepairNo = log.RepairNo,
                                      DateInserted = log.DateInserted
                                  }).ToListAsync(ct);

            return infotran;
        }



        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <returns></returns>
        public async Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct)
        {
            var infotran = await (from log in _context.TzebB2bReplacementPartsInventoryLog
                                  join masterInv in _context.TzebB2bMasterPartDefinition
                                  on log.PartDefinitionId equals masterInv.Id
                                  join masterInvSource in _context.TzebB2bInventoryType
                                  on log.InventoryTypeIdSource equals masterInvSource.Id
                                  join masterInvDest in _context.TzebB2bInventoryType
                                on log.InventoryTypeIdDest equals masterInvDest.Id
                                  where log.PartDefinitionId == PartNumberId
                                  select new TzebB2bReplacementPartsInventoryLogDto
                                  {
                                      Id = log.Id,
                                      PartDefinitionId = log.PartDefinitionId,
                                      PartNumberName = masterInv.PartNo,
                                      InventoryTypeIdSource = log.InventoryTypeIdSource,
                                      InvSourceName = masterInvSource.Name,
                                      InventoryTypeIdDest = log.InventoryTypeIdDest,
                                      InvDestName = masterInvDest.Name,
                                      SerialNo = log.SerialNo,
                                      Qty = log.Qty,
                                      Notes = string.IsNullOrEmpty(log.Notes) ? "" : log.Notes.Trim(),
                                      RepairNo = log.RepairNo,
                                      DateInserted = log.DateInserted
                                  }).ToListAsync(ct);

            return infotran;
        }

        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <returns></returns>
        public async Task<List<TdivisionCompany>> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct)
        {
            var infoDivision = await _context.TdivisionCompany
                .Where(x => x.CompanyDsc == CompanyClient && x.Active == true).ToListAsync(ct);

            return infoDivision;
        }


        /// <summary>
        /// Gell all (input, output) calls for an pick process
        /// </summary>
        /// <param name="DocumentId"></param>
        /// <param name="processtype">one of these (ANS-PICKPROCESS-B2B)</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ProcessCallsRead?> GetAllCalls(string DocumentId, string processtype, CancellationToken ct)
        {


            ProcessCallsRead pickcalls = new ProcessCallsRead();
            List<TzebB2bInbConsReqs> listInputCalls = new List<TzebB2bInbConsReqs>();
            List<TzebB2bOutConsReqs> listOutputCalls = new List<TzebB2bOutConsReqs>();

            switch (processtype)
            {
                case "PICKPROCESS":

                    var infodelivery = await _context.TzebB2bPSoWoHdrTblItemInbConsReqsLog.FirstOrDefaultAsync(x => x.DeliveryId == DocumentId);

                    if (infodelivery == null)
                    {
                        return null;
                    }


                    var listcallsIn = await _context.TzebB2bInbConsReqs.Where(x => x.Id == infodelivery.ConsignRequestId).ToListAsync();

                    if (listcallsIn.Any())
                    {
                        listInputCalls = listcallsIn;
                    }

                    //we check if there is pick-reponse call

                    DateTime datecall = DateTime.Now;

                    if (infodelivery.OutConsReqsId > 0)
                    {
                        if (infodelivery.DateTimeConfirmed != null)
                        {
                            datecall = Convert.ToDateTime(infodelivery.DateTimeConfirmed);

                            // datecall = Convert.ToDateTime("09/20/2025");



                            var listcalls = await (
                             from f in _context.TzebB2bInbConsReqsFullXmls
                             join c in _context.TzebB2bInbConsReqs on f.Id equals c.Id
                             where f.FullXml.Contains(infodelivery.DeliveryId!) && f.DateTimeInserted >= datecall
                             select c
                         ).ToListAsync();


                            if (listcalls.Any())
                            {
                                listInputCalls.AddRange(listcalls);

                            }

                        }


                    }
                    //finish pick-reponse call


                    var listcallsOut = await _context.TzebB2bOutConsReqs
                       .Where(x => x.Id == infodelivery.OutConsReqsId || x.Id == infodelivery.ShipOutConsReqsId).ToListAsync();

                    if (listcallsOut.Any())
                    {
                        listOutputCalls = listcallsOut;
                    }

                    foreach (var info in listOutputCalls)
                    {

                        var infcalldet = await _context.TzebB2bConsignmentCallsType.FirstOrDefaultAsync(x => x.Id == info.TypeOfCallId);
                        if (infcalldet != null)
                        {
                            info.callName = infcalldet.Name;
                        }



                    }

                    pickcalls.inputCalls = listInputCalls;
                    pickcalls.outputCalls = listOutputCalls;

                    break;

                case "ASN":

                    var infoasn = await _context.TzebB2bAsnOutHdrDetItemInbConsReqs.FirstOrDefaultAsync(x => x.ShipmentNum == DocumentId);

                    if (infoasn == null)
                    {
                        return null;
                    }


                    var listcallsInAsn = await _context.TzebB2bInbConsReqs.Where(x => x.Id == infoasn.ConsignRequestId).ToListAsync();

                    if (listcallsInAsn.Any())
                    {
                        listInputCalls = listcallsInAsn;
                    }

                    //we check if there is pick-reponse call

                    //DateTime datecallasn = DateTime.Now;

                    //if (infoasn.OutConsReqsId > 0)
                    //{
                    //    if (infodelivery.DateTimeConfirmed != null)
                    //    {
                    //        datecall = Convert.ToDateTime(infodelivery.DateTimeConfirmed);

                    //        // datecall = Convert.ToDateTime("09/20/2025");



                    //        var listcalls = await (
                    //         from f in _context.TzebB2bInbConsReqsFullXmls
                    //         join c in _context.TzebB2bInbConsReqs on f.Id equals c.Id
                    //         where f.FullXml.Contains(infodelivery.DeliveryId!) && f.DateTimeInserted >= datecall
                    //         select c
                    //     ).ToListAsync();


                    //        if (listcalls.Any())
                    //        {
                    //            listInputCalls.AddRange(listcalls);

                    //        }

                    //    }


                    //}
                    ////finish pick-reponse call


                    //var listcallsOutAsn = await _context.TzebB2bOutConsReqs
                    //   .Where(x => x.Id == infodelivery.OutConsReqsId || x.Id == infodelivery.ShipOutConsReqsId).ToListAsync();

                    //if (listcallsOutAsn.Any())
                    //{
                    //    listOutputCalls = listcallsOut;
                    //}

                    //foreach (var info in listOutputCalls)
                    //{

                    //    var infcalldet = await _context.TzebB2bConsignmentCallsType.FirstOrDefaultAsync(x => x.Id == info.TypeOfCallId);
                    //    if (infcalldet != null)
                    //    {
                    //        info.callName = infcalldet.Name;
                    //    }



                    //}

                    pickcalls.inputCalls = listInputCalls;
                    pickcalls.outputCalls = listOutputCalls;

                    foreach (var info in listOutputCalls)
                    {

                        var infcalldet = await _context.TzebB2bConsignmentCallsType.FirstOrDefaultAsync(x => x.Id == info.TypeOfCallId);
                        if (infcalldet != null)
                        {
                            info.callName = infcalldet.Name;
                        }
                    }

                    pickcalls.inputCalls = listInputCalls;
                    pickcalls.outputCalls = listOutputCalls;

                    break;
            }

            return pickcalls;
        }
    }
}

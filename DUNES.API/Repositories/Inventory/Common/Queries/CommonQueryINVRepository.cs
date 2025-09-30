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

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="context"></param>
        public CommonQueryINVRepository(AppDbContext context)
        {
            _context = context;
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
        public async Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate,CancellationToken ct)
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
    }
}

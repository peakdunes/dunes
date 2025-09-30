using DUNES.API.Models.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.Common.Queries
{
    /// <summary>
    /// All common inventory transactions queries
    /// </summary>
    public interface ICommonQueryINVService
    {
        /// <summary>
        /// get all inventory transactions for a document number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct);

        /// <summary>
        /// Get all inventory transactions for a Document Number and a search Start Date
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <param name="startDate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate, CancellationToken ct);
        

        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct);




        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct);
    }
}

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
        /// <returns></returns>
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocument(string DocumentNumber, CancellationToken ct);



        /// <summary>
        /// Get all inventory transactions for a Part Number ID
        /// </summary>
        /// <param name="PartNumberId"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByPartNumberId(int PartNumberId, CancellationToken ct);




        /// <summary>
        /// Get all Division for a company
        /// </summary>
        /// <param name="CompanyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(string CompanyClient, CancellationToken ct);
    }
}

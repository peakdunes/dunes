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
        Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocument(string DocumentNumber);
    }
}

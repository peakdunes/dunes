using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{ 
    /// <summary>
    /// All common inventory transactions queries
    /// </summary>
    public interface ICommonQueryINVRepository
    {
        /// <summary>
        /// Get all inventory transactions for a Document Number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        Task<List<TzebB2bReplacementPartsInventoryLogDto>> GetAllInventoryTransactionsByDocument(string DocumentNumber);
    }
}

using DUNES.API.ReadModels.Inventory;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{ 
    /// <summary>
    /// All common inventory queries
    /// </summary>
    public interface ICommonQueryINVRepository
    {
        /// <summary>
        /// Get all inventory transactions for a Document Number
        /// </summary>
        /// <param name="DocumentNumber"></param>
        /// <returns></returns>
        Task<ASNRead> GetAllInventoryTransactionsByDocument(string DocumentNumber);
    }
}

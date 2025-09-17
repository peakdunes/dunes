using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.ServicesWMS.Inventory.Transactions
{
    /// <summary>
    /// All WMS inventory transactions
    /// </summary>
    public interface ITransactionsWMSINVService
    {

        /// <summary>
        /// Create inventory transaction
        /// </summary>
        /// <param name="objcreate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<int>> CreateInventoryTransaction(NewInventoryTransactionTm objcreate, CancellationToken ct);
    }
}

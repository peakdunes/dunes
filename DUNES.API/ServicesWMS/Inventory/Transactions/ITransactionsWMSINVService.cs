using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using Microsoft.EntityFrameworkCore.Storage;

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


        /// <summary>
        /// Delete inventory transaction NO Processed (Header and details)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyClientId"></param>
        /// <param name="transactionNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteInventoryTransaction(int companyId, string companyClientId, int transactionNumber, CancellationToken ct);

    }
}

using DUNES.API.Models.Inventory;
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.Repositories.Inventory.Common.Transactions
{

    /// <summary>
    /// Common Transactions Inventory
    /// </summary>
    public interface ITransactionsCommonINVRepository
    {

        /// <summary>
        /// Create a inventory transaction log
        /// </summary>
        /// <returns></returns>
        Task<bool> createInventoryTransactionLog(List<TzebB2bReplacementPartsInventoryLogDto> listItemDetail, string user, CancellationToken ct);
    }
}

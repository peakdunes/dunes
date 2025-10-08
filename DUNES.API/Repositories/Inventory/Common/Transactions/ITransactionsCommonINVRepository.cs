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


        /// <summary>
        /// Create a new Cons Output call
        /// </summary>
        /// <param name="callInfo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<int> createConsOutPutCall(TzebB2bOutConsReqsInsertDto callInfo, CancellationToken ct);

        /// <summary>
        /// Leave a call ready to process
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> updateConsOutPutCallReadyToProcess (int id, CancellationToken ct);

    }
}

using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.Services.Inventory.ASN.Transactions
{

    /// <summary>
    /// ASN Transactions
    /// </summary>
    public interface ITransactionsASNINVService
    {

        /// <summary>
        /// Perform ASN processing
        /// </summary>
        /// <param name="AsnId"></param>
        /// <param name="objInvData"></param>
        /// <param name="trackingNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessResponseDto>> CreateASNReceivingTransaction(string AsnId, NewInventoryTransactionTm objInvData, string trackingNumber, CancellationToken ct);


    }
}

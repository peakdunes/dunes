using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.WiewModels.Inventory;

namespace DUNES.API.Repositories.Inventory.ASN.Transactions
{

    /// <summary>
    /// ASN transactions
    /// </summary>
    public interface ITransactionsASNINVRepository
    {

        /// <summary>
        /// Create a start Receiving process for a ASN and return record id
        /// </summary>
        /// <param name="DataLog"></param>
        /// <param name="detaillist"></param>
        /// <returns></returns>
        Task<int> CreateIrReceiptHdrAndDetailLog(AsnReceivedHdrLogRead DataLog, List<BinsToLoadWm> detaillist, CancellationToken ct);

        /// <summary>
        /// Insert line by line qty received by ASN
        /// </summary>
        /// <returns></returns>
        Task<bool> InsertQtyDetailByAsn(List<BinsToLoadWm> detaillist, string userid, CancellationToken ct);

        /// <summary>
        /// Update ASN Item detail with a qty received
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateASNDetail(List<BinsToLoadWm> detaillist, CancellationToken ct);

        

    }
}

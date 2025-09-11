using DUNES.API.DTOs.Inventory;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.Repositories.Inventory.PickProcess.Transactions
{   
    /// <summary>
    /// All pick-process transactions
    /// </summary>
    public interface ITransactionsPickProcessINVRepository
    {

        /// <summary>
        /// Get all information about a PickProcess
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ServTrackReferenceCreatedDto> CreateServTrackOrderFromPickProcess(string DeliveryId);
    }
}

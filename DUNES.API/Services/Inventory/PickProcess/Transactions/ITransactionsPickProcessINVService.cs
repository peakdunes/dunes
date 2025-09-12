using DUNES.API.DTOs.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.PickProcess.Transactions
{
    /// <summary>
    /// All pick-process transactions
    /// </summary>
    public interface ITransactionsPickProcessINVService
    {
        /// <summary>
        /// Create a servtrack order from delivery id
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ApiResponse<ServTrackReferenceCreatedDto>> CreateServTrackOrderFromPickProcess(string DeliveryId);



        /// <summary>
        /// Perform pick process processing
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<int> CreatePickProccessTransaction(string DeliveryId);

    }



}

using DUNES.API.DTOs.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

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
        /// <param name="objInvData"></param>
        /// <param name="lpnid"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(string DeliveryId, NewInventoryTransactionTm objInvData, string lpnid);


        /// <summary>
        /// Create a pick process call (13)
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ApiResponse<int>> CreatePickProcessCall(string DeliveryId);


        /// <summary>
        /// update pick process tables from pick process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="call13id"></param>
        /// <param name="LPNNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdatePickProcessTables(string DeliveryId, int call13id, string LPNNumber);
    }



}

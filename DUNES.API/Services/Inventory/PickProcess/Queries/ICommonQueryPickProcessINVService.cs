using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.PickProcess.Queries
{
 /// <summary>
 /// All Pick Process queries service
 /// </summary>
    public interface ICommonQueryPickProcessINVService
    {
        /// <summary>
        /// Get all information about a Pick Process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessDto>> GetPickProcessAllInfo(string DeliveryId);

        /// <summary>
        /// Get all (input, output) calls for a delivery id
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessCallsReadDto>> GetPickProcessAllCalls(string DeliveryId);
    }
}

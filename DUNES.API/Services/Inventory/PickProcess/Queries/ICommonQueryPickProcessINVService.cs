using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.API.Services.Inventory.PickProcess.Queries
{
    public interface ICommonQueryPickProcessINVService
    {
        /// <summary>
        /// Get all information about a Pick Process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessDto>> GetPickProcessAllInfo(string DeliveryId);
    }
}

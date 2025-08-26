using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;

namespace DUNES.API.Services.Inventory.Common.Queries
{

    /// <summary>
    /// All common Inventory queries (ASN - PickProcess
    /// </summary>
    public interface ICommonQueryINVService
    {
        /// <summary>
        /// Get all information about a ASN
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        Task <ApiResponse<ASNWm>> GetASNAllInfo(string ShipmentNum);




        /// <summary>
        /// Get all information about a Pick Process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessDto>> GetPickProcessAllInfo(string DeliveryId);
    }

}

using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.Repositories.Inventory.Common.Queries
{


    /// <summary>
    /// Get all information about inventory transaction (PICK-PROCESS and ASN)
    /// </summary>
    public interface ICommonQueryINVRepository
    {
        /// <summary>
        /// Get all information about a PickProcess
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<PickProcessRead> GetPickProcessAllInfo(string DeliveryId);

        /// <summary>
        /// Get ALl information about ASN
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        Task<ASNRead> GetASNAllInfo(string ShipmentNum);
    }
}

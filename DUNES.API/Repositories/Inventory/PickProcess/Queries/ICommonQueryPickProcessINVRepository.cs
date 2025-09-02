using DUNES.API.ReadModels.Inventory;

namespace DUNES.API.Repositories.Inventory.PickProcess.Queries
{

    /// <summary>
    /// All Inventory PickProcess
    /// </summary>
    public interface ICommonQueryPickProcessINVRepository
    {

        /// <summary>
        /// Get all information about a PickProcess
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<PickProcessRead> GetPickProcessAllInfo(string DeliveryId);
    }
}

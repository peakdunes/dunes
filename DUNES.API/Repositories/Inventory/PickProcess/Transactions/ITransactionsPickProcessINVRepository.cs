using DUNES.API.DTOs.Inventory;
using DUNES.API.Models.Inventory;
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
        /// Create All records in  torderrepair tables for a pick process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<ServTrackReferenceCreatedDto> CreateServTrackOrderFromPickProcess(string DeliveryId);

        /// <summary>
        /// Create a pick process call (number 13) for a pick process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <returns></returns>
        Task<int> CreatePickProcessCall(string DeliveryId);

        /// <summary>
        /// Update pickprocess table for pick and confirm process 
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="call13id"></param>
        /// <param name="LPNNumber"></param>
        /// <returns></returns>
        Task<int> UpdatePickProcessTables(string DeliveryId, int call13id, string LPNNumber);


       
    }
}

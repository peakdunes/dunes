using DUNES.API.Models.Inventory.PickProcess;
using DUNES.API.ReadModels.B2B;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

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
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<PickProcessRead?> GetPickProcessAllInfo(string DeliveryId, CancellationToken ct);


        /// <summary>
        /// get all pick process from a start date
        /// </summary>
        /// <param name="dateSearch"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<TzebB2bPSoWoHdrTblItemInbConsReqsLog>> GetAllPickProcessByStartDate(DateTime dateSearch,  CancellationToken ct);



        /// <summary>
        /// Displays the 4 tables associated with an Pick Process in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="ConsignRequestId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<OrderRepairFourTablesRead?> GetAllTablesOrderRepairCreatedByPickProcessAsync(string ConsignRequestId, CancellationToken ct);
    }
}

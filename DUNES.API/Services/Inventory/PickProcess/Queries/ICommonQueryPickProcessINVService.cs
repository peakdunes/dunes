using DUNES.API.ReadModels.B2B;
using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.API.Services.Inventory.PickProcess.Queries
{
 /// <summary>
 /// All Pick Process queries service
 /// </summary>
    public interface ICommonQueryPickProcessINVService
    {
        /// <summary>
        /// Get Header and Detail information about a Pick Process
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessRequestDto>> GetPickProcessAllInfo(string DeliveryId, CancellationToken ct);

        /// <summary>
        /// Get all (input, output) calls for a delivery id
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<PickProcessCallsReadDto>> GetPickProcessAllCalls(string DeliveryId,CancellationToken ct);



        /// <summary>
        /// Displays the 4 tables associated with an Pick Process in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="ConsignRequestId">This parameter is a field in the pick process header table</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TorderRepairTm>> GetAllTablesOrderRepairCreatedByPickProcessAsync(string ConsignRequestId, CancellationToken ct);
    }
}

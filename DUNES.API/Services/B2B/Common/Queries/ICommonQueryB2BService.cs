using DUNES.API.DTOs.B2B;
using DUNES.API.Models.B2b;
using DUNES.API.ReadModels.B2B;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;

namespace DUNES.API.Services.B2B.Common.Queries
{

    /// <summary>
    /// ALl queries in B2B
    /// </summary>
    public interface ICommonQueryB2BService
    {
        /// <summary>
        /// Display all information related to a repair
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<CheckRepairInformationDto>> GetRepairInfoAsync(int repairNumber, CancellationToken ct);

        /// <summary>
        /// Show RMA information for a serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebInBoundRequestsFile>>> GetRMAReceivingInfo(string serialNumber, CancellationToken ct);


        /// <summary>
        /// Check if a RMA was created correcty in our database (4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> GetAllRMATablesCreatedAsync(int refNo, CancellationToken ct);


        /// <summary>
        /// Displays the 4 tables associated with an order in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="refNo"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<OrderRepairFourTablesRead>> GetAllTablesOrderRepairCreatedAsync(int refNo, CancellationToken ct);




        /// <summary>
        /// Get the current area from the sql
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<AreaNameDto>> GetAreaByFunction(int repairNumber, CancellationToken ct);


        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialnumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<RepairReadyToReceiveDto>>> GetRepairReadyToReceive(string serialnumber, CancellationToken ct);

        /// <summary>
        /// get all date fields for a Repair Number
        /// </summary>
        /// <param name="refNumber"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<TorderRepairHdrDatesDto>> GetAllDateFieldsRepair(int refNumber, CancellationToken ct);


    }
}

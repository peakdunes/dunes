using APIZEBRA.DTOs.B2B;
using APIZEBRA.Models.B2b;
using APIZEBRA.Utils.Responses;

namespace APIZEBRA.Services.B2B.Common.Queries
{

    /// <summary>
    /// ALl queries in B2B
    /// </summary>
    public interface ICommonQueryService
    {
        /// <summary>
        /// Display all information related to a repair
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<CheckRepairInformationDto>> GetRepairInfoAsync(int repairNumber);

        /// <summary>
        /// Show RMA information for a serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<List<TzebInBoundRequestsFile>>> GetRMAReceivingInfo(string serialNumber);


        /// <summary>
        /// Check if a RMA was created correcty in our database (4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> GetAllRMATablesCreatedAsync(int refNo);

        /// <summary>
        /// Get the current area from the sql
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<AreaNameDto>> GetAreaByFunction(int repairNumber);


        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialnumber"></param>
        /// <returns></returns>
        Task<ApiResponse<List<RepairReadyToReceiveDto>>> GetRepairReadyToReceive(string serialnumber);

        /// <summary>
        /// get all date fields for a Repair Number
        /// </summary>
        /// <param name="refNumber"></param>
        /// <returns></returns>
        Task<ApiResponse<TorderRepairHdrDatesDto>> GetAllDateFieldsRepair(int refNumber);


    }
}

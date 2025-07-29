using APIZEBRA.DTOs.B2B;
using APIZEBRA.Models.B2b;
using APIZEBRA.Models.B2B;
using APIZEBRA.Utils.Responses;

namespace APIZEBRA.Repositories.B2B.Common.Queries
{
    /// <summary>
    /// Repository interface for querying repair-related data.
    /// </summary>
    public interface ICommonQueryRepository
    {
        /// <summary>
        /// get all information for a repair 
        /// </summary>
        /// <param name="RepairNumber"></param>
        /// <returns></returns>
        Task <CheckRepairInformationDto> GetRepairInfoAsync(int RepairNumber);


        /// <summary>
        /// Show RMA information for a serial number
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        Task<List<TzebInBoundRequestsFile>> GetRMAReceivingInfo(string serialNumber);


        /// <summary>
        /// Check if a RMA was created correcty in our database (records in 4 tables)
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        Task<bool> GetAllRMATablesCreatedAsync(int refNo);

        /// <summary>
        /// Get the current area from the sql
        /// </summary>
        /// <param name="repairNumber"></param>
        /// <returns></returns>
        Task<AreaNameDto> GetAreaByFunction(int repairNumber);

        /// <summary>
        /// Get info about one repair when it's ready for receive
        /// </summary>
        /// <param name="serialnumber"></param>
        /// <returns></returns>
        Task<List<RepairReadyToReceiveDto>> GetRepairReadyToReceive(string serialnumber);

        /// <summary>
        /// Gell all date fields for a Reference Number
        /// </summary>
        /// <param name="refNumber"></param>
        /// <returns></returns>
        Task<TorderRepairHdrDatesDto> GetAllDateFieldsRepair(int refNumber);

    }
}

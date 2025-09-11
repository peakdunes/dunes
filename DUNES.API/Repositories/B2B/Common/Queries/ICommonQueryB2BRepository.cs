using DUNES.API.DTOs.B2B;
using DUNES.API.Models.B2b;
using DUNES.API.Models.B2B;
using DUNES.API.ReadModels.B2B;
using DUNES.API.Utils.Responses;

namespace DUNES.API.Repositories.B2B.Common.Queries
{
    /// <summary>
    /// Repository interface for querying repair-related data.
    /// </summary>
    public interface ICommonQueryB2BRepository
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
        /// Check if a RMA was created correcty in our database 
        /// records in 4 tables : 
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        Task<bool> GetAllRMATablesCreatedAsync(int refNo);



        /// <summary>
        /// Displays the 4 tables associated with an order in Servtrack.
        /// _TOrderRepair_Hdr
        /// _TorderRepair_ItemsSerials_Receiving
        /// _TorderRepair_ItemsSerials_Shipping 
        /// _TOrderRepair_Items
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        Task<OrderRepairFourTablesRead> GetAllTablesOrderRepairCreatedAsync(int refNo);


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

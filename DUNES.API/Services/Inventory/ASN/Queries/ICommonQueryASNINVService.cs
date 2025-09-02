using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;

namespace DUNES.API.Services.Inventory.ASN.Queries
{

    /// <summary>
    /// All common Inventory queries (ASN - PickProcess
    /// </summary>
    public interface ICommonQueryASNINVService
    {
        /// <summary>
        /// Get all information about a ASN
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        Task <ApiResponse<ASNWm>> GetASNAllInfo(string ShipmentNum);




     
    }

}

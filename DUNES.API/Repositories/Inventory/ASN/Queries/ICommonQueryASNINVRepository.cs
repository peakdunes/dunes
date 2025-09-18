using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;

namespace DUNES.API.Repositories.Inventory.ASN.Queries
{


    /// <summary>
    /// Get all information about inventory transaction (PICK-PROCESS and ASN)
    /// </summary>
    public interface ICommonQueryASNINVRepository
    {
       

        /// <summary>
        /// Get ALl information about ASN
        /// </summary>
        /// <param name="ShipmentNum"></param>
        /// <returns></returns>
        Task<ASNRead?> GetASNAllInfo(string ShipmentNum);
    }
}

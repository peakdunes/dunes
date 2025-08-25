using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Inventory.Common.Queries
{

    /// <summary>
    /// ALL common WMS queries
    /// </summary>
    public interface ICommonQueryWMSINVService
    {
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient);

        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient);
    }
}

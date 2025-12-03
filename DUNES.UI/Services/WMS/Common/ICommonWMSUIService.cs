using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Common
{
    public interface ICommonWMSUIService
    {


        /// <summary>
        /// Get all active location for a company
        /// </summary>
        /// <param name="asnNumber"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesDTO>>> GetAllCompaniesInformation(string token, CancellationToken ct);


    }
}

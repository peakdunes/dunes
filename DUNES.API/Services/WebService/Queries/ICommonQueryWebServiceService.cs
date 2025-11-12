using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;

namespace DUNES.API.Services.WebService.Queries
{
    /// <summary>
    /// web service Inteface
    /// </summary>
    public interface ICommonQueryWebServiceService
    {

       /// <summary>
       /// Get all record per day - hour
       /// </summary>
       /// <param name="DateRequest"></param>
       /// <param name="ct"></param>
       /// <returns></returns>
        Task<ApiResponse<List<MvcWebServiceHourlySummaryDto>>> GetHourlyTransactions(DateTime DateRequest, CancellationToken ct);

       



    }
}

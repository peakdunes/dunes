using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;

namespace DUNES.API.Repositories.WebService.Queries
{

    /// <summary>
    /// web service Inteface
    /// </summary>
    public interface ICommonQueryWebServiceRepository
    {
        /// <summary>
        ///  Get all calls log per day - hour
        /// </summary>
        /// <param name="DateRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MvcWebServiceHourlySummaryDto>> GetHourlyTransactions(DateTime DateRequest, CancellationToken ct);
              
    
        /// <summary>
        /// Get daily summary by range
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<MvcWebServiceDailySummaryDto>> GetDailyRangeAsync(DateTime fromDate, DateTime toDate, CancellationToken ct);

        /// <summary>
        /// get daily summmary
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<MvcWebServiceDailySummaryDto?> GetCurrentDailyAsync(CancellationToken ct);
    }
}

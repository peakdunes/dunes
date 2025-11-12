using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;


namespace DUNES.API.Services.WebService.Transactions
{

    /// <summary>
    /// web service service
    /// </summary>
    public interface ITransactionsWebServiceService
    {


        /// <summary>
        /// insert or update summary record
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpsertHourlyAsync(MvcWebServiceHourlySummaryDto dto,CancellationToken ct);


    }
}

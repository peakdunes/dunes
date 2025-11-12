using DUNES.Shared.DTOs.WebService;


namespace DUNES.API.Repositories.WebService.Transactions
{

    /// <summary>
    /// web service log transactions (insert summary in web service  log tables
    /// for all RMA web service calls
    /// </summary>
    public interface ITransactionsWebServiceRepository
    {
        /// <summary>
        /// update summary log records
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task <bool>UpsertHourlyAsync(MvcWebServiceHourlySummaryDto dto,  CancellationToken ct);


        /// <summary>
        /// update daily record
        /// </summary>
        /// <param name="dateRequest"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
       
        Task UpsertDailyFromHourlyAsync(DateTime dateRequest, CancellationToken ct);

    }
}

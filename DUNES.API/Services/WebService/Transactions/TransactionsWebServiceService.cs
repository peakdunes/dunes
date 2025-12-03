using DUNES.API.Repositories.WebService.Transactions;

using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;


namespace DUNES.API.Services.WebService.Transactions
{
    /// <summary>
    /// web service service
    /// </summary>
    public class TransactionsWebServiceService : ITransactionsWebServiceService
    {

        private readonly ITransactionsWebServiceRepository _repository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public TransactionsWebServiceService(ITransactionsWebServiceRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// add or update a record 
        /// </summary>
        /// <param name="dto"></param>
      
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> UpsertHourlyAsync(MvcWebServiceHourlySummaryDto dto, CancellationToken ct)
        {
            if (!IsValidYMD(dto.Year, dto.Month,dto.Day))
            {
               return ApiResponseFactory.Unauthorized<bool>("Invalid date");
            }

            if (dto.Hour < 0 || dto.Hour > 23)
            {
                return ApiResponseFactory.Unauthorized<bool>("Invalid hour");
            }

             await _repository.UpsertHourlyAsync(dto, ct);

            return ApiResponseFactory.Ok(true, "OK");
        }






        bool IsValidYMD(int year, int month, int day)
        {
            if (month < 1 || month > 12) return false;
            if (day < 1) return false;

            int maxDay = DateTime.DaysInMonth(year, month); // maneja bisiestos automáticamente
            return day <= maxDay;
        }

    }
}

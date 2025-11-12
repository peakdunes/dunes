using DUNES.API.Repositories.WebService.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;

namespace DUNES.API.Services.WebService.Queries
{
    /// <summary>
    /// web service Inteface implementation
    /// </summary>
    public class CommonQueryWebServiceService : ICommonQueryWebServiceService
    {

        private readonly ICommonQueryWebServiceRepository _repository;

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="repository"></param>
        public CommonQueryWebServiceService(ICommonQueryWebServiceRepository repository)
        {
            _repository = repository;   
        }

        /// <summary>
        /// web service Inteface
        /// </summary>
        public async Task<ApiResponse<List<MvcWebServiceHourlySummaryDto>>> GetHourlyTransactions(DateTime DateRequest, CancellationToken ct)
        {

             var infolist = await _repository.GetHourlyTransactions(DateRequest, ct);
           

            return ApiResponseFactory.Ok(infolist, "OK");
        }
    }
}

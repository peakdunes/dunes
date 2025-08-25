using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.Utils.Responses;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Inventory.Common.Queries
{
    /// <summary>
    /// Get all information used for WMS inventory transactions (general queries)
    /// </summary>
    public class CommonQueryWMSINVService : ICommonQueryWMSINVService
    {




        private readonly ICommonQueryWMSINVRepository _repository;

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>

        public CommonQueryWMSINVService(ICommonQueryWMSINVRepository repository)
        {
            
            _repository = repository;
        }


        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            var info = await _repository.GetAllActiveBinsByCompanyClient(companyid, companyClient);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Bines>>($"Active bins not found for this company client {companyClient} ");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<Bines>>> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {
            var info = await _repository.GetAllBinsByCompanyClient(companyid, companyClient);

            if (info == null)
            {
                return ApiResponseFactory.NotFound<List<Bines>>($"there is not bins for this company client {companyClient}");
            }

            return ApiResponseFactory.Ok(info, "OK");
        }
    }
}

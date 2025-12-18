using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.CompaniesClientDivision
{
    /// <summary>
    /// Client Company services
    /// </summary>

    public class CompaniesClientDivisionWMSAPIService : ICompaniesClientDivisionWMSAPIService
    {



        private readonly ICommandCompaniesClientDivisionWMSAPIRepository _commandrepository;

        private readonly IQueryCompaniesClientDivisionWMSAPIRepository _queryrepository;

        /// <summary>
        /// constructor and dependency injection
        /// </summary>
        /// <param name="commandrepository"></param>
        /// <param name="queryrepository"></param>
        public CompaniesClientDivisionWMSAPIService(ICommandCompaniesClientDivisionWMSAPIRepository commandrepository,
            IQueryCompaniesClientDivisionWMSAPIRepository queryrepository )
        {
            _commandrepository = commandrepository;
            _queryrepository = queryrepository;
            
        }

        /// <summary>
        /// add mew company client division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<WMSCompanyClientDivisionDTO>> AddClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, CancellationToken ct)
        {


            throw new NotImplementedException();
        }
        /// <summary>
        /// delete company client division
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> DeleteClientCompanyDivisionAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get all company client division
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformation(CancellationToken ct)
        {
            var infodivisionlist = await _queryrepository.GetAllCompaniesClientDivisionInformation(ct);
                     

            return ApiResponseFactory.Ok(infodivisionlist, "");
        }
        /// <summary>
        /// get a division by id
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionById(int divisionId, CancellationToken ct)
        {
            if (divisionId <= 0)
            {
                return ApiResponseFactory.Error<WMSCompanyClientDivisionReadDTO?>("Division id must be bigger than Zero.");
            }

            var infodivision = await _queryrepository.GetCompanyClientDivisionById(divisionId, ct);


            return ApiResponseFactory.Ok(infodivision, "");
        }

        /// <summary>
        /// get a client division by name
        /// </summary>
        /// <param name="divisionname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionByNameAsync(string divisionname, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(divisionname))
            {
                return ApiResponseFactory.Error<WMSCompanyClientDivisionReadDTO?>("Division name can not null or blank");
            }

            var infodivision = await _queryrepository.GetCompanyClientDivisionByNameAsync(divisionname, ct);


            return ApiResponseFactory.Ok(infodivision, "");
        }

        /// <summary>
        /// update client division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> UpdateClientCompanyDivisionAsync(CompanyClientDivision entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

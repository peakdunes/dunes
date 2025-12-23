using AutoMapper;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision;
using DUNES.API.ServicesWMS.Masters.Cities;
using DUNES.API.ServicesWMS.Masters.ClientCompanies;
using DUNES.API.ServicesWMS.Masters.Countries;
using DUNES.API.ServicesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using FluentValidation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.ServicesWMS.Masters.CompaniesClientDivision
{
    /// <summary>
    /// Client Company services
    /// </summary>

    public class CompaniesClientDivisionWMSAPIService : ICompaniesClientDivisionWMSAPIService
    {
        private readonly IValidator<WMSCompanyClientDivisionDTO> _validator;
        private readonly ICommandCompaniesClientDivisionWMSAPIRepository _commandrepository;
        private readonly IQueryCompaniesClientDivisionWMSAPIRepository _queryrepository;
        private readonly IClientCompaniesWMSAPIService _companyService;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor and dependency injection
        /// </summary>
        /// <param name="commandrepository"></param>
        /// <param name="queryrepository"></param>
        /// <param name="validator"></param>
        /// <param name="companyService"></param>
        /// <param name="mapper"></param>

        public CompaniesClientDivisionWMSAPIService(ICommandCompaniesClientDivisionWMSAPIRepository commandrepository,
            IQueryCompaniesClientDivisionWMSAPIRepository queryrepository,
            IValidator<WMSCompanyClientDivisionDTO> validator, IClientCompaniesWMSAPIService companyService,
            IMapper mapper)
        {
            _commandrepository = commandrepository;
            _queryrepository = queryrepository;
            _validator = validator;
            _companyService = companyService;
            _mapper = mapper;
            
        }

        /// <summary>
        /// add mew company client division
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> AddClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO dto, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(dto, o => o.IncludeRuleSets("Create"));

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));

                return ApiResponseFactory.BadRequest<bool>(errors);

            }

            var existCompany = await _companyService.GetClientCompanyInformationByIdAsync(dto.Idcompanyclient, ct);

            if (existCompany == null || existCompany.Data == null)
            {
                return ApiResponseFactory.BadRequest<bool>("Company Client do not exist");
            }

            var existDivision = await _queryrepository.GetCompanyClientDivisionByNameAsync(dto.Idcompanyclient, dto.DivisionName!, ct);

            if(existDivision != null)
            {
                return ApiResponseFactory.BadRequest<bool>("Division already exist");
            }

           var comCliDiv =  _mapper.Map<CompanyClientDivision>(dto);

            var infoinsert = _commandrepository.AddClientCompanyDivisionAsync(comCliDiv, ct);

            return ApiResponseFactory.Ok(true, "Client company created successfully.");
            //throw new NotImplementedException();
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
        /// get all company client division
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformationByCompanyClient(int companyclientid,CancellationToken ct)
        {
            var infodivisionlist = await _queryrepository.GetAllCompaniesClientDivisionInformationByCompanyClient(companyclientid,ct);

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
        /// <param name="companyClientId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionByNameAsync(int companyClientId, string divisionname, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(divisionname))
            {
                return ApiResponseFactory.Error<WMSCompanyClientDivisionReadDTO?>("Division name can not null or blank");
            }

            var infodivision = await _queryrepository.GetCompanyClientDivisionByNameAsync(companyClientId, divisionname, ct);


            return ApiResponseFactory.Ok(infodivision, "");
        }

        /// <summary>
        /// update client division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> UpdateClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

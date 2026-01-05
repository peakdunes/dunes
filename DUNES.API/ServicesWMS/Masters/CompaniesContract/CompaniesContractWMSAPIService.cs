using AutoMapper;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.CompaniesContract;
using DUNES.API.ServicesWMS.Masters.ClientCompanies;
using DUNES.API.ServicesWMS.Masters.Companies;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using FluentValidation;
using Humanizer;
using System.ComponentModel.Design;

namespace DUNES.API.ServicesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// companies client contracts
    /// </summary>
    public class CompaniesContractWMSAPIService : ICompaniesContractWMSAPIService
    {
       

        private readonly IValidator<WMSCompaniesContractDTO> _validator;
        private readonly ICommandCompaniesContractWMSAPIRepository _commandrepository;
        private readonly IQueryCompaniesContractWMSAPIRepository _queryrepository;
        private readonly IClientCompaniesWMSAPIService _clientcompanyService;
        private readonly ICompaniesWMSAPIService _companyService;
        
        private readonly IMapper _mapper;


        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="validator"></param>
        /// <param name="commandrepository"></param>
        /// <param name="queryrepository"></param>
        /// <param name="mapper"></param>
        /// <param name="clientcompanyService"></param>
        /// <param name="companyService"></param>
        public CompaniesContractWMSAPIService(IValidator<WMSCompaniesContractDTO> validator,
                    ICommandCompaniesContractWMSAPIRepository commandrepository,
                    IQueryCompaniesContractWMSAPIRepository queryrepository,
                    IClientCompaniesWMSAPIService clientcompanyService,
                    ICompaniesWMSAPIService companyService,
                    IMapper mapper)
        {
            _validator = validator;
            _commandrepository = commandrepository;
            _queryrepository = queryrepository;
            _clientcompanyService = clientcompanyService;
            _companyService = companyService;
            _mapper = mapper;
        }

        /// <summary>
        /// get all contract information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct)
        {
            var listcontract = await _queryrepository.GetAllClientCompaniesContractInformationAsync(ct);

            return ApiResponseFactory.Ok(listcontract, "Contract(s) registered in our system.");
        }

        /// <summary>
        /// Get all contract information for a company client id
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationByCompanyClientIdAsync(int companyclientid, CancellationToken ct)
        {
            var infocontract = await _queryrepository.GetAllClientCompaniesContractInformationByCompanyClientIdAsync(companyclientid, ct);

            if (infocontract == null)
                return ApiResponseFactory.NotFound<List<WMSCompaniesContractReadDTO>>("Contract for this company client not found.");

            return ApiResponseFactory.Ok(infocontract, "Contract(s) retrieved successfully.");
        }

        /// <summary>
        /// get all contract information for a company id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationByCompanyIdAsync(int companyid, CancellationToken ct)
        {
            var infocontract = await _queryrepository.GetAllClientCompaniesContractInformationByCompanyIdAsync(companyid, ct);

            if (infocontract == null)
                return ApiResponseFactory.NotFound<List<WMSCompaniesContractReadDTO>>("Contract for this company not found.");

            return ApiResponseFactory.Ok(infocontract, "Contract(s) retrieved successfully.");
        }
        /// <summary>
        /// get contract information by id (PK id)
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByContractIdAsync(int Id, CancellationToken ct)
        {
            var infocontract = await _queryrepository.GetClientCompanyContractInformationByContractIdAsync(Id, ct);

            if (infocontract == null)
                return ApiResponseFactory.NotFound<WMSCompaniesContractReadDTO>("Contract for this company not found.");

            return ApiResponseFactory.Ok(infocontract, "Contract retrieved successfully.");
        }

        /// <summary>
        /// get contract information by company client id and  contract code (contract number)
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByCompanyIdAndNumberAsync(int companyclientid, string contractcode, CancellationToken ct)
        {
            var infocontract = await _queryrepository.GetClientCompanyInformationContractByNumberAndCompanyIdAsync(companyclientid, contractcode, ct);

            if (infocontract == null)
                return ApiResponseFactory.NotFound<WMSCompaniesContractReadDTO>("Contract for this company not found.");

            return ApiResponseFactory.Ok(infocontract, "Contract retrieved successfully.");
        }
        /// <summary>
        /// get contract information by contract code (contract number)
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct)
        {
            var infocontract = await _queryrepository.GetClientCompanyInformationContractByNumberAsync(contractcode, ct);

            if (infocontract == null)
                return ApiResponseFactory.NotFound<WMSCompaniesContractReadDTO>("Contract not found.");

            return ApiResponseFactory.Ok(infocontract, "Contract retrieved successfully.");

        }
        /// <summary>
        /// update contract information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(WMSCompaniesContractDTO entity, CancellationToken ct)
        {
            var infocontract = await _queryrepository.GetClientCompanyContractInformationByContractIdAsync(entity.Id, ct);


            if (infocontract == null)
            {
                return ApiResponseFactory.BadRequest<bool>("Contract do not exist");
            }

            var validation = await _validator.ValidateAsync(entity, o =>
                    o.IncludeRuleSets("Update")
                     .IncludeRulesNotInRuleSet()
                );

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));

                return ApiResponseFactory.BadRequest<bool>(errors);

            }


            var info = _mapper.Map<DUNES.API.ModelsWMS.Masters.CompaniesContract>(infocontract);

            var result = await _commandrepository.UpdateClientCompanyContractAsync(info, ct);

            return ApiResponseFactory.Ok(true, "Contract updated successfully.");
        }
        /// <summary>
        /// add new contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> AddClientCompanyContractAsync(WMSCompaniesContractDTO entity, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(entity, o =>
                        o.IncludeRuleSets("Create")
                         .IncludeRulesNotInRuleSet()
                    );

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));

                return ApiResponseFactory.BadRequest<bool>(errors);

            }

            var existClientCompany = await _clientcompanyService.GetClientCompanyInformationByIdAsync(entity.CompanyClientId, ct);

            if (existClientCompany == null || existClientCompany.Data == null)
            {
                return ApiResponseFactory.BadRequest<bool>("Company Client do not exist");
            }

            var existCompany = await _companyService.GetByIdAsync(entity.CompanyId, ct);

            if (existCompany == null || existCompany.Data == null)
            {
                return ApiResponseFactory.BadRequest<bool>("Company do not exist");
            }
     
            var contractinfo = _mapper.Map<DUNES.API.ModelsWMS.Masters.CompaniesContract>(entity);
         
            var infoinsert = await _commandrepository.AddClientCompanyContractAsync(contractinfo, ct);

            return ApiResponseFactory.Ok(true, "Contract created successfully.");
        }

        /// <summary>
        /// delete contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, CancellationToken ct)
        {
           var infocontract = await _queryrepository.GetClientCompanyContractInformationByContractIdAsync(id, ct);


            if (infocontract == null )
            {
                return ApiResponseFactory.BadRequest<bool>("Contract do not exist");
            }

            var result = await _commandrepository.DeleteClientCompanyContractAsync(id, ct);

            return ApiResponseFactory.Ok(true, "Contract deleted successfully.");
        }

    }
}

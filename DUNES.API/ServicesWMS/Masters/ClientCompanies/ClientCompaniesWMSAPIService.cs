using AutoMapper;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.ClientCompanies;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using FluentValidation;

namespace DUNES.API.ServicesWMS.Masters.ClientCompanies
{  
    /// <summary>
    /// Companies Client Services
    /// </summary>
    public class ClientCompaniesWMSAPIService : IClientCompaniesWMSAPIService
    {

        private readonly IValidator<WMSClientCompaniesDTO> _validator;
        private readonly IClientCompaniesWMSAPIRepository _repository;
        private readonly IMapper _mapper;
       

        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="validator"></param>
        public ClientCompaniesWMSAPIService(IClientCompaniesWMSAPIRepository repository
            ,
            IValidator<WMSClientCompaniesDTO> validator
            , IMapper mapper
            )
        {
            _repository = repository;
            _validator = validator;
            _mapper = mapper;
        }

        /// <summary>
        /// Add new company client
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> AddClientCompanyAsync(WMSClientCompaniesDTO dto, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(dto, o => o.IncludeRuleSets("Create"));

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));

                return ApiResponseFactory.BadRequest<bool>(errors);

            }

            var exist = await _repository.GetClientCompanyInformationByIdentificationAsync(dto.CompanyId!, ct);
            
            if (exist != null)
            {
                return ApiResponseFactory.BadRequest<bool>($"A client with this CompanyId {dto.CompanyId} already exists.");
            }

            var entity = new CompanyClient
            {
                Id = 0,
                CompanyId = dto.CompanyId,
                Name = dto.Name,
                Idcountry = dto.Idcountry,
                Idstate = dto.Idstate,
                Idcity = dto.Idcity,
                Zipcode = dto.Zipcode,
                Address = dto.Address,
                Phone = dto.Phone,
                Active = dto.Active
            };

            await _repository.AddClientCompanyAsync(entity, ct);

            // 5) Respuesta
            return ApiResponseFactory.Ok(true, "Client company created successfully.");
        }
        /// <summary>
        /// Delete company client by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> DeleteClientCompanyAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get all client companies
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSClientCompaniesReadDTO>>> GetAllClientCompaniesInformation(CancellationToken ct)
        {
            var infocli = await _repository.GetAllClientCompaniesInformationAsync(ct);

            var infoclimap = _mapper.Map<List<WMSClientCompaniesReadDTO>>(infocli);

            return ApiResponseFactory.Ok(infoclimap, "");
        }
        /// <summary>
        /// get client company for id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<ApiResponse<WMSClientCompaniesReadDTO>> GetClientCompanyInformationByIdAsync(int Id, CancellationToken ct)
        {
            var infocli = await _repository.GetClientCompanyInformationByIdAsync(Id, ct);

            var infoclimap = _mapper.Map<WMSClientCompaniesReadDTO>(infocli);

            return ApiResponseFactory.Ok(infoclimap, "");
        }


        /// <summary>
        /// Get company information by name
        /// </summary>
        /// <param name="companyname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSClientCompaniesReadDTO>> GetClientCompanyInformationByNameAsync(string companyname, CancellationToken ct)
        {
            var infocli = await _repository.GetClientCompanyInformationByNameAsync(companyname, ct);

            var infoclimap = _mapper.Map<WMSClientCompaniesReadDTO>(infocli);

            return ApiResponseFactory.Ok(infoclimap, "");
        }
        /// <summary>
        /// get client company information by company identification
        /// /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<ApiResponse<WMSClientCompaniesReadDTO>> GetClientCompanyInformationByIdentificationAsync(string companyid, CancellationToken ct)
        {
            var infocli = await _repository.GetClientCompanyInformationByIdentificationAsync(companyid, ct);

            var infoclimap = _mapper.Map<WMSClientCompaniesReadDTO>(infocli);

            return ApiResponseFactory.Ok(infoclimap, "");
        }

        /// <summary>
        /// update client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> UpdateClientCompanyAsync(CompanyClient entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

       
    }
}

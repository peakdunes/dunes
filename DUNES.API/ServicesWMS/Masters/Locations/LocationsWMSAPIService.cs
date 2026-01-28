using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Locations;
using DUNES.API.ServicesWMS.Masters.Cities;
using DUNES.API.ServicesWMS.Masters.Companies;
using DUNES.API.ServicesWMS.Masters.Countries;
using DUNES.API.ServicesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using FluentValidation;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.Locations
{
    /// <summary>
    /// locations service implementation
    /// </summary>
    public class LocationsWMSAPIService : ILocationsWMSAPIService
    {
        private readonly IValidator<WMSLocationsDTO> _validator;
        private readonly ILocationsWMSAPIRepository _repository;
        private readonly ICompaniesWMSAPIService _companiesWMSAPIService;
        private readonly ICountriesWMSAPIService _countriesWMSAPIService;
        private readonly IStateCountriesWMSAPIService _statesService;
        private readonly ICitiesWMSAPIService _citiesWMSAPIService;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        public LocationsWMSAPIService(
            IValidator<WMSLocationsDTO> validator,
            ILocationsWMSAPIRepository repository,
            IMapper mapper,
            ICompaniesWMSAPIService companiesWMSAPIService,
            ICountriesWMSAPIService countriesWMSAPIService,
            IStateCountriesWMSAPIService statesService,
            ICitiesWMSAPIService citiesWMSAPIService)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
            _companiesWMSAPIService = companiesWMSAPIService;
            _countriesWMSAPIService = countriesWMSAPIService;
            _statesService = statesService;
            _citiesWMSAPIService = citiesWMSAPIService;
        }

        /// <summary>
        /// add new location
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSLocationsDTO entity,
            CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(entity, o =>
                o.IncludeRuleSets("Create")
                 .IncludeRulesNotInRuleSet());

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));
                return ApiResponseFactory.BadRequest<bool>(errors);
            }

            var infocompany = await _companiesWMSAPIService.GetByIdAsync(companyId, ct);

            if (infocompany.Data == null)
                return ApiResponseFactory.NotFound<bool>($"Company with Id {companyId} was not found.");

            if (!infocompany.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"Company with Id {companyId} is not active.");

            var infocountry = await _countriesWMSAPIService.GetByIdAsync(entity.Idcountry, ct);
            if (infocountry.Data == null || !infocountry.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Idcountry} was not found or is not active.");

            var infostate = await _statesService.GetByIdAsync(entity.Idstate, ct);
            if (infostate.Data == null || !infostate.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Idstate} was not found or is not active.");

            var infocity = await _citiesWMSAPIService.GetByIdAsync(entity.Idcity, ct);
            if (infocity.Data == null || !infocity.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Idcity} was not found or is not active.");

            var locExist = await _repository.ExistsByNameAsync(
                companyId,
                entity.Name!,
                null,
                ct);

            if (locExist)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_LOCATION_NAME",
                    message: $"There is already a location with the name '{entity.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var entityMap = _mapper.Map<DUNES.API.ModelsWMS.Masters.Locations>(entity);
            entityMap.Idcompany = companyId;

            await _repository.CreateAsync(entityMap, ct);

            return ApiResponseFactory.Ok(true, "Location created successfully.");
        }

        /// <summary>
        /// search location by name
        /// </summary>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "Name is required",
                    message: "Location name is required",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var locExist = await _repository.ExistsByNameAsync(
                companyId,
                name,
                excludeId,
                ct);

            return ApiResponseFactory.Ok(locExist, "");
        }

        /// <summary>
        /// get all active locations
        /// </summary>
        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            var locList = await _repository.GetActiveAsync(companyId, ct);
            return ApiResponseFactory.Ok(locList, "");
        }

        /// <summary>
        /// get all locations
        /// </summary>
        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            var locList = await _repository.GetAllAsync(companyId, ct);
            return ApiResponseFactory.Ok(locList, "");
        }

        /// <summary>
        /// get location by id
        /// </summary>
        public async Task<ApiResponse<WMSLocationsDTO?>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            if (id <= 0)
            {
                return ApiResponseFactory.Fail<WMSLocationsDTO?>(
                    error: "ID is required",
                    message: "Location ID is required",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var infoLoc = await _repository.GetByIdAsync(companyId, id, ct);
            return ApiResponseFactory.Ok(infoLoc, "");
        }

        /// <summary>
        /// active no active location
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(companyId, id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"Location with Id {id} was not found.");

            var msg = isActive
                ? "Location has been activated successfully."
                : "Location has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }

        /// <summary>
        /// update location
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            WMSLocationsDTO entity,
            CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(entity, o =>
                o.IncludeRuleSets("Update")
                 .IncludeRulesNotInRuleSet());

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));
                return ApiResponseFactory.BadRequest<bool>(errors);
            }

            var existEntity = await _repository.GetByIdAsync(companyId, entity.Id, ct);

            if (existEntity == null)
                return ApiResponseFactory.NotFound<bool>($"Location with id '{entity.Id}' was not found.");

            var infocompany = await _companiesWMSAPIService.GetByIdAsync(companyId, ct);
            if (infocompany.Data == null || !infocompany.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"Company with Id {companyId} was not found or is not active.");

            var infocountry = await _countriesWMSAPIService.GetByIdAsync(entity.Idcountry, ct);
            if (infocountry.Data == null || !infocountry.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Idcountry} was not found or is not active.");

            var infostate = await _statesService.GetByIdAsync(entity.Idstate, ct);
            if (infostate.Data == null || !infostate.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Idstate} was not found or is not active.");

            var infocity = await _citiesWMSAPIService.GetByIdAsync(entity.Idcity, ct);
            if (infocity.Data == null || !infocity.Data.Active)
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Idcity} was not found or is not active.");

            var locExist = await _repository.ExistsByNameAsync(
                companyId,
                entity.Name!,
                entity.Id,
                ct);

            if (locExist)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_LOCATION_NAME",
                    message: $"There is already a location with the name '{entity.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var location = _mapper.Map<DUNES.API.ModelsWMS.Masters.Locations>(entity);
                      

            await _repository.UpdateAsync(location, ct);

            return ApiResponseFactory.Ok(true, "Location updated successfully.");
        }
    }
}

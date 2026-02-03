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
        /// <summary>
        /// Locations service implementation.
        /// Handles business rules for WMS locations scoped by Company.
        /// </summary>

        private readonly IValidator<WMSLocationsUpdateDTO> _validator;
        private readonly ILocationsWMSAPIRepository _repository;
        private readonly ICountriesWMSAPIService _countriesService;
        private readonly IStateCountriesWMSAPIService _statesService;
        private readonly ICitiesWMSAPIService _citiesService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsWMSAPIService"/> class.
        /// </summary>
        /// <param name="validator">FluentValidation validator for location DTO.</param>
        /// <param name="repository">Locations repository.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        /// <param name="countriesService">Countries service.</param>
        /// <param name="statesService">States service.</param>
        /// <param name="citiesService">Cities service.</param>
        public LocationsWMSAPIService(
            IValidator<WMSLocationsUpdateDTO> validator,
            ILocationsWMSAPIRepository repository,
            IMapper mapper,
            ICountriesWMSAPIService countriesService,
            IStateCountriesWMSAPIService statesService,
            ICitiesWMSAPIService citiesService)
        {
            _validator = validator;
            _repository = repository;
            _mapper = mapper;
            _countriesService = countriesService;
            _statesService = statesService;
            _citiesService = citiesService;
        }

        /// <summary>
        /// Creates a new location for the specified company.
        /// </summary>
        /// <param name="companyId">Company identifier (from token).</param>
        /// <param name="dto">Location data transfer object.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSLocationsUpdateDTO dto,
            CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(dto, o =>
                o.IncludeRuleSets("Create")
                 .IncludeRulesNotInRuleSet());

            if (!validation.IsValid)
            {
                var errors = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
                return ApiResponseFactory.BadRequest<bool>(errors);
            }

            // Validate hierarchy (Country → State → City)
            var country = await _countriesService.GetByIdAsync(dto.Idcountry, ct);
            if (country.Data == null || !country.Data.Active)
                return ApiResponseFactory.NotFound<bool>("Invalid or inactive country.");

            var state = await _statesService.GetByIdAsync(dto.Idstate, ct);
            if (state.Data == null || !state.Data.Active)
                return ApiResponseFactory.NotFound<bool>("Invalid or inactive state.");

            var city = await _citiesService.GetByIdAsync(dto.Idcity, ct);
            if (city.Data == null || !city.Data.Active)
                return ApiResponseFactory.NotFound<bool>("Invalid or inactive city.");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                dto.Name!,
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_LOCATION_NAME",
                    message: $"There is already a location with the name '{dto.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var entity = _mapper.Map<ModelsWMS.Masters.Locations>(dto);
            entity.Idcompany = companyId;

            await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Ok(true, "Location created successfully.");
        }

        /// <summary>
        /// Gets all locations for the specified company.
        /// </summary>
        /// <param name="companyId">Company identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of locations.</returns>
        public async Task<ApiResponse<List<WMSLocationsReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            var list = await _repository.GetAllAsync(companyId, ct);
            return ApiResponseFactory.Ok(list);
        }

        /// <summary>
        /// Gets all active locations for the specified company.
        /// </summary>
        /// <param name="companyId">Company identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of active locations.</returns>
        public async Task<ApiResponse<List<WMSLocationsReadDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            var list = await _repository.GetActiveAsync(companyId, ct);
            return ApiResponseFactory.Ok(list);
        }

        /// <summary>
        /// Gets a location by its identifier.
        /// </summary>
        /// <param name="companyId">Company identifier.</param>
        /// <param name="id">Location identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Location information.</returns>
        public async Task<ApiResponse<WMSLocationsReadDTO?>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSLocationsReadDTO?>("Location id is required.");

            var data = await _repository.GetByIdAsync(companyId, id, ct);
            return ApiResponseFactory.Ok(data);
        }

        /// <summary>
        /// Updates an existing location.
        /// </summary>
        /// <param name="companyId">Company identifier.</param>
        /// <param name="id">Location identifier.</param>
        /// <param name="dto">Updated location data.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSLocationsUpdateDTO dto,
            CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(dto, o =>
                o.IncludeRuleSets("Update")
                 .IncludeRulesNotInRuleSet());

            if (!validation.IsValid)
            {
                var errors = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
                return ApiResponseFactory.BadRequest<bool>(errors);
            }

            var existing = await _repository.GetByIdAsync(companyId, id, ct);
            if (existing == null)
                return ApiResponseFactory.NotFound<bool>("Location not found.");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                dto.Name!,
                id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_LOCATION_NAME",
                    message: $"There is already a location with the name '{dto.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            // Map only allowed fields (ownership is preserved)
          //  _mapper.Map(dto, existing);
          
            var entity = _mapper.Map<ModelsWMS.Masters.Locations>(dto);

            entity.Idcompany = companyId;


            await _repository.UpdateAsync(entity, ct);

            return ApiResponseFactory.Ok(true, "Location updated successfully.");
        }

        /// <summary>
        /// Activates or deactivates a location.
        /// </summary>
        /// <param name="companyId">Company identifier.</param>
        /// <param name="id">Location identifier.</param>
        /// <param name="isActive">Activation state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Operation result.</returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(companyId, id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>("Location not found.");

            var message = isActive
                ? "Location activated successfully."
                : "Location deactivated successfully.";

            return ApiResponseFactory.Ok(true, message);
        }

        /// <summary>
        /// Checks if a location with the same name already exists.
        /// </summary>
        /// <param name="companyId">Company identifier.</param>
        /// <param name="name">Location name.</param>
        /// <param name="excludeId">Optional location id to exclude.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if exists; otherwise false.</returns>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ApiResponseFactory.BadRequest<bool>("Location name is required.");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                name,
                excludeId,
                ct);

            return ApiResponseFactory.Ok(exists);
        }
    }
}


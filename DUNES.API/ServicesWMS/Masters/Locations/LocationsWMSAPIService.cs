using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.API.RepositoriesWMS.Masters.Locations;
using DUNES.API.ServicesWMS.Masters.Cities;
using DUNES.API.ServicesWMS.Masters.Companies;
using DUNES.API.ServicesWMS.Masters.Countries;
using DUNES.API.ServicesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

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
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="validator"></param>
        /// <param name="companiesWMSAPIService"></param>
        /// <param name="countriesWMSAPIService"></param>
        /// <param name="statesService"></param>
        /// <param name="citiesWMSAPIService"></param>


        public LocationsWMSAPIService(IValidator<WMSLocationsDTO> validator, ILocationsWMSAPIRepository repository,
            IMapper mapper, ICompaniesWMSAPIService companiesWMSAPIService,
            ICountriesWMSAPIService countriesWMSAPIService, IStateCountriesWMSAPIService statesService,
            ICitiesWMSAPIService citiesWMSAPIService
            )
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
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> CreateAsync(WMSLocationsDTO entity, CancellationToken ct)
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

            var infocompany = await _companiesWMSAPIService.GetByIdAsync(entity.Idcompany, ct);

            if (infocompany.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"Company with Id {entity.Idcompany} was not found.");
            }

            if (infocompany.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"Company with Id {entity.Idcompany} is not active.");
            }


            var infocountry = await _countriesWMSAPIService.GetByIdAsync(entity.Idcountry, ct);

            if (infocountry.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Idcountry} was not found.");
            }

            if (infocountry.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Idcountry} is not active.");
            }

            var infostate = await _statesService.GetByIdAsync(entity.Idstate, ct);

            if (infostate.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Idstate} was not found.");
            }

            if (infostate.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Idstate} is not active.");
            }


            var infocity = await _citiesWMSAPIService.GetByIdAsync(entity.Idcity, ct);

            if (infocity.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Idstate} was not found.");
            }

            if (infocity.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Idstate} is not active.");
            }



            var locExist = await _repository.ExistsByNameAsync(entity.Name!, entity.Id, ct);

            if (locExist)
            {
                return ApiResponseFactory.Fail<bool>(
                        error: "DUPLICATE_LOCATION_NAME",
                        message: $"There is already a location with the name '{entity.Name}'.",
                        statusCode: (int)HttpStatusCode.Conflict);
            }

            var entityMap = _mapper.Map<DUNES.API.ModelsWMS.Masters.Locations>(entity);

            var locInsert = _repository.CreateAsync(entityMap, ct);

            return ApiResponseFactory.Ok(true, "Location created successfully.");
        }
        /// <summary>
        /// search location by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {

            if (string.IsNullOrEmpty(name))
            {
                return ApiResponseFactory.Fail<bool>(
                        error: "Name is required",
                        message: "Location name is required",
                        statusCode: (int)HttpStatusCode.Conflict);
            }

            var locExist = await _repository.ExistsByNameAsync(name, null, ct);

            return ApiResponseFactory.Ok(locExist, "");
        }

        /// <summary>
        /// get all active locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetActiveAsync(CancellationToken ct)
        {
            var locList = await _repository.GetActiveAsync(ct);

            return ApiResponseFactory.Ok(locList, "");
        }
        /// <summary>
        /// get all locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetAllAsync(CancellationToken ct)
        {
            var locList = await _repository.GetAllAsync(ct);

            return ApiResponseFactory.Ok(locList, "");
        }

        /// <summary>
        /// get location by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSLocationsDTO?>> GetByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return ApiResponseFactory.Fail<WMSLocationsDTO?>(
                        error: "ID is required",
                        message: "Location ID is required",
                        statusCode: (int)HttpStatusCode.Conflict);
            }
            var infoLoc = await _repository.GetByIdAsync(id, ct);

            return ApiResponseFactory.Ok(infoLoc, "");
        }
        /// <summary>
        /// active no active location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(id, isActive, ct);

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
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSLocationsDTO entity, CancellationToken ct)
        {
            var validation = await _validator.ValidateAsync(entity, o =>
                     o.IncludeRuleSets("Update")
                      .IncludeRulesNotInRuleSet()
                 );

            if (!validation.IsValid)
            {
                var errors = string.Join(" |", validation.Errors.Select(e => e.ErrorMessage));

                return ApiResponseFactory.BadRequest<bool>(errors);

            }

            var existId = _repository.GetByIdAsync(entity.Id, ct);

            if (existId == null)
            {
                return ApiResponseFactory.NotFound<bool>($"Location with id '{entity.Id}' was not found.");


            }


            var infocompany = await _companiesWMSAPIService.GetByIdAsync(entity.Idcompany, ct);

            if (infocompany.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"Company with Id {entity.Idcompany} was not found.");
            }

            if (infocompany.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"Company with Id {entity.Idcompany} is not active.");
            }


            var infocountry = await _countriesWMSAPIService.GetByIdAsync(entity.Idcountry, ct);

            if (infocountry.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Idcountry} was not found.");
            }

            if (infocountry.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Idcountry} is not active.");
            }

            var infostate = await _statesService.GetByIdAsync(entity.Idstate, ct);

            if (infostate.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Idstate} was not found.");
            }

            if (infostate.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Idstate} is not active.");
            }


            var infocity = await _citiesWMSAPIService.GetByIdAsync(entity.Idcity, ct);

            if (infocity.Data == null)
            {
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Idstate} was not found.");
            }

            if (infocity.Data.Active == false)
            {
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Idstate} is not active.");
            }



            var locExist = await _repository.ExistsByNameAsync(entity.Name!, entity.Id, ct);

            if (locExist)
            {
                return ApiResponseFactory.Fail<bool>(
                        error: "DUPLICATE_LOCATION_NAME",
                        message: $"There is already a location with the name '{entity.Name}'.",
                        statusCode: (int)HttpStatusCode.Conflict);
            }

            var entityMap = _mapper.Map<DUNES.API.ModelsWMS.Masters.Locations>(entity);

            var locUpdate = await _repository.UpdateAsync(entityMap, ct);

            return ApiResponseFactory.Ok(true, "Location created successfully.");
        }

        
    }
}

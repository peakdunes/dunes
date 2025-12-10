using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.API.RepositoriesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Net;
using System.Reflection.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.ServicesWMS.Masters.StateCountries
{

    /// <summary>
    /// states servoce
    /// </summary>
    public class StateCountriesWMSAPIService : IStateCountriesWMSAPIService
    {
        private readonly IStateCountriesWMSAPIRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public StateCountriesWMSAPIService(IStateCountriesWMSAPIRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// get all states
        /// </summary>
        public async Task<ApiResponse<List<WMSStatesCountriesReadDTO>>> GetAllAsync(int countryId, CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(countryId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSStatesCountriesReadDTO>>("No states found.");

            var objlist = _mapper.Map<List<WMSStatesCountriesReadDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get all active states
        /// </summary>
        public async Task<ApiResponse<List<WMSStatesCountriesReadDTO>>> GetActiveAsync(int countryId, CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(countryId, ct);

            var objlist = _mapper.Map<List<WMSStatesCountriesReadDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get state by id
        /// </summary>
        public async Task<ApiResponse<WMSStatesCountriesReadDTO?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSStatesCountriesReadDTO?>($"state with Id {id} was not found.");

            var objmap = _mapper.Map<WMSStatesCountriesReadDTO>(entity);

            return ApiResponseFactory.Ok(objmap);
        }

        /// <summary>
        /// create new state
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(WMSStatesCountriesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado
            var infocountry = await _repository.ExistsByNameAsync(entity.Idcountry, entity.Name!, null, ct);
            if (infocountry != null)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_STATE_NAME",
                         message: $"There is already a state with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            //var objmap = _mapper.Map<DUNES.API.ModelsWMS.Masters.StatesCountries>(entity);

            await _repository.CreateAsync(entity, ct);
            return ApiResponseFactory.Ok(true, "State created successfully.");
        }

        /// <summary>
        /// update state information
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSStatesCountriesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado excluyendo el propio Id
            var infostate = await _repository.ExistsByNameAsync(entity.Idcountry,entity.Name!, entity.Id, ct);
            if (infostate != null)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_STATE_NAME",
                         message: $"There is already a state with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var current = await _repository.GetByIdAsync(entity.Id, ct);
            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>($"State with Id {entity.Id} was not found.");
            }

            // si quieres, puedes copiar solo campos editables en vez de reemplazar la entidad
            current.Name = entity.Name;
            current.Active = entity.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(true, "State updated successfully.");
        }

        /// <summary>
        /// activate / deactivate country
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"State with Id {id} was not found.");

            var msg = isActive
                ? "State has been activated successfully."
                : "State has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }

        /// <summary>
        /// exist state name
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<WMSStatesCountriesReadDTO?>> ExistsByNameAsync(int countryid, string name, int? excludeId, CancellationToken ct)
        {
            var infostate = await _repository.ExistsByNameAsync(countryid, name, excludeId, ct);

            if (infostate != null)
            {
                var dto = _mapper.Map<WMSStatesCountriesReadDTO>(infostate);

                var response = ApiResponseFactory.Fail<WMSStatesCountriesReadDTO?>(
                        error: "DUPLICATE_ISO_CODE",
                        message: $"There is already a state with ISO Code '{name}'.",
                        statusCode: (int)HttpStatusCode.Conflict
                    );

                response.Data = dto;

                return response;
            }

            return ApiResponseFactory.Ok<WMSStatesCountriesReadDTO?>(
                  data: null,
                  message: "ISO Code not found"
              );
        }

        /// <summary>
        /// check if a ISO Code exist in our system
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="isocode"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        ///       
     
        public async Task<ApiResponse<WMSStatesCountriesReadDTO?>> GetByISOCodeAsync(int countryid, string isocode, int? excludeId, CancellationToken ct)
        {
            // validar nombre duplicado
            var infocountry = await _repository.ExistsByISOCodeAsync(countryid, isocode!, null, ct);
            if (infocountry != null)
            {
                var dto = _mapper.Map<WMSStatesCountriesReadDTO>(infocountry);

                var response = ApiResponseFactory.Fail<WMSStatesCountriesReadDTO?>(
                        error: "DUPLICATE_ISO_CODE",
                        message: $"There is already a state with ISO Code '{isocode}'.",
                        statusCode: (int)HttpStatusCode.Conflict
                    );

                response.Data = dto;

                return response;
            }

            return ApiResponseFactory.Ok<WMSStatesCountriesReadDTO?>(
                  data: null,
                  message: "ISO Code not found"
              );
        }



        /// <summary>
        /// get state info by name
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="name"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ApiResponse<WMSStatesCountriesReadDTO?>> GetByNameAsync(int countryid, string name, CancellationToken ct)
        {
            // validar nombre duplicado
            var infocountry = await _repository.ExistsByNameAsync(countryid, name!, null, ct);
            if (infocountry != null)
            {
                var dto = _mapper.Map<WMSStatesCountriesReadDTO>(infocountry);

                var response = ApiResponseFactory.Fail<WMSStatesCountriesReadDTO?>(
                        error: "DUPLICATE_ISO_CODE",
                        message: $"There is already a state with Name '{name}'.",
                        statusCode: (int)HttpStatusCode.Conflict
                    );

                response.Data = dto;

                return response;
            }

            return ApiResponseFactory.Ok<WMSStatesCountriesReadDTO?>(
                  data: null,
                  message: "ISO Code not found"
              );


        }

      
    }
}

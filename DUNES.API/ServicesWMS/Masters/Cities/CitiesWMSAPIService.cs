using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Cities;
using DUNES.API.RepositoriesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.ServicesWMS.Masters.Cities
{

    /// <summary>
    /// cities service
    /// </summary>
    public class CitiesWMSAPIService : ICitiesWMSAPIService
    {

        private readonly ICitiesWMSAPIRepository _repository;
        private readonly IMapper _mapper;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public CitiesWMSAPIService(ICitiesWMSAPIRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// get all states
        /// </summary>
        public async Task<ApiResponse<List<WMSCitiesReadDTO>>> GetAllAsync(int countryid, CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(countryid,ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCitiesReadDTO>>("No cities found.");

            var objlist = _mapper.Map<List<WMSCitiesReadDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get all active states
        /// </summary>
        public async Task<ApiResponse<List<WMSCitiesReadDTO>>> GetActiveAsync(int countryid, CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(countryid,ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCitiesReadDTO>>("No active cities found.");

            var objlist = _mapper.Map<List<WMSCitiesReadDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get state by id
        /// </summary>
        public async Task<ApiResponse<WMSCitiesReadDTO?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSCitiesReadDTO?>($"city with Id {id} was not found.");

            var objmap = _mapper.Map<WMSCitiesReadDTO>(entity);

            return ApiResponseFactory.Ok(objmap);
        }

        /// <summary>
        /// create new state
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(WMSCitiesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado
            var exists = await _repository.ExistsByNameAsync(entity.Idcountry, entity.Name!, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_CITY_NAME",
                         message: $"There is already a city with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var objmap = _mapper.Map<ModelsWMS.Masters.Cities>(entity);

            await _repository.CreateAsync(objmap, ct);
            return ApiResponseFactory.Ok(true, "City created successfully.");
        }

        /// <summary>
        /// update state information
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSCitiesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado excluyendo el propio Id
            var exists = await _repository.ExistsByNameAsync(entity.Idcountry,entity.Name!, entity.Id, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_CITY_NAME",
                         message: $"There is already a city with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var current = await _repository.GetByIdAsync(entity.Id, ct);
            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>($"City with Id {entity.Id} was not found.");
            }

            // si quieres, puedes copiar solo campos editables en vez de reemplazar la entidad
            current.Name = entity.Name;
            current.Active = entity.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(true, "City updated successfully.");
        }

        /// <summary>
        /// activate / deactivate country
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"City with Id {id} was not found.");

            var msg = isActive
                ? "City has been activated successfully."
                : "City has been deactivated successfully.";

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
        public async Task<ApiResponse<bool>> ExistsByNameAsync(int countryid,string name, int? excludeId, CancellationToken ct)
        {
            var exists = await _repository.ExistsByNameAsync(countryid, name, excludeId, ct);

            // aquí tienes dos enfoques posibles:

            // 1) Regresar "exists" tal cual (true = ya existe, false = no existe):
            return ApiResponseFactory.Ok(exists);

           
        }
    }
}

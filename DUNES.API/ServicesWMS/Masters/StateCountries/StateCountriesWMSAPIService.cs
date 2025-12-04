using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.API.RepositoriesWMS.Masters.StateCountries;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.ServicesWMS.Masters.StateCountries
{

    /// <summary>
    /// states servoce
    /// </summary>
    public class StateCountriesWMSAPIService : IStateCountriesWMSAPIService
    {
        private readonly  IStateCountriesWMSAPIRepository _repository;
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
        public async Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetAllAsync(CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSStatesCountriesDTO>>("No states found.");

            var objlist = _mapper.Map<List<WMSStatesCountriesDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get all active states
        /// </summary>
        public async Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetActiveAsync(CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSStatesCountriesDTO>>("No active states found.");

            var objlist = _mapper.Map<List<WMSStatesCountriesDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get state by id
        /// </summary>
        public async Task<ApiResponse<WMSStatesCountriesDTO?>> GetByIdAsync(int id, CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSStatesCountriesDTO?>($"state with Id {id} was not found.");

            var objmap = _mapper.Map<WMSStatesCountriesDTO>(entity);

            return ApiResponseFactory.Ok(objmap);
        }

        /// <summary>
        /// create new state
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(WMSStatesCountriesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado
            var exists = await _repository.ExistsByNameAsync(entity.Name!, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_STATE_NAME",
                         message: $"There is already a state with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var objmap = _mapper.Map<DUNES.API.ModelsWMS.Masters.StatesCountries>(entity);
            
            await _repository.CreateAsync(objmap, ct);
            return ApiResponseFactory.Ok(true, "State created successfully.");
        }

        /// <summary>
        /// update state information
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSStatesCountriesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado excluyendo el propio Id
            var exists = await _repository.ExistsByNameAsync(entity.Name!, entity.Id, ct);
            if (exists)
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
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            var exists = await _repository.ExistsByNameAsync(name, excludeId, ct);

            // aquí tienes dos enfoques posibles:

            // 1) Regresar "exists" tal cual (true = ya existe, false = no existe):
            return ApiResponseFactory.Ok(exists);

            // 2) O si lo quieres como "isAvailable" (true = nombre disponible):
            // return ApiResponseFactory.Ok(!exists);
        }
    }
}

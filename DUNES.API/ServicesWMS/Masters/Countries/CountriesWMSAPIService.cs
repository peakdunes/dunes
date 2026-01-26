using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DUNES.API.ServicesWMS.Masters.Countries
{
    /// <summary>
    /// Country service implementation
    /// </summary>
    public class CountriesWMSAPIService : ICountriesWMSAPIService
    {
        private readonly ICountriesWMSAPIRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="repository"></param>
        ///  <param name="mapper"></param>

       
        public CountriesWMSAPIService(ICountriesWMSAPIRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// get all countries
        /// </summary>
        public async Task<ApiResponse<List<WMSCountriesDTO>>> GetAllAsync(CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCountriesDTO>>("No countries found.");

            var objlist = _mapper.Map<List<WMSCountriesDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get all active countries
        /// </summary>
        public async Task<ApiResponse<List<WMSCountriesDTO>>> GetActiveAsync(CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCountriesDTO>>("No active countries found.");

            var objlist = _mapper.Map<List<WMSCountriesDTO>>(data);

            return ApiResponseFactory.Ok(objlist);
        }

        /// <summary>
        /// get country by id
        /// </summary>
        public async Task<ApiResponse<WMSCountriesDTO>> GetByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCountriesDTO>("Country id required");
            }


            var entity = await _repository.GetByIdAsync(id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSCountriesDTO>($"Country with Id {id} was not found.");

            var objmap = _mapper.Map<WMSCountriesDTO>(entity);

            return ApiResponseFactory.Ok(objmap);
        }

        /// <summary>
        /// create new country
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(WMSCountriesDTO entity, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(entity.Sigla))
            {
                return ApiResponseFactory.BadRequest<bool>("ISO Country Code is required");
            }


            if (string.IsNullOrEmpty(entity.Name))
            {
                return ApiResponseFactory.BadRequest<bool>("Country Name is required");
            }

            // validar nombre duplicado
            var exists = await _repository.ExistsByNameAsync(entity.Name!, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_COUNTRY_NAME",
                         message: $"There is already a country with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var objmap = _mapper.Map<DUNES.API.ModelsWMS.Masters.Countries>(entity);

            await _repository.CreateAsync(objmap, ct);
            return ApiResponseFactory.Ok(true, "Country created successfully.");
        }

        /// <summary>
        /// update country information
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(WMSCountriesDTO entity, CancellationToken ct)
        {
            // validar nombre duplicado excluyendo el propio Id
            var exists = await _repository.ExistsByNameAsync(entity.Name!, entity.Id, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                         error: "DUPLICATE_COUNTRY_NAME",
                         message: $"There is already a country with the name '{entity.Name}'.",
                         statusCode: (int)HttpStatusCode.Conflict);
            }

            var current = await _repository.GetByIdAsync(entity.Id, ct);
            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>($"Country with Id {entity.Id} was not found.");
            }

            // si quieres, puedes copiar solo campos editables en vez de reemplazar la entidad
            current.Name = entity.Name;
            current.Active = entity.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(true, "Country updated successfully.");
        }

        /// <summary>
        /// activate / deactivate country
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"Country with Id {id} was not found.");

            var msg = isActive
                ? "Country has been activated successfully."
                : "Country has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }

        /// <summary>
        /// exist country name
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

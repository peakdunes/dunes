using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.Locations;
using DUNES.API.RepositoriesWMS.Masters.Racks;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.Racks
{
    /// <summary>
    /// Racks Service
    /// STANDARD COMPANYID compliant
    /// </summary>
    public class RacksWMSAPIService : IRacksWMSAPIService
    {
        private readonly IRacksWMSAPIRepository _repository;
        private readonly ICompaniesWMSAPIRepository _companyRepository;
        private readonly ILocationsWMSAPIRepository _locationRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public RacksWMSAPIService(
            IRacksWMSAPIRepository repository,
            IMapper mapper,
            ICompaniesWMSAPIRepository companyRepository,
            ILocationsWMSAPIRepository locationRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _companyRepository = companyRepository;
            _locationRepository = locationRepository;
        }

       
        /// <summary>
        /// Create a new rack for a specific company and location
        /// </summary>
        public async Task<ApiResponse<WMSRacksCreateDTO>> CreateAsync(
            int companyId,
            int locationId,
            WMSRacksCreateDTO dto,
            CancellationToken ct)
        {
            // 1️⃣ Validar Company
            var company = await _companyRepository.GetByIdAsync(companyId, ct);
            if (company == null)
                return ApiResponseFactory.BadRequest<WMSRacksCreateDTO>($"Company {companyId} does not exist");

            if (!company.Active)
                return ApiResponseFactory.BadRequest<WMSRacksCreateDTO>($"Company {company.Name} is not active");

            // 2️⃣ Validar Location
            var location = await _locationRepository.GetByIdAsync(companyId, locationId, ct);
            if (location == null)
                return ApiResponseFactory.BadRequest<WMSRacksCreateDTO>($"Location {locationId} does not exist");

            if (!location.Active)
                return ApiResponseFactory.BadRequest<WMSRacksCreateDTO>($"Location {location.Name} is not active");

            // 3️⃣ Validar datos
            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<WMSRacksCreateDTO>("Rack name is required");

            // 4️⃣ Validar duplicado
            var exists = await _repository.ExistsByNameAsync(
                companyId,
                locationId,
                dto.Name.Trim(),
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSRacksCreateDTO>(
                    error: "DUPLICATE_RACK_NAME",
                    message: $"There is already a rack with the name '{dto.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            // 5️⃣ Map DTO → Entity
            var entity = _mapper.Map<ModelsWMS.Masters.Racks>(dto);
            entity.Idcompany = companyId;
            entity.LocationsId = locationId;
            entity.Active = true;

            // 6️⃣ Persistir
            await _repository.CreateAsync(entity, ct);

            // 7️⃣ Map Entity → DTO
            var resultDto = _mapper.Map<WMSRacksCreateDTO>(entity);

            return ApiResponseFactory.Ok(resultDto, "Rack created successfully.");
        }

       
        /// <summary>
        /// Update an existing rack
        /// </summary>
        public async Task<ApiResponse<WMSRacksCreateDTO>> UpdateAsync(
            int companyId,
            int locationId,
            int id,
            WMSRacksCreateDTO dto,
            CancellationToken ct)
        {
            // 1️⃣ Buscar rack actual (con ownership)
            var current = await _repository.GetByIdAsync(companyId, locationId, id, ct);
            if (current == null)
                return ApiResponseFactory.NotFound<WMSRacksCreateDTO>($"Rack with Id {id} was not found.");

            // 2️⃣ Validar nombre
            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<WMSRacksCreateDTO>("Rack name is required");

            // 3️⃣ Validar duplicado (excluyendo el actual)
            var exists = await _repository.ExistsByNameAsync(
                companyId,
                locationId,
                dto.Name.Trim(),
                id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSRacksCreateDTO>(
                    error: "DUPLICATE_RACK_NAME",
                    message: $"There is already a rack with the name '{dto.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            // 4️⃣ Actualizar campos permitidos
            current.Name = dto.Name.Trim();
            current.LocationsId = locationId;
            current.Active = dto.Active;

            var infoupdate = _mapper.Map<DUNES.API.ModelsWMS.Masters.Racks>(current);

            await _repository.UpdateAsync(infoupdate, ct);

            // 5️⃣ Map Entity → DTO
            var resultDto = _mapper.Map<WMSRacksCreateDTO>(infoupdate);

            return ApiResponseFactory.Ok(resultDto, "Rack updated successfully.");
        }

      

        /// <summary>
        /// exist by name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            int locationId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ApiResponseFactory.BadRequest<bool>("Rack name is required");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                locationId,
                name.Trim(),
                excludeId,
                ct);

            return ApiResponseFactory.Ok(exists);
        }
        /// <summary>
        /// get all active racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSRacksQueryDTO>>> GetActiveAsync(
            int companyId,
            int locationId,
            CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(companyId, locationId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSRacksQueryDTO>>("No active racks found.");

            return ApiResponseFactory.Ok(_mapper.Map<List<WMSRacksQueryDTO>>(data));
        }
        /// <summary>
        /// get all racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSRacksQueryDTO>>> GetAllAsync(
            int companyId,
            int locationId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, locationId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSRacksQueryDTO>>("No racks found.");

            return ApiResponseFactory.Ok(_mapper.Map<List<WMSRacksQueryDTO>>(data));
        }

        /// <summary>
        /// get rack by id and company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<WMSRacksQueryDTO>> GetByIdAsync(
            int companyId,
            int locationId,
            int id,
            CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(companyId, locationId, id, ct);

            if (entity == null)
                return ApiResponseFactory.NotFound<WMSRacksQueryDTO>($"Rack with Id {id} was not found.");

            return ApiResponseFactory.Ok(_mapper.Map<WMSRacksQueryDTO>(entity));
        }

        /// <summary>
        /// Active no active change for a rack
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int locationId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var ok = await _repository.SetActiveAsync(companyId, locationId, id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"Rack with Id {id} was not found.");

            var msg = isActive
                ? "Rack has been activated successfully."
                : "Rack has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }
    }
}

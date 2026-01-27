using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Bins;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.Bins
{
    /// <summary>
    /// Bins Service
    /// Scoped by Company + Location + Rack
    /// </summary>
    public class BinsWMSAPIService : IBinsWMSAPIService
    {
        private readonly IBinsWMSAPIRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor DI
        /// </summary>
        public BinsWMSAPIService(
            IBinsWMSAPIRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all bins
        /// </summary>
        public async Task<ApiResponse<List<WMSBinsDto>>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsDto>>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsDto>>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsDto>>("Rack is required");

            var data = await _repository.GetAllAsync(companyId, locationId, rackId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSBinsDto>>("No bins found.");

            var result = _mapper.Map<List<WMSBinsDto>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all active bins
        /// </summary>
        public async Task<ApiResponse<List<WMSBinsDto>>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsDto>>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsDto>>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsDto>>("Rack is required");

            var data = await _repository.GetActiveAsync(companyId, locationId, rackId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSBinsDto>>("No active bins found.");

            var result = _mapper.Map<List<WMSBinsDto>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get bin by id
        /// </summary>
        public async Task<ApiResponse<WMSBinsDto>> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsDto>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsDto>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsDto>("Rack is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsDto>("Bin is required");

            var entity = await _repository.GetByIdAsync(companyId, locationId, rackId, id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSBinsDto>($"Bin with Id {id} was not found.");

            var result = _mapper.Map<WMSBinsDto>(entity);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Create new bin
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(
            WMSBinsDto entity,
            CancellationToken ct)
        {
            if (entity.Idcompany <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (entity.LocationsId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Location is required");

            if (entity.RacksId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Rack is required");

            if (string.IsNullOrWhiteSpace(entity.Name))
                return ApiResponseFactory.BadRequest<bool>("Bin name is required");

            var exists = await _repository.ExistsByNameAsync(
                entity.Idcompany,
                entity.LocationsId,
                entity.RacksId,
                entity.Name!,
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_BIN_NAME",
                    message: $"There is already a bin with the name '{entity.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var model = _mapper.Map<DUNES.API.ModelsWMS.Masters.Bines>(entity);

            await _repository.CreateAsync(model, ct);

            return ApiResponseFactory.Ok(true, "Bin created successfully.");
        }

        /// <summary>
        /// Update bin
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(
            WMSBinsDto entity,
            CancellationToken ct)
        {
            if (entity.Idcompany <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (entity.LocationsId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Location is required");

            if (entity.RacksId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Rack is required");

            if (entity.Id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Bin is required");

            if (string.IsNullOrWhiteSpace(entity.Name))
                return ApiResponseFactory.BadRequest<bool>("Bin name is required");

            var exists = await _repository.ExistsByNameAsync(
                entity.Idcompany,
                entity.LocationsId,
                entity.RacksId,
                entity.Name!,
                entity.Id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_BIN_NAME",
                    message: $"There is already a bin with the name '{entity.Name}'.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var current = await _repository.GetByIdAsync(
                entity.Idcompany,
                entity.LocationsId,
                entity.RacksId,
                entity.Id,
                ct);

            if (current is null)
                return ApiResponseFactory.NotFound<bool>($"Bin with Id {entity.Id} was not found.");

            current.Name = entity.Name;
            current.Active = entity.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(true, "Bin updated successfully.");
        }

        /// <summary>
        /// Activate / Deactivate bin
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Rack is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Bin is required");

            var ok = await _repository.SetActiveAsync(
                companyId,
                locationId,
                rackId,
                id,
                isActive,
                ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>($"Bin with Id {id} was not found.");

            var msg = isActive
                ? "Bin has been activated successfully."
                : "Bin has been deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }

        /// <summary>
        /// Check if bin exists by name
        /// </summary>
        public async Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            int locationId,
            int rackId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Rack is required");

            if (string.IsNullOrWhiteSpace(name))
                return ApiResponseFactory.BadRequest<bool>("Bin name is required");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                locationId,
                rackId,
                name,
                excludeId,
                ct);

            return ApiResponseFactory.Ok(exists);
        }
    }
}

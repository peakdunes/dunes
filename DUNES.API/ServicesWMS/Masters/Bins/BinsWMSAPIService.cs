using AutoMapper;
using DUNES.API.RepositoriesWMS.Masters.Bins;
using DUNES.API.ServicesWMS.Masters.ClientCompanies;
using DUNES.API.ServicesWMS.Masters.Companies;
using DUNES.API.ServicesWMS.Masters.Locations;
using DUNES.API.ServicesWMS.Masters.Racks;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.Bins
{
    /// <summary>
    /// Bins Service
    /// 
    /// Scoped by:
    /// Company (tenant) + Location + Rack
    /// 
    /// IMPORTANT:
    /// - Ownership is enforced at service level
    /// - DTOs must NOT be trusted for CompanyId, LocationId or RackId
    /// </summary>
    public class BinsWMSAPIService : IBinsWMSAPIService
    {
        private readonly IBinsWMSAPIRepository _repository;
        private readonly ICompaniesWMSAPIService _companiesservice;
        private readonly ILocationsWMSAPIService _locationservice;
        private readonly IRacksWMSAPIService _rackservice;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public BinsWMSAPIService(
            IBinsWMSAPIRepository repository,
            IMapper mapper, ILocationsWMSAPIService locationservice, IRacksWMSAPIService rackservice, ICompaniesWMSAPIService companiesservice)
        {
            _repository = repository;
            _mapper = mapper;
            _locationservice = locationservice;
            _rackservice = rackservice;
            _companiesservice = companiesservice;
        }

        /// <summary>
        /// Get all bins
        /// </summary>
        public async Task<ApiResponse<List<WMSBinsCreateDto>>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsCreateDto>>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsCreateDto>>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsCreateDto>>("Rack is required");

            var data = await _repository.GetAllAsync(companyId, locationId, rackId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSBinsCreateDto>>("No bins found.");

            var result = _mapper.Map<List<WMSBinsCreateDto>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all active bins
        /// </summary>
        public async Task<ApiResponse<List<WMSBinsCreateDto>>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsCreateDto>>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsCreateDto>>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSBinsCreateDto>>("Rack is required");

            var data = await _repository.GetActiveAsync(companyId, locationId, rackId, ct);

            if (data == null || data.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSBinsCreateDto>>("No active bins found.");

            var result = _mapper.Map<List<WMSBinsCreateDto>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get bin by id
        /// </summary>
        public async Task<ApiResponse<WMSBinsCreateDto>> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsCreateDto>("Company is required");

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsCreateDto>("Location is required");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsCreateDto>("Rack is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSBinsCreateDto>("Bin is required");

            var entity = await _repository.GetByIdAsync(companyId, locationId, rackId, id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSBinsCreateDto>($"Bin with Id {id} was not found.");

            var result = _mapper.Map<WMSBinsCreateDto>(entity);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Create a new bin
        /// </summary>
        public async Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            int locationId,
            int rackId,
            WMSBinsCreateDto dto,
            CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");



            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Location is required");

            var infoloc = await _locationservice.GetByIdAsync(companyId, locationId, ct);

            if (infoloc is null || infoloc.Data is null) return ApiResponseFactory.NotFound<bool>("Location not found");

            if (!infoloc.Data.Active) return ApiResponseFactory.BadRequest<bool>("Location  is not active");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Rack is required");

            var inforack = await _rackservice.GetByIdAsync(companyId, locationId, rackId, ct);

            if (inforack is null || inforack.Data is null) return ApiResponseFactory.NotFound<bool>("Rack not found");

            if (!inforack.Data.Active) return ApiResponseFactory.BadRequest<bool>("Rack  is not active");



            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Bin name is required");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                locationId,
                rackId,
                dto.Name!,
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_BIN_NAME",
                    message: $"There is already a bin with the name '{dto.Name}'.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            var entity = _mapper.Map<ModelsWMS.Masters.Bines>(dto);
            entity.Idcompany = companyId;
            entity.LocationsId = locationId;
            entity.RacksId = rackId;

            await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Ok(true, "Bin created successfully.");
        }

        /// <summary>
        /// Update an existing bin
        /// </summary>
        public async Task<ApiResponse<bool>> UpdateAsync(int companyId, int locationId, int rackId, int id,
            WMSBinsCreateDto dto, CancellationToken ct)
        {

            if (locationId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Location is required");

            var infoloc = await _locationservice.GetByIdAsync(companyId, locationId, ct);

            if (infoloc is null || infoloc.Data is null) return ApiResponseFactory.NotFound<bool>("Location not found");

            if (!infoloc.Data.Active) return ApiResponseFactory.BadRequest<bool>("Location  is not active");

            if (rackId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Rack is required");

            var inforack = await _rackservice.GetByIdAsync(companyId, locationId, rackId, ct);

            if (inforack is null || inforack.Data is null) return ApiResponseFactory.NotFound<bool>("Rack not found");

            if (!inforack.Data.Active) return ApiResponseFactory.BadRequest<bool>("Rack  is not active");



            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Bin is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Bin name is required");

            var current = await _repository.GetByIdAsync(
                companyId,
                locationId,
                rackId,
                id,
                ct);

            if (current is null)
                return ApiResponseFactory.NotFound<bool>($"Bin with Id {id} was not found.");

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                locationId,
                rackId,
                dto.Name!,
                id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_BIN_NAME",
                    message: $"There is already a bin with the name '{dto.Name}'.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            current.Name = dto.Name;
            current.Active = dto.Active;

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
        /// Check if a bin exists by name
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

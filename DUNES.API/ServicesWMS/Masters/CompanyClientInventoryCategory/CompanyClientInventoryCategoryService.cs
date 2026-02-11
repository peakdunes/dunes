using DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryCategory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Business logic service for managing inventory categories per client.
    /// </summary>
    public class CompanyClientInventoryCategoryService : ICompanyClientInventoryCategoryService
    {
        private readonly ICompanyClientInventoryCategoryWMSAPIRepository _repository;

        /// <summary>
        /// Constructor (Dependency Injection)
        /// </summary>
        public CompanyClientInventoryCategoryService(ICompanyClientInventoryCategoryWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSCompanyClientInventoryCategoryReadDTO>>("Company or Client is required.");

            var result = await _repository.GetAllAsync(companyId, companyClientId, ct);

            if (result.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompanyClientInventoryCategoryReadDTO>>("No mappings found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryCategoryReadDTO>("Company or Client is required.");

            var result = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);

            if (result is null)
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryCategoryReadDTO>("Mapping not found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryCategoryReadDTO>("Company or Client is required.");

            if (dto.InventoryCategoryId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryCategoryReadDTO>("InventoryCategoryId is required.");

            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.InventoryCategoryId, null, ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryCategoryReadDTO>(
                    "DUPLICATE_MAPPING",
                    "This inventory category is already assigned to the client.",
                    (int)HttpStatusCode.Conflict);
            }

            var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);
            return ApiResponseFactory.Created(created, "Mapping created.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryCategoryUpdateDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company or Client is required.");

            if (dto.Id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Mapping Id is required.");

            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.InventoryCategoryId, dto.Id, ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    "DUPLICATE_MAPPING",
                    "This inventory category is already assigned to the client.",
                    (int)HttpStatusCode.Conflict);
            }

            var success = await _repository.UpdateAsync(dto, companyId, companyClientId, ct);

            if (!success)
                return ApiResponseFactory.NotFound<bool>("Mapping not found.");

            return ApiResponseFactory.Ok(true, "Mapping updated successfully.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company or Client is required.");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Mapping Id is required.");

            var updated = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

            if (!updated)
                return ApiResponseFactory.NotFound<bool>("Mapping not found.");

            return ApiResponseFactory.Ok(true, isActive ? "Mapping activated." : "Mapping deactivated.");
        }
    }
}

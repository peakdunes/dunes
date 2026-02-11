using DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Service implementation for client-inventory type mappings.
    /// Enforces validation and scoped access control by company and client.
    /// </summary>
    public class CompanyClientInventoryTypeService : ICompanyClientInventoryTypeService
    {
        private readonly ICompanyClientInventoryTypeWMSAPIRepository _repository;


        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="repository"></param>
        public CompanyClientInventoryTypeService(ICompanyClientInventoryTypeWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSCompanyClientInventoryTypeReadDTO>>("Invalid company or client context.");

            var result = await _repository.GetAllAsync(companyId, companyClientId, ct);

            if (result is null || result.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompanyClientInventoryTypeReadDTO>>("No mappings found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Invalid company or client context.");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Invalid mapping ID.");

            var entity = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryTypeReadDTO>("Mapping not found.");

            return ApiResponseFactory.Ok(entity);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryTypeCreateDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Invalid company or client context.");

            if (dto.InventoryTypeId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Inventory Type is required.");

            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.InventoryTypeId, null, ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryTypeReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This inventory type is already assigned to the client.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);
            return ApiResponseFactory.Ok(created, "Mapping created successfully.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0 || dto.Id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Invalid context or mapping ID.");

            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.InventoryTypeId, dto.Id, ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_MAPPING",
                    message: "This inventory type is already assigned to the client.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var updated = await _repository.UpdateAsync(dto, companyId, companyClientId, ct);

            if (!updated)
                return ApiResponseFactory.NotFound<bool>("Mapping not found or unauthorized.");

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
            if (companyId <= 0 || companyClientId <= 0 || id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Invalid identifiers.");

            var ok = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

            if (!ok)
                return ApiResponseFactory.NotFound<bool>("Mapping not found.");

            var msg = isActive ? "Mapping activated." : "Mapping deactivated.";
            return ApiResponseFactory.Ok(true, msg);
        }
    }
}

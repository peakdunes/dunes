using DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// Service implementation for client-inventory type enablement.
    /// Anti-error design:
    /// - No UpdateAsync (avoid changing InventoryTypeId accidentally).
    /// - Use GetEnabledAsync and SetEnabledSetAsync.
    /// </summary>
    public class CompanyClientInventoryTypeService : ICompanyClientInventoryTypeService
    {
        private readonly ICompanyClientInventoryTypeWMSAPIRepository _repository;

        /// <summary>Constructor (DI).</summary>
        public CompanyClientInventoryTypeService(ICompanyClientInventoryTypeWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSCompanyClientInventoryTypeReadDTO>>("Invalid company or client context.");

            var result = await _repository.GetEnabledAsync(companyId, companyClientId, ct);

            if (result is null || result.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompanyClientInventoryTypeReadDTO>>("No enabled mappings found.");

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

            if (dto is null)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Payload is required.");

            if (dto.InventoryTypeId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("InventoryTypeId is required.");

            // Enforce: master must be active
            var masterActive = await _repository.IsMasterActiveAsync(companyId, dto.InventoryTypeId, ct);
            if (!masterActive)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryTypeReadDTO>(
                    error: "MASTER_INACTIVE",
                    message: "The selected inventory type is inactive (master catalog).",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }

            // Enforce: no duplicates
            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.InventoryTypeId, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryTypeReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This inventory type is already assigned to the client.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            try
            {
                var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);
                return ApiResponseFactory.Created(created, "Mapping created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryTypeReadDTO>(
                    error: "CREATE_FAILED",
                    message: ex.Message,
                    statusCode: (int)HttpStatusCode.BadRequest);
            }
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

            try
            {
                var ok = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

                if (!ok)
                    return ApiResponseFactory.NotFound<bool>("Mapping not found.");

                var msg = isActive ? "Mapping activated." : "Mapping deactivated.";
                return ApiResponseFactory.Ok(true, msg);
            }
            catch (InvalidOperationException ex)
            {
                // Typically: trying to activate while master is inactive
                return ApiResponseFactory.Fail<bool>(
                    error: "MASTER_INACTIVE",
                    message: ex.Message,
                    statusCode: (int)HttpStatusCode.BadRequest);
            }
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<bool>> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryTypeIds,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Invalid company or client context.");

            inventoryTypeIds ??= new List<int>();

            // sanitize
            inventoryTypeIds = inventoryTypeIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            try
            {
                var ok = await _repository.SetEnabledSetAsync(companyId, companyClientId, inventoryTypeIds, ct);

                if (!ok)
                    return ApiResponseFactory.BadRequest<bool>("Unable to update enabled inventory types.");

                return ApiResponseFactory.Ok(true, "Enabled inventory types updated.");
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "INVALID_OR_INACTIVE_MASTER",
                    message: ex.Message,
                    statusCode: (int)HttpStatusCode.BadRequest);
            }
        }
    }

}

using DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryCategory;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Business logic service for managing inventory categories per client.
    /// Anti-error design:
    /// - No UpdateAsync (avoid changing category IDs accidentally).
    /// - Use GetEnabledAsync and SetEnabledSetAsync.
    /// </summary>
    public class CompanyClientInventoryCategoryService : ICompanyClientInventoryCategoryService
    {
        private readonly ICompanyClientInventoryCategoryWMSAPIRepository _repository;

        /// <summary>Constructor (Dependency Injection).</summary>
        public CompanyClientInventoryCategoryService(ICompanyClientInventoryCategoryWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSCompanyClientInventoryCategoryReadDTO>>("Company or Client is required.");

            var result = await _repository.GetEnabledAsync(companyId, companyClientId, ct);

            //if (result.Count == 0)
            //    return ApiResponseFactory.NotFound<List<WMSCompanyClientInventoryCategoryReadDTO>>("No enabled categories found.");

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

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryCategoryReadDTO>("Mapping Id is required.");

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

            if (dto is null)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryCategoryReadDTO>("Payload is required.");

            if (dto.InventoryCategoryId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryCategoryReadDTO>("InventoryCategoryId is required.");

            // Enforce: master must be active
            var masterActive = await _repository.IsMasterActiveAsync(companyId, dto.InventoryCategoryId, ct);
            if (!masterActive)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryCategoryReadDTO>(
                    "MASTER_INACTIVE",
                    "The selected inventory category is inactive (master catalog).",
                    (int)HttpStatusCode.BadRequest);
            }

            // Enforce: no duplicates
            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.InventoryCategoryId, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryCategoryReadDTO>(
                    "DUPLICATE_MAPPING",
                    "This inventory category is already assigned to the client.",
                    (int)HttpStatusCode.Conflict);
            }

            try
            {
                var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);
                return ApiResponseFactory.Created(created, "Mapping created.");
            }
            catch (InvalidOperationException ex)
            {
                // Repo may throw for invalid/inactive master or duplicates (extra safety)
                return ApiResponseFactory.Fail<WMSCompanyClientInventoryCategoryReadDTO>(
                    "CREATE_FAILED",
                    ex.Message,
                    (int)HttpStatusCode.BadRequest);
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
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company or Client is required.");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Mapping Id is required.");

            try
            {
                var updated = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

                if (!updated)
                    return ApiResponseFactory.NotFound<bool>("Mapping not found.");

                return ApiResponseFactory.Ok(true, isActive ? "Mapping activated." : "Mapping deactivated.");
            }
            catch (InvalidOperationException ex)
            {
                // Most likely: trying to activate while master is inactive
                return ApiResponseFactory.Fail<bool>(
                    "MASTER_INACTIVE",
                    ex.Message,
                    (int)HttpStatusCode.BadRequest);
            }
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<bool>> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryCategoryIds,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company or Client is required.");

            inventoryCategoryIds ??= new List<int>();

            // Basic sanitation
            inventoryCategoryIds = inventoryCategoryIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            try
            {
                var ok = await _repository.SetEnabledSetAsync(companyId, companyClientId, inventoryCategoryIds, ct);

                if (!ok)
                    return ApiResponseFactory.BadRequest<bool>("Unable to update enabled categories.");

                return ApiResponseFactory.Ok(true, "Enabled categories updated.");
            }
            catch (InvalidOperationException ex)
            {
                // Repo throws when list contains invalid/inactive master categories
                return ApiResponseFactory.Fail<bool>(
                    "INVALID_OR_INACTIVE_MASTER",
                    ex.Message,
                    (int)HttpStatusCode.BadRequest);
            }
        }
    }
}

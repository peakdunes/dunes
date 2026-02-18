using DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service implementation for client-level item status enablement.
    /// Anti-error design:
    /// - No UpdateAsync (avoid changing ItemStatusId accidentally).
    /// - Use GetEnabledAsync and SetEnabledSetAsync.
    /// </summary>
    public class CompanyClientItemStatusService : ICompanyClientItemStatusService
    {
        private readonly ICompanyClientItemStatusWMSAPIRepository _repository;

        /// <summary>Constructor (DI).</summary>
        public CompanyClientItemStatusService(ICompanyClientItemStatusWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSCompanyClientItemStatusReadDTO>>("Invalid company or client context.");

            var result = await _repository.GetEnabledAsync(companyId, companyClientId, ct);

            if (result is null || result.Count == 0)
                return ApiResponseFactory.NotFound<List<WMSCompanyClientItemStatusReadDTO>>("No enabled mappings found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Invalid company or client context.");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Invalid mapping ID.");

            var result = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);

            if (result is null)
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>("Mapping not found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Invalid company or client context.");

            if (dto is null)
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Payload is required.");

            if (dto.ItemStatusId <= 0)
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("ItemStatusId is required.");

            // Rule: master must be active
            var masterActive = await _repository.IsMasterActiveAsync(companyId, dto.ItemStatusId, ct);
            if (!masterActive)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientItemStatusReadDTO>(
                    error: "MASTER_INACTIVE",
                    message: "The selected item status is inactive (master catalog).",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }

            // Rule: no duplicates
            var exists = await _repository.ExistsAsync(companyId, companyClientId, dto.ItemStatusId, null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientItemStatusReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This item status is already assigned to the client.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            try
            {
                var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);
                return ApiResponseFactory.Created(created, "Mapping created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientItemStatusReadDTO>(
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
                // Most common: trying to activate while master is inactive
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
            List<int> itemStatusIds,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Invalid company or client context.");

            itemStatusIds ??= new List<int>();
            itemStatusIds = itemStatusIds.Where(x => x > 0).Distinct().ToList();

            try
            {
                var ok = await _repository.SetEnabledSetAsync(companyId, companyClientId, itemStatusIds, ct);

                if (!ok)
                    return ApiResponseFactory.BadRequest<bool>("Unable to update enabled item statuses.");

                return ApiResponseFactory.Ok(true, "Enabled item statuses updated.");
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

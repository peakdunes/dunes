using DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service implementation for managing item statuses per client.
    /// Handles validations and business logic.
    /// </summary>
    public class CompanyClientItemStatusService : ICompanyClientItemStatusService
    {
        private readonly ICompanyClientItemStatusWMSAPIRepository _repository;


        /// <summary>
        /// construction (DI)
        /// </summary>
        /// <param name="repository"></param>
        public CompanyClientItemStatusService(ICompanyClientItemStatusWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<WMSCompanyClientItemStatusReadDTO>>(
                    "Company and client IDs are required.");
            }

            var result = await _repository.GetAllAsync(companyId, companyClientId, ct);
            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0 || id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                    "Invalid input parameters.");
            }

            var result = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);

            if (result is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>(
                    "Item status mapping not found.");
            }

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
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                    "Company and client IDs are required.");
            }

            if (dto.ItemStatusId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                    "ItemStatusId is required.");
            }

            var exists = await _repository.ExistsAsync(
                companyId,
                companyClientId,
                dto.ItemStatusId,
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSCompanyClientItemStatusReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This item status is already assigned to the client.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);
            return ApiResponseFactory.Created(created, "Item status mapping created.");
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientItemStatusUpdateDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0 || companyClientId <= 0 || dto.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Invalid input parameters.");
            }

            var exists = await _repository.ExistsAsync(
                companyId,
                companyClientId,
                dto.ItemStatusId,
                dto.Id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_MAPPING",
                    message: "This item status is already assigned to the client.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var updated = await _repository.UpdateAsync(dto, companyId, companyClientId, ct);

            if (!updated)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Item status mapping not found.");
            }

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
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Invalid input parameters.");
            }

            var success = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

            if (!success)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Item status mapping not found.");
            }

            var msg = isActive
                ? "Mapping activated successfully."
                : "Mapping deactivated successfully.";

            return ApiResponseFactory.Ok(true, msg);
        }
    }
}

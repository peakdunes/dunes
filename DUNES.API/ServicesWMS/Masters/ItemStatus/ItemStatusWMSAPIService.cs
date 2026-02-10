using DUNES.API.RepositoriesWMS.Masters.ItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.ItemStatus
{
    /// <summary>
    /// Business logic for managing Item Status (WMS).
    /// All operations are scoped by company (tenant) following STANDARD COMPANYID.
    /// </summary>
    public class ItemStatusWMSAPIService : IItemStatusWMSAPIService
    {
        private readonly IItemStatusWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of <see cref="ItemStatusWMSAPIService"/>.
        /// </summary>
        /// <param name="repository">Item status repository (tenant-scoped).</param>
        public ItemStatusWMSAPIService(IItemStatusWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetAllAsync(int companyId, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSItemStatusReadDTO>>("Company is required");

            var result = await _repository.GetAllAsync(companyId, ct);

            return ApiResponseFactory.Ok(result ?? new List<WMSItemStatusReadDTO>());
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetActiveAsync(int companyId, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSItemStatusReadDTO>>("Company is required");

            var result = await _repository.GetActiveAsync(companyId, ct);

            return ApiResponseFactory.Ok(result ?? new List<WMSItemStatusReadDTO>());
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSItemStatusReadDTO>> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<WMSItemStatusReadDTO>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSItemStatusReadDTO>("Status Id is required");

            var result = await _repository.GetByIdAsync(companyId, id, ct);

            if (result is null)
                return ApiResponseFactory.NotFound<WMSItemStatusReadDTO>("Item status not found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> CreateAsync(int companyId, WMSItemStatusCreateDTO dto, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (dto is null)
                return ApiResponseFactory.BadRequest<bool>("Body is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Status name is required");

            var exists = await _repository.ExistsByNameAsync(companyId, dto.Name, excludeId: null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_ITEM_STATUS",
                    message: $"An item status with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var created = await _repository.CreateAsync(dto, companyId, ct);
            return ApiResponseFactory.Ok(true, $"Item status '{created.Name}' created successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> UpdateAsync(int companyId, int id, WMSItemStatusUpdateDTO dto, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Status Id is required");

            if (dto is null)
                return ApiResponseFactory.BadRequest<bool>("Body is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Status name is required");

            var existing = await _repository.GetByIdAsync(companyId, id, ct);
            if (existing is null)
                return ApiResponseFactory.NotFound<bool>("Item status not found.");

            var duplicate = await _repository.ExistsByNameAsync(companyId, dto.Name, excludeId: id, ct);
            if (duplicate)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_ITEM_STATUS",
                    message: $"An item status with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            dto.Id = id;

            await _repository.UpdateAsync(dto, companyId, ct);
            return ApiResponseFactory.Ok(true, $"Item status '{dto.Name}' updated successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Status Id is required");

            var ok = await _repository.SetActiveAsync(companyId, id, isActive, ct);
            if (!ok)
                return ApiResponseFactory.NotFound<bool>("Item status not found.");

            var message = isActive
                ? "Item status activated successfully."
                : "Item status deactivated successfully.";

            return ApiResponseFactory.Ok(true, message);
        }
    }
}

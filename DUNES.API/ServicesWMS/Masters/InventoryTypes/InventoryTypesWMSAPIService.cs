using DUNES.API.RepositoriesWMS.Masters.InventoryTypes;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.InventoryTypes
{
    /// <summary>
    /// Business logic for managing Inventory Types (WMS).
    /// All operations are scoped by company (tenant) following STANDARD COMPANYID.
    /// </summary>
    public class InventoryTypesWMSAPIService : IInventoryTypesWMSAPIService
    {
        private readonly IInventoryTypesWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryTypesWMSAPIService"/>.
        /// </summary>
        /// <param name="repository">Inventory types repository (tenant-scoped).</param>
        public InventoryTypesWMSAPIService(IInventoryTypesWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetAllAsync(int companyId, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSInventoryTypesReadDTO>>("Company is required");

            var result = await _repository.GetAllAsync(companyId, ct);

            // Recommended: 200 with empty list if no data.
            return ApiResponseFactory.Ok(result ?? new List<WMSInventoryTypesReadDTO>());
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetActiveAsync(int companyId, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSInventoryTypesReadDTO>>("Company is required");

            var result = await _repository.GetActiveAsync(companyId, ct);

            // Recommended: 200 with empty list if no active records.
            return ApiResponseFactory.Ok(result ?? new List<WMSInventoryTypesReadDTO>());
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSInventoryTypesReadDTO>> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<WMSInventoryTypesReadDTO>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSInventoryTypesReadDTO>("Type Id is required");

            var result = await _repository.GetByIdAsync(companyId, id, ct);

            if (result is null)
                return ApiResponseFactory.NotFound<WMSInventoryTypesReadDTO>("Inventory type not found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> CreateAsync(int companyId, WMSInventoryTypesCreateDTO dto, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (dto is null)
                return ApiResponseFactory.BadRequest<bool>("Body is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Type name is required");

            var exists = await _repository.ExistsByNameAsync(companyId, dto.Name, excludeId: null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_INVENTORY_TYPE",
                    message: $"An inventory type with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var created = await _repository.CreateAsync(dto, companyId, ct);
            return ApiResponseFactory.Ok(true, $"Inventory type '{created.Name}' created successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> UpdateAsync(int companyId, int id, WMSInventoryTypesUpdateDTO dto, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Type Id is required");

            if (dto is null)
                return ApiResponseFactory.BadRequest<bool>("Body is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Type name is required");

            var existing = await _repository.GetByIdAsync(companyId, id, ct);
            if (existing is null)
                return ApiResponseFactory.NotFound<bool>("Inventory type not found.");

            var duplicate = await _repository.ExistsByNameAsync(companyId, dto.Name, excludeId: id, ct);
            if (duplicate)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_INVENTORY_TYPE",
                    message: $"An inventory type with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            // Route ID is authoritative (prevents tampering).
            dto.Id = id;

            await _repository.UpdateAsync(dto, companyId, ct);
            return ApiResponseFactory.Ok(true, $"Inventory type '{dto.Name}' updated successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Type Id is required");

            var ok = await _repository.SetActiveAsync(companyId, id, isActive, ct);
            if (!ok)
                return ApiResponseFactory.NotFound<bool>("Inventory type not found.");

            var message = isActive
                ? "Inventory type activated successfully."
                : "Inventory type deactivated successfully.";

            return ApiResponseFactory.Ok(true, message);
        }
        /// <summary>
        /// Deletes an inventory type master record physically only when it is not referenced
        /// by client mappings or transactional records.
        /// </summary>
        /// <param name="id">Inventory type id.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <see cref="ApiResponse{T}"/> with <c>true</c> when deleted successfully.
        /// Returns a failure response when the record is not found or has dependencies.
        /// </returns>
        public async Task<ApiResponse<bool>> DeleteAsync(int companyId, int id, CancellationToken ct)
        {
            // 1) Validate existence first (clear response instead of generic exception path)
            var current = await _repository.GetByIdAsync(companyId, id, ct);
            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>("Inventory type not found.");
            }

            // 2) Validate dependencies across all clients for the company
            var hasDependencies = await _repository.HasDependenciesAsync(companyId, id, ct);
            if (hasDependencies)
            {
                return ApiResponseFactory.Fail<bool>(error: "INVENTORY_IN_USE",
                     message: "Inventory type is in use and cannot be deleted. Deactivate it instead.",
                     statusCode: 409);


            }
            // 3) Physical delete
            var deleted = await _repository.DeleteAsync(companyId, id, ct);

            // 4) Return standard response
            return ApiResponseFactory.Success(
                data: deleted,
                message: "Inventory type deleted successfully.",
                statusCode: 200);
        }
    }
}

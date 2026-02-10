using DUNES.API.RepositoriesWMS.Masters.InventoryCategories;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Business logic for managing Inventory Categories in the WMS module.
    /// All operations are scoped by CompanyId (tenant) following STANDARD COMPANYID.
    /// </summary>
    public class InventoryCategoriesWMSAPIService : IInventoryCategoriesWMSAPIService
    {
        private readonly IInventoryCategoriesWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryCategoriesWMSAPIService"/>.
        /// </summary>
        /// <param name="repository">
        /// Repository for Inventory Categories.
        /// All repository calls must receive the <c>companyId</c> and enforce tenant scoping.
        /// </param>
        public InventoryCategoriesWMSAPIService(IInventoryCategoriesWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all inventory categories for a given company (tenant).
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier. Must be &gt; 0.
        /// This value must come from the authenticated token (STANDARD COMPANYID),
        /// never from request body/querystring.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// 200 OK with a list (can be empty) of <see cref="WMSInventorycategoriesReadDTO"/>.
        /// 400 BadRequest if <paramref name="companyId"/> is invalid.
        /// </returns>
        public async Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetAllAsync(int companyId, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<List<WMSInventorycategoriesReadDTO>>("Company is required");

            var result = await _repository.GetAllAsync(companyId, ct);

            // Recommended REST behavior: return 200 with an empty list instead of 404.
            return ApiResponseFactory.Ok(result ?? new List<WMSInventorycategoriesReadDTO>());
        }

        /// <summary>
        /// Retrieves a single inventory category by its ID for a given company (tenant).
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier. Must be &gt; 0.
        /// Must come from token (STANDARD COMPANYID).
        /// </param>
        /// <param name="id">Inventory category identifier. Must be &gt; 0.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// 200 OK with the category.
        /// 404 NotFound if the category does not exist for the given tenant.
        /// 400 BadRequest if parameters are invalid.
        /// </returns>
        public async Task<ApiResponse<WMSInventorycategoriesReadDTO>> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<WMSInventorycategoriesReadDTO>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<WMSInventorycategoriesReadDTO>("Category Id is required");

            var result = await _repository.GetByIdAsync(companyId, id, ct);

            if (result is null)
                return ApiResponseFactory.NotFound<WMSInventorycategoriesReadDTO>("Inventory category not found.");

            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Creates a new inventory category for a given company (tenant).
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier. Must be &gt; 0 and must come from token (STANDARD COMPANYID).
        /// </param>
        /// <param name="dto">Create DTO containing the required category data.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// 200 OK when created successfully.
        /// 409 Conflict if a duplicate name already exists within the tenant.
        /// 400 BadRequest if input is invalid.
        /// </returns>
        public async Task<ApiResponse<bool>> CreateAsync(int companyId, WMSInventorycategoriesCreateDTO dto, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (dto is null)
                return ApiResponseFactory.BadRequest<bool>("Body is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Category name is required");

            var exists = await _repository.ExistsByNameAsync(companyId, dto.Name, excludeId: null, ct);
            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_CATEGORY",
                    message: $"An inventory category with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var created = await _repository.CreateAsync(dto, companyId, ct);

            return ApiResponseFactory.Ok(true, $"Category '{created.Name}' created successfully.");
        }

        /// <summary>
        /// Updates an existing inventory category for a given company (tenant).
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier. Must be &gt; 0 and must come from token (STANDARD COMPANYID).
        /// </param>
        /// <param name="id">
        /// Inventory category identifier (from route). Must be &gt; 0.
        /// Route ID is authoritative; DTO ID is forced to this value to prevent tampering.
        /// </param>
        /// <param name="dto">Update DTO containing the updated values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// 200 OK when updated successfully.
        /// 404 NotFound if the category does not exist for the tenant.
        /// 409 Conflict if the update would cause a duplicate name.
        /// 400 BadRequest if input is invalid.
        /// </returns>
        public async Task<ApiResponse<bool>> UpdateAsync(int companyId, int id, WMSInventorycategoriesUpdateDTO dto, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Category Id is required");

            if (dto is null)
                return ApiResponseFactory.BadRequest<bool>("Body is required");

            if (string.IsNullOrWhiteSpace(dto.Name))
                return ApiResponseFactory.BadRequest<bool>("Category name is required");

            // Ensure the record exists within the tenant.
            var existing = await _repository.GetByIdAsync(companyId, id, ct);
            if (existing is null)
                return ApiResponseFactory.NotFound<bool>("Inventory category not found.");

            // Prevent duplicate names within the tenant, excluding the current record.
            var duplicate = await _repository.ExistsByNameAsync(companyId, dto.Name, excludeId: id, ct);
            if (duplicate)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_CATEGORY",
                    message: $"An inventory category with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            // Route ID is authoritative (prevents DTO tampering).
            dto.Id = id;

            await _repository.UpdateAsync(dto, companyId, ct);

            return ApiResponseFactory.Ok(true, $"Category '{dto.Name}' updated successfully.");
        }

        /// <summary>
        /// Activates or deactivates an inventory category for a given company (tenant).
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier. Must be &gt; 0 and must come from token (STANDARD COMPANYID).
        /// </param>
        /// <param name="id">Inventory category identifier. Must be &gt; 0.</param>
        /// <param name="isActive">True to activate; false to deactivate.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// 200 OK if the status was updated.
        /// 404 NotFound if the category does not exist for the tenant.
        /// 400 BadRequest if input is invalid.
        /// </returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            if (companyId <= 0)
                return ApiResponseFactory.BadRequest<bool>("Company is required");

            if (id <= 0)
                return ApiResponseFactory.BadRequest<bool>("Category Id is required");

            var ok = await _repository.SetActiveAsync(companyId, id, isActive, ct);
            if (!ok)
                return ApiResponseFactory.NotFound<bool>("Inventory category not found.");

            var message = isActive
                ? "Inventory category activated successfully."
                : "Inventory category deactivated successfully.";

            return ApiResponseFactory.Ok(true, message);
        }
    }
}

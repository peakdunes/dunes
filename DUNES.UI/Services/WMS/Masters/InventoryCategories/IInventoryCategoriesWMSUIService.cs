using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.InventoryCategories
{
    public interface IInventoryCategoriesWMSUIService
    {

        /// <summary>
        /// Retrieves all inventory categories for the current tenant.
        /// API: GET /api/wms/masters/inventory-categories/GetAll
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetAllAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves a single inventory category by id for the current tenant.
        /// API: GET /api/wms/masters/inventory-categories/GetById/{id}
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSInventorycategoriesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// Creates a new inventory category for the current tenant.
        /// API: POST /api/wms/masters/inventory-categories/Create
        /// </summary>
        /// <param name="entity">Create DTO.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> CreateAsync(WMSInventorycategoriesCreateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Updates an existing inventory category (route id is authoritative).
        /// API: PUT /api/wms/masters/inventory-categories/Update/{id}
        /// </summary>
        /// <param name="id">Category Id (route).</param>
        /// <param name="entity">Update DTO.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> UpdateAsync(int id, WMSInventorycategoriesUpdateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an inventory category.
        /// API: PATCH /api/wms/masters/inventory-categories/SetActive/{id}?isActive=true|false
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <param name="isActive">Activation flag.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);

        /// <summary>
        /// Checks whether a category name exists (optional excludeId).
        /// NOTE: Requires API endpoint ExistsByName to exist.
        /// Recommended API: GET /api/wms/masters/inventory-categories/ExistsByName?name=...&excludeId=...
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <param name="excludeId">Optional exclude id.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct);

        /// <summary>
        /// Hard deletes an inventory category if not used by any client mapping.
        /// API: DELETE /api/wms/masters/inventory-categories/Delete/{id}
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Category Id.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> DeleteByIdAsync(string token, int id, CancellationToken ct);
    }
}


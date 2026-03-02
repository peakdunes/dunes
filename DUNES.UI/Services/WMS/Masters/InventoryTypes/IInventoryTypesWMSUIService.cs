using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.InventoryTypes
{
    /// <summary>
    /// UI Contract for Inventory Types (WMS / Masters).
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER sent from UI.
    /// - API obtains CompanyId from the authenticated token.
    /// </summary>
    public interface IInventoryTypesWMSUIService
    {
        /// <summary>
        /// Retrieves all inventory types for the current tenant.
        /// API: GET /api/wms/masters/inventory-types/GetAll
        /// </summary>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetAllAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves active inventory types for the current tenant.
        /// API: GET /api/wms/masters/inventory-types/GetActive
        /// </summary>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetActiveAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves a single inventory type by id.
        /// API: GET /api/wms/masters/inventory-types/GetById/{id}
        /// </summary>
        /// <param name="id">Inventory type identifier.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSInventoryTypesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// Creates a new inventory type.
        /// API: POST /api/wms/masters/inventory-types/Create
        /// </summary>
        /// <param name="entity">Create DTO.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> CreateAsync(WMSInventoryTypesCreateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Updates an existing inventory type (route id is authoritative).
        /// API: PUT /api/wms/masters/inventory-types/Update/{id}
        /// </summary>
        /// <param name="id">Inventory type identifier (route).</param>
        /// <param name="entity">Update DTO.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> UpdateAsync(int id, WMSInventoryTypesUpdateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an inventory type.
        /// API: PATCH /api/wms/masters/inventory-types/SetActive/{id}?isActive=true|false
        /// </summary>
        /// <param name="id">Inventory type identifier.</param>
        /// <param name="isActive">True to activate; false to deactivate.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);

        /// <summary>
        /// Deletes an inventory type (hard delete).
        /// API: DELETE /api/wms/masters/inventory-types/Delete/{id}
        /// </summary>
        /// <param name="id">Inventory type identifier.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> DeleteByIdAsync(int id, string token, CancellationToken ct);
    }
}

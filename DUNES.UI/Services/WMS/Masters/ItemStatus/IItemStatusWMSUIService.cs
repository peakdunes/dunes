using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.ItemStatus
{
    /// <summary>
    /// UI Contract for Item Status (WMS / Masters).
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER sent from UI.
    /// - API obtains CompanyId from the authenticated token.
    /// </summary>
    public interface IItemStatusWMSUIService
    {
        /// <summary>
        /// Retrieves all item statuses for the current tenant.
        /// API: GET /api/wms/masters/item-status/GetAll
        /// </summary>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetAllAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves active item statuses for the current tenant.
        /// API: GET /api/wms/masters/item-status/GetActive
        /// </summary>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetActiveAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves a single item status by id.
        /// API: GET /api/wms/masters/item-status/GetById/{id}
        /// </summary>
        /// <param name="id">Item status identifier.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSItemStatusReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// Creates a new item status.
        /// API: POST /api/wms/masters/item-status/Create
        /// </summary>
        /// <param name="entity">Create DTO.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> CreateAsync(WMSItemStatusCreateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Updates an existing item status (route id is authoritative).
        /// API: PUT /api/wms/masters/item-status/Update/{id}
        /// </summary>
        /// <param name="id">Item status identifier (route).</param>
        /// <param name="entity">Update DTO.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> UpdateAsync(int id, WMSItemStatusUpdateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an item status.
        /// API: PATCH /api/wms/masters/item-status/SetActive/{id}?isActive=true|false
        /// </summary>
        /// <param name="id">Item status identifier.</param>
        /// <param name="isActive">True to activate; false to deactivate.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);


        /// <summary>
        /// Deletes an item status (hard delete).
        /// API: DELETE /api/wms/masters/item-status/Delete/{id}
        /// </summary>
        /// <param name="id">Inventory type identifier.</param>
        /// <param name="token">JWT token used for authorization.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> DeleteByIdAsync(int id, string token, CancellationToken ct);
    }
}

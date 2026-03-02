using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.ItemStatus
{
    /// <summary>
    /// UI Service for Item Status (WMS / Masters).
    ///
    /// Calls API routes under: /api/wms/masters/item-status
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER sent from UI.
    /// - API obtains CompanyId from the authenticated token.
    /// </summary>
    public class ItemStatusWMSUIService : UIApiServiceBase, IItemStatusWMSUIService
    {
        /// <summary>
        /// Base API route for Item Status controller.
        /// </summary>
        private const string BasePath = "/api/wms/masters/item-status";

        /// <summary>
        /// Initializes a new instance of <see cref="ItemStatusWMSUIService"/>.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public ItemStatusWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Retrieves all item statuses for the current tenant.
        /// API: GET /api/wms/masters/item-status/GetAll
        /// </summary>
        public Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetAllAsync(string token, CancellationToken ct)
            => GetApiAsync<List<WMSItemStatusReadDTO>>(
                $"{BasePath}/GetAll",
                token,
                ct);

        /// <summary>
        /// Retrieves active item statuses for the current tenant.
        /// API: GET /api/wms/masters/item-status/GetActive
        /// </summary>
        public Task<ApiResponse<List<WMSItemStatusReadDTO>>> GetActiveAsync(string token, CancellationToken ct)
            => GetApiAsync<List<WMSItemStatusReadDTO>>(
                $"{BasePath}/GetActive",
                token,
                ct);

        /// <summary>
        /// Retrieves a single item status by id.
        /// API: GET /api/wms/masters/item-status/GetById/{id}
        /// </summary>
        public Task<ApiResponse<WMSItemStatusReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct)
            => GetApiAsync<WMSItemStatusReadDTO?>(
                $"{BasePath}/GetById/{id}",
                token,
                ct);

        /// <summary>
        /// Creates a new item status.
        /// API: POST /api/wms/masters/item-status/Create
        /// </summary>
        public Task<ApiResponse<bool>> CreateAsync(WMSItemStatusCreateDTO entity, string token, CancellationToken ct)
            => PostApiAsync<bool, WMSItemStatusCreateDTO>(
                $"{BasePath}/Create",
                entity,
                token,
                ct);

        /// <summary>
        /// Updates an existing item status (route id is authoritative).
        /// API: PUT /api/wms/masters/item-status/Update/{id}
        /// </summary>
        public Task<ApiResponse<bool>> UpdateAsync(int id, WMSItemStatusUpdateDTO entity, string token, CancellationToken ct)
            => PutApiAsync<bool, WMSItemStatusUpdateDTO>(
                $"{BasePath}/Update/{id}",
                entity,
                token,
                ct);

        /// <summary>
        /// Activates or deactivates an item status.
        /// API: PATCH /api/wms/masters/item-status/SetActive/{id}?isActive=true|false
        /// </summary>
        public Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct)
            => PatchApiAsync<bool>(
                $"{BasePath}/SetActive/{id}?isActive={isActive.ToString().ToLowerInvariant()}",
                token: token,
                ct: ct);


        /// <summary>
        /// Deletes an inventory type (hard delete).
        /// API: DELETE /api/wms/masters/inventory-types/Delete/{id}
        /// </summary>
        public Task<ApiResponse<bool>> DeleteByIdAsync(int id, string token, CancellationToken ct)
            => DeleteApiAsync<bool>(
                $"{BasePath}/Delete/{id}",
                token,
                ct);
    }
}

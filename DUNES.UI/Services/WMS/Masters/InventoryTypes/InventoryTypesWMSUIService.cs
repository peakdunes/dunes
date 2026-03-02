using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.InventoryTypes
{
    /// <summary>
    /// UI Service for Inventory Types (WMS / Masters).
    ///
    /// Calls API routes under: /api/wms/masters/inventory-types
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is NEVER sent from UI.
    /// - API obtains CompanyId from the authenticated token.
    /// </summary>
    public class InventoryTypesWMSUIService : UIApiServiceBase, IInventoryTypesWMSUIService
    {
        /// <summary>
        /// Base API route for Inventory Types controller.
        /// </summary>
        private const string BasePath = "/api/wms/masters/inventory-types";

        /// <summary>
        /// Initializes a new instance of <see cref="InventoryTypesWMSUIService"/>.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public InventoryTypesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Retrieves all inventory types for the current tenant.
        /// API: GET /api/wms/masters/inventory-types/GetAll
        /// </summary>
        public Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetAllAsync(string token, CancellationToken ct)
            => GetApiAsync<List<WMSInventoryTypesReadDTO>>(
                $"{BasePath}/GetAll",
                token,
                ct);

        /// <summary>
        /// Retrieves active inventory types for the current tenant.
        /// API: GET /api/wms/masters/inventory-types/GetActive
        /// </summary>
        public Task<ApiResponse<List<WMSInventoryTypesReadDTO>>> GetActiveAsync(string token, CancellationToken ct)
            => GetApiAsync<List<WMSInventoryTypesReadDTO>>(
                $"{BasePath}/GetActive",
                token,
                ct);

        /// <summary>
        /// Retrieves a single inventory type by id.
        /// API: GET /api/wms/masters/inventory-types/GetById/{id}
        /// </summary>
        public Task<ApiResponse<WMSInventoryTypesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct)
            => GetApiAsync<WMSInventoryTypesReadDTO?>(
                $"{BasePath}/GetById/{id}",
                token,
                ct);

        /// <summary>
        /// Creates a new inventory type.
        /// API: POST /api/wms/masters/inventory-types/Create
        /// </summary>
        public Task<ApiResponse<bool>> CreateAsync(WMSInventoryTypesCreateDTO entity, string token, CancellationToken ct)
            => PostApiAsync<bool, WMSInventoryTypesCreateDTO>(
                $"{BasePath}/Create",
                entity,
                token,
                ct);

        /// <summary>
        /// Updates an existing inventory type (route id is authoritative).
        /// API: PUT /api/wms/masters/inventory-types/Update/{id}
        /// </summary>
        public Task<ApiResponse<bool>> UpdateAsync(int id, WMSInventoryTypesUpdateDTO entity, string token, CancellationToken ct)
            => PutApiAsync<bool, WMSInventoryTypesUpdateDTO>(
                $"{BasePath}/Update/{id}",
                entity,
                token,
                ct);

        /// <summary>
        /// Activates or deactivates an inventory type.
        /// API: PATCH /api/wms/masters/inventory-types/SetActive/{id}?isActive=true|false
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

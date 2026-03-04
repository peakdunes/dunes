using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.TransactionTypes
{
    public interface ITransactionTypesWMSUIService
    {

        /// <summary>
        /// Retrieves all transaction types for the current tenant.
        /// API: GET /api/wms/masters/transaction-concepts/GetAll
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSTransactiontypesReadDTO>>> GetAllAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves a single transaction types by id for the current tenant.
        /// API: GET /api/wms/masters/transaction-types/GetById/{id}
        /// </summary>
        /// <param name="id">transaciton type Id.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSTransactiontypesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// Creates a new transaction types for the current tenant.
        /// API: POST /api/wms/masters/transaction-types/Create
        /// </summary>
        /// <param name="entity">Create DTO.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> CreateAsync(WMSTransactiontypesCreateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Updates an existing transaction types (route id is authoritative).
        /// API: PUT /api/wms/masters/transaction-types/Update/{id}
        /// </summary>
        /// <param name="id">transaction types Id (route).</param>
        /// <param name="entity">Update DTO.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> UpdateAsync(int id, WMSTransactionTypesUpdateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an inventory category.
        /// API: PATCH /api/wms/masters/transaction-types/SetActive/{id}?isActive=true|false
        /// </summary>
        /// <param name="id">transaction types Id.</param>
        /// <param name="isActive">Activation flag.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);

        /// <summary>
        /// Checks whether a transaction types name exists (optional excludeId).
        /// NOTE: Requires API endpoint ExistsByName to exist.
        /// Recommended API: GET /api/wms/masters/transaction-types/ExistsByName?name=...&excludeId=...
        /// </summary>
        /// <param name="name">transaction types name.</param>
        /// <param name="excludeId">Optional exclude id.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct);

        /// <summary>
        /// Hard deletes an inventory category if not used by any client mapping.
        /// API: DELETE /api/wms/masters/transaction-types/Delete/{id}
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">transaction types Id.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> DeleteByIdAsync(string token, int id, CancellationToken ct);

    }
}

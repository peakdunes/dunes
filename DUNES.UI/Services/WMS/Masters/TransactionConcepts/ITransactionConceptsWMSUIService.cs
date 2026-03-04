using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.TransactionConcepts
{
    public interface ITransactionConceptsWMSUIService
    {

        /// <summary>
        /// Retrieves all transactions concept for the current tenant.
        /// API: GET /api/wms/masters/transaction-concepts/GetAll
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSTransactionconceptsReadDTO>>> GetAllAsync(string token, CancellationToken ct);

        /// <summary>
        /// Retrieves a single transactions concept by id for the current tenant.
        /// API: GET /api/wms/masters/transaction-concepts/GetById/{id}
        /// </summary>
        /// <param name="id">transaction concept Id.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSTransactionconceptsReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// Creates a new transactions concept for the current tenant.
        /// API: POST /api/wms/masters/transaction-concepts/Create
        /// </summary>
        /// <param name="entity">Create DTO.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> CreateAsync(WMSTransactionconceptsCreateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Updates an existing transactions concept (route id is authoritative).
        /// API: PUT /api/wms/masters/transaction-concepts/Update/{id}
        /// </summary>
        /// <param name="id">transactions concept Id (route).</param>
        /// <param name="entity">Update DTO.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> UpdateAsync(int id, WMSTransactionconceptsUpdateDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an inventory category.
        /// API: PATCH /api/wms/masters/transaction-concepts/SetActive/{id}?isActive=true|false
        /// </summary>
        /// <param name="id">transactions concept Id.</param>
        /// <param name="isActive">Activation flag.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);

        /// <summary>
        /// Checks whether a category name exists (optional excludeId).
        /// NOTE: Requires API endpoint ExistsByName to exist.
        /// Recommended API: GET /api/wms/masters/transaction-concepts/ExistsByName?name=...&excludeId=...
        /// </summary>
        /// <param name="name">transactions concept name.</param>
        /// <param name="excludeId">Optional exclude id.</param>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct);

        /// <summary>
        /// Hard deletes an transactions concept if not used by any client mapping.
        /// API: DELETE /api/wms/masters/transaction-concepts/Delete/{id}
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">transactions concept Id.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> DeleteByIdAsync(string token, int id, CancellationToken ct);

    }
}


using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service contract for client-level item status enablement.
    /// Business rules:
    /// - CompanyId and CompanyClientId always come from the authenticated token.
    /// - Master ItemStatus must exist and be active before it can be enabled for a client.
    /// - Mapping uniqueness is enforced by (CompanyId, CompanyClientId, ItemStatusId).
    /// - Delete only removes the client mapping, never the master item status.
    /// </summary>
    public interface ICompanyClientItemStatusWMSAPIService
    {
        /// <summary>
        /// Returns all enabled item status mappings for the current client.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the list of enabled item statuses for the client.</returns>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Returns enabled item status mappings for the current client.
        /// This method exists to preserve the explicit enabled-only use case.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the list of enabled item statuses for the client.</returns>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Returns a specific client mapping by its Id.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the requested mapping if found.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new client mapping for a master item status.
        /// The service must validate:
        /// - the master item status exists and is active
        /// - the mapping does not already exist for the client
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the created mapping.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Enables or disables an existing mapping.
        /// If activating a mapping, the master item status must still be active.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="isActive">New mapping status.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the update succeeded.</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Replaces the enabled set for the client.
        /// Typical UI behavior: the user selects all desired item statuses and saves.
        /// The service must validate:
        /// - all provided master ids exist
        /// - all provided master ids are active
        /// - missing mappings are created when necessary
        /// - existing mappings not included are disabled
        /// </summary>
        /// <param name="itemStatusIds">Final list of enabled master item status ids.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the operation succeeded.</returns>
        Task<ApiResponse<bool>> SetEnabledSetAsync(
            List<int> itemStatusIds,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes a client mapping by Id.
        /// Important: this deletes only the relationship, not the master item status.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the delete succeeded.</returns>
        Task<ApiResponse<object>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);
    }
}
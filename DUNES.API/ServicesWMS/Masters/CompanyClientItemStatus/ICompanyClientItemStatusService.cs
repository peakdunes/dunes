using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service contract for managing CompanyClientItemStatus mappings.
    /// Applies business rules and tenant validation using CompanyId and CompanyClientId from token.
    /// </summary>
    public interface ICompanyClientItemStatusWMSAPIService
    {
        /// <summary>
        /// Gets all ItemStatus mappings for the current tenant scope (company + client),
        /// including active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the list of mappings.</returns>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets an ItemStatus mapping by mapping Id within the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the mapping if found.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new ItemStatus mapping for the current tenant scope.
        /// Business rules:
        /// - Master ItemStatus must exist.
        /// - No duplicate mappings allowed.
        /// - If mapping is active, master ItemStatus must be active.
        /// </summary>
        /// <param name="request">Create DTO (tenant values are not accepted in body).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the created mapping.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing ItemStatus mapping for the current tenant scope.
        /// Business rules:
        /// - Mapping must exist within tenant scope.
        /// - Master ItemStatus must exist.
        /// - No duplicate mappings allowed.
        /// - If mapping is active, master ItemStatus must be active.
        /// </summary>
        /// <param name="request">Update DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the updated mapping.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> UpdateAsync(
            WMSCompanyClientItemStatusUpdateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an existing ItemStatus mapping within the current tenant scope.
        /// If activating the mapping, the master ItemStatus must be active.
        /// </summary>
        /// <param name="request">Set-active request DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the updated mapping.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> SetActiveAsync(
            WMSCompanyClientItemStatusSetActiveDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes an ItemStatus mapping by mapping Id within the current tenant scope.
        /// Intended for wrong assignments (physical delete of mapping only).
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response indicating delete result.</returns>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);
    }
}

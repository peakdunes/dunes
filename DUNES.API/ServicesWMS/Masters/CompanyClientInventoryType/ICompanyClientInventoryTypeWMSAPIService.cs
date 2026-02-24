using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// Service contract for managing CompanyClientInventoryType mappings.
    /// Applies business rules and tenant validation using CompanyId and CompanyClientId from token.
    /// </summary>
    public interface ICompanyClientInventoryTypeWMSAPIService
    {
        /// <summary>
        /// Gets all InventoryType mappings for the current tenant scope (company + client),
        /// including active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Standard API response containing the list of mapped inventory types.
        /// </returns>
        Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a single InventoryType mapping by mapping Id within the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Standard API response containing the mapped InventoryType if found.
        /// </returns>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new InventoryType mapping for the current tenant scope.
        /// Business rules include:
        /// - Master InventoryType must exist.
        /// - Duplicate mappings are not allowed.
        /// - If mapping is enabled, master InventoryType must be active.
        /// </summary>
        /// <param name="request">Create DTO (tenant values are NOT included).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Standard API response containing the created mapping.
        /// </returns>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing InventoryType mapping for the current tenant scope.
        /// Business rules include:
        /// - Mapping must exist in tenant scope.
        /// - Master InventoryType must exist.
        /// - Duplicate mappings are not allowed.
        /// - If mapping is enabled, master InventoryType must be active.
        /// </summary>
        /// <param name="request">Update DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Standard API response containing the updated mapping.
        /// </returns>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes an existing InventoryType mapping by mapping Id within the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Standard API response indicating whether the delete operation succeeded.
        /// </returns>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);


        /// <summary>
        /// Changes the active status of an existing InventoryType mapping within the current tenant scope.
        /// If activating the mapping, the referenced master InventoryType must be active.
        /// </summary>
        /// <param name="request">Set-active request DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Standard API response with the updated mapping.</returns>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> SetActiveAsync(
            WMSCompanyClientInventoryTypeSetActiveDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct);
    }
}

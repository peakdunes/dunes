using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// UI service contract for client-specific Item Status configuration.
    /// </summary>
    public interface ICompanyClientItemStatusWMSUIService
    {
        /// <summary>
        /// Gets all enabled Item Status mappings for the current client.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the client item status list.</returns>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets explicitly enabled Item Status mappings for the current client.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the enabled client item status list.</returns>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets a client Item Status mapping by Id.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the requested mapping.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            string token,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new client Item Status mapping.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="dto">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the created mapping.</returns>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            string token,
            WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Updates the active flag of an existing client Item Status mapping.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">DTO containing the new IsActive value.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response indicating whether the update succeeded.</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            string token,
            int id,
            WMSCompanyClientItemStatusSetActiveDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Replaces the enabled set for the current client.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="dto">DTO containing the final list of enabled master item status ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response indicating whether the bulk operation succeeded.</returns>
        Task<ApiResponse<bool>> SetEnabledSetAsync(
            string token,
            WMSCompanyClientItemStatusSetEnabledDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Deletes a client Item Status mapping by Id.
        /// This removes only the relationship, not the master Item Status.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response indicating whether the delete succeeded.</returns>
        Task<ApiResponse<object>> DeleteAsync(
            string token,
            int id,
            CancellationToken ct);
    }
}
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// UI service implementation for client-specific Item Status configuration.
    /// </summary>
    public class CompanyClientItemStatusWMSUIService
        : UIApiServiceBase, ICompanyClientItemStatusWMSUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public CompanyClientItemStatusWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Gets all enabled Item Status mappings for the current client.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the client item status list.</returns>
        public Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientItemStatusReadDTO>>(
                "/api/wms/masters/company-client/item-status/GetAll",
                token,
                ct);

        /// <summary>
        /// Gets explicitly enabled Item Status mappings for the current client.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the enabled client item status list.</returns>
        public Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientItemStatusReadDTO>>(
                "/api/wms/masters/company-client/item-status/GetEnabled",
                token,
                ct);

        /// <summary>
        /// Gets a client Item Status mapping by Id.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the requested mapping.</returns>
        public Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            string token,
            int id,
            CancellationToken ct)
            => GetApiAsync<WMSCompanyClientItemStatusReadDTO>(
                $"/api/wms/masters/company-client/item-status/GetById/{id}",
                token,
                ct);

        /// <summary>
        /// Creates a new client Item Status mapping.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="dto">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response containing the created mapping.</returns>
        public Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            string token,
            WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct)
            => PostApiAsync<WMSCompanyClientItemStatusReadDTO, WMSCompanyClientItemStatusCreateDTO>(
                "/api/wms/masters/company-client/item-status/Create",
                dto,
                token,
                ct);

        /// <summary>
        /// Updates the active flag of an existing client Item Status mapping.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="dto">DTO containing the new IsActive value.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response indicating whether the update succeeded.</returns>
        public Task<ApiResponse<bool>> SetActiveAsync(
            string token,
            int id,
            WMSCompanyClientItemStatusSetActiveDTO dto,
            CancellationToken ct)
            => PutApiAsync<bool, WMSCompanyClientItemStatusSetActiveDTO>(
                $"/api/wms/masters/company-client/item-status/SetActive/{id}",
                dto,
                token,
                ct);

        /// <summary>
        /// Replaces the enabled set for the current client.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="dto">DTO containing the final list of enabled master item status ids.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response indicating whether the bulk operation succeeded.</returns>
        public Task<ApiResponse<bool>> SetEnabledSetAsync(
            string token,
            WMSCompanyClientItemStatusSetEnabledDTO dto,
            CancellationToken ct)
            => PutApiAsync<bool, WMSCompanyClientItemStatusSetEnabledDTO>(
                "/api/wms/masters/company-client/item-status/SetEnabledSet",
                dto,
                token,
                ct);

        /// <summary>
        /// Deletes a client Item Status mapping by Id.
        /// This removes only the relationship, not the master Item Status.
        /// </summary>
        /// <param name="token">JWT token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Api response indicating whether the delete succeeded.</returns>
        public Task<ApiResponse<object>> DeleteAsync(
            string token,
            int id,
            CancellationToken ct)
            => DeleteApiAsync<object>(
                $"/api/wms/masters/company-client/item-status/Delete/{id}",
                token,
                ct);
    }
}
using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Items
{
    /// <summary>
    /// Service contract for managing Items within WMS.
    /// Applies tenant scope, ownership mode rules, validation,
    /// and business constraints on top of the repository layer.
    /// </summary>
    public interface IItemsWMSAPIService
    {
        /// <summary>
        /// Returns all items available for the current tenant scope
        /// according to the configured ownership mode.
        /// </summary>
        /// <param name="companyId">Company identifier from token.</param>
        /// <param name="companyClientId">Company client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of items visible to the current client context.</returns>
        Task<ApiResponse<List<WMSItemsReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Returns a specific item by Id within the current tenant scope
        /// according to the configured ownership mode.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token.</param>
        /// <param name="companyClientId">Company client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested item if found.</returns>
        Task<ApiResponse<WMSItemsReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new item according to the ownership mode and tenant rules.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="companyId">Company identifier from token.</param>
        /// <param name="companyClientId">Company client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created item.</returns>
        Task<ApiResponse<WMSItemsReadDTO>> CreateAsync(
            WMSItemsCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing item within the current tenant scope.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="companyId">Company identifier from token.</param>
        /// <param name="companyClientId">Company client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated item.</returns>
        Task<ApiResponse<WMSItemsReadDTO>> UpdateAsync(
            int id,
            WMSItemsUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates the active status of an item within the current tenant scope.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="isActive">New active state.</param>
        /// <param name="companyId">Company identifier from token.</param>
        /// <param name="companyClientId">Company client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the active status update.</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes an item within the current tenant scope.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token.</param>
        /// <param name="companyClientId">Company client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>Result of the delete operation.</returns>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);
    }
}

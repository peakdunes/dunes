using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// UI service interface for client-specific Item Status configuration.
    /// </summary>
    public interface ICompanyClientItemStatusWMSUIService
    {
        /// <summary>
        /// Gets all item statuses enabled for a client.
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets a specific item status mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Creates a new item status mapping for the client.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing item status mapping.
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            WMSCompanyClientItemStatusUpdateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an item status mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct);
    }
}

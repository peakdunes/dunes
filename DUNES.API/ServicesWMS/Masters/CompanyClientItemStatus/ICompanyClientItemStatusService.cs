using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service interface for managing item statuses available per client.
    /// Handles business logic, ownership validation, and response formatting.
    /// </summary>
    public interface ICompanyClientItemStatusService
    {
        /// <summary>
        /// Get all item statuses configured for the current client.
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Get a specific item status mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new item status mapping for the client.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update the active state of a client item status mapping.
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientItemStatusUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Set active/inactive state.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

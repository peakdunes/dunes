using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// UI service interface for client-specific Inventory Types configuration.
    /// </summary>
    public interface ICompanyClientInventoryTypeWMSUIService
    {
        /// <summary>
        /// Gets all inventory types enabled for a client.
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets a specific client inventory type mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Creates a new inventory type mapping for the client.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing inventory type mapping.
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a client inventory type mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct);
    }
}

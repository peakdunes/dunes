using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// UI service interface for managing inventory category mappings per client.
    /// </summary>
    public interface ICompanyClientInventoryCategoryWMSUIService
    {
        /// <summary>
        /// Get all inventory category mappings for a client.
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetAllAsync(
            int companyClientId,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Get a specific inventory category mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Create a new inventory category mapping for a client.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Update an existing inventory category mapping.
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            WMSCompanyClientInventoryCategoryUpdateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a specific mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct);
    }
}

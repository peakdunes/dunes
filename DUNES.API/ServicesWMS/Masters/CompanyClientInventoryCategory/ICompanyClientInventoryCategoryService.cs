using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Service interface for managing client-level inventory category mappings.
    /// Scoped strictly by CompanyId and CompanyClientId.
    /// </summary>
    public interface ICompanyClientInventoryCategoryService
    {
        /// <summary>
        /// Get all active inventory categories for a given client.
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Get a specific inventory category mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new client-category mapping.
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update an existing client-category mapping.
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryCategoryUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a client-category mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories Service
    /// Scoped by Company
    /// </summary>
    public interface IInventoryCategoriesWMSAPIService
    {
        /// <summary>
        /// Get all inventory categories by company
        /// </summary>
        Task<ApiResponse<List<WMSInventoryCategoriesDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get inventory category by id
        /// </summary>
        Task<ApiResponse<WMSInventoryCategoriesDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create inventory category
        /// </summary>
        Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSInventoryCategoriesDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update inventory category
        /// </summary>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSInventoryCategoriesDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate / Deactivate inventory category
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

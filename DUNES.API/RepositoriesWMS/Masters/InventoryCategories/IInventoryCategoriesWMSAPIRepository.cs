
using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using System.Threading;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories Repository
    /// 
    /// Scoped by:
    /// Company (tenant)
    /// 
    /// IMPORTANT:
    /// This repository is the last line of defense for multi-tenant security.
    /// All operations MUST be filtered by CompanyId.
    /// </summary>
    public interface IInventoryCategoriesWMSAPIRepository
    {
        /// <summary>
        /// Get all inventory categories for a company.
        /// </summary>
        Task<List<WMSInventorycategoriesReadDTO>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active inventory categories for a company.
        /// </summary>
        Task<List<WMSInventorycategoriesReadDTO>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get inventory category by id validating ownership.
        /// </summary>
        Task<WMSInventorycategoriesReadDTO?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if an inventory category name already exists for a company.
        /// </summary>
        Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new inventory category.
        /// </summary>
        Task<WMSInventorycategoriesReadDTO> CreateAsync(
            WMSInventorycategoriesCreateDTO dto,
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Update an existing inventory category.
        /// </summary>
        Task<WMSInventorycategoriesReadDTO> UpdateAsync(
            WMSInventorycategoriesUpdateDTO dto,
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate an inventory category.
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}



using DUNES.API.ModelsWMS.Masters;

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
        /// <param name="companyId">Company identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of inventory categories</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Inventorycategories>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active inventory categories for a company.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active inventory categories</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Inventorycategories>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get inventory category by id validating ownership.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Category identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Inventory category or null if not found</returns>
        Task<DUNES.API.ModelsWMS.Masters.Inventorycategories?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if an inventory category name already exists for a company.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="name">Category name</param>
        /// <param name="excludeId">Optional category id to exclude (used on update)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new inventory category.
        /// 
        /// IMPORTANT:
        /// - Entity must already contain CompanyId
        /// - Repository must NOT infer or override ownership
        /// </summary>
        /// <param name="entity">Inventory category entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.Inventorycategories> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Inventorycategories entity,
            CancellationToken ct);

        /// <summary>
        /// Update an existing inventory category.
        /// 
        /// IMPORTANT:
        /// - Ownership (CompanyId) must NOT be changed here
        /// </summary>
        /// <param name="entity">Inventory category entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.Inventorycategories> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Inventorycategories entity,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate an inventory category.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Category identifier</param>
        /// <param name="isActive">Active flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}


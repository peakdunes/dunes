namespace DUNES.API.RepositoriesWMS.Masters.Items
{
    /// <summary>
    /// Items Repository
    /// Data access layer for inventory items.
    /// Scoped by Company (STANDARD COMPANYID).
    /// </summary>
    public interface IItemsWMSAPIRepository
    {
        /// <summary>
        /// Get all items for a company (CompanyClientId = null)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of items</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Items>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active items for a company (CompanyClientId = null)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active items</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Items>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get item by id (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Item identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Item entity or null</returns>
        Task<DUNES.API.ModelsWMS.Masters.Items?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if an item exists by SKU (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="sku">SKU value</param>
        /// <param name="excludeId">Optional item id to exclude</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if SKU exists</returns>
        Task<bool> ExistsBySkuAsync(
            int companyId,
            string sku,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Check if an item exists by Barcode (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="barcode">Barcode value</param>
        /// <param name="excludeId">Optional item id to exclude</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if barcode exists</returns>
        Task<bool> ExistsByBarcodeAsync(
            int companyId,
            string barcode,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new item
        /// </summary>
        /// <param name="entity">Item entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created item</returns>
        Task<DUNES.API.ModelsWMS.Masters.Items> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct);

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="entity">Item entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated item</returns>
        Task<DUNES.API.ModelsWMS.Masters.Items> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate an item
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="id">Item identifier</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if operation succeeded</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Items
{
    /// <summary>
    /// Items Service
    /// Business logic layer for company-owned inventory items.
    /// Scoped strictly by Company (STANDARD COMPANYID).
    /// </summary>
    public interface IItemsWMSAPIService
    {
        /// <summary>
        /// Get all items for a company
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of items</returns>
        Task<ApiResponse<List<WMSItemsDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active items for a company
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active items</returns>
        Task<ApiResponse<List<WMSItemsDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get item by id (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Item identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Item data</returns>
        Task<ApiResponse<WMSItemsDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new item
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="dto">Item DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSItemsDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Item identifier</param>
        /// <param name="dto">Item DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSItemsDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate an item
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Item identifier</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Validate if an item exists with the same SKU (scoped)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="sku">SKU value</param>
        /// <param name="excludeId">Optional item id to exclude</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if SKU exists</returns>
        Task<ApiResponse<bool>> ExistsBySkuAsync(
            int companyId,
            string sku,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Validate if an item exists with the same Barcode (scoped)
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="barcode">Barcode value</param>
        /// <param name="excludeId">Optional item id to exclude</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if barcode exists</returns>
        Task<ApiResponse<bool>> ExistsByBarcodeAsync(
            int companyId,
            string barcode,
            int? excludeId,
            CancellationToken ct);
    }
}

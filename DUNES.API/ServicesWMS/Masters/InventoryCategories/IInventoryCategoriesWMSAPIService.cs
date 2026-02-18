using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories Service
    /// Scoped by company (tenant). All methods enforce tenant filtering.
    /// </summary>
    public interface IInventoryCategoriesWMSAPIService
    {
        /// <summary>
        /// Retrieves all inventory categories for a given company.
        /// </summary>
        /// <param name="companyId">The company (tenant) identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of inventory category read DTOs wrapped in ApiResponse</returns>
        Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves a single inventory category by its ID.
        /// </summary>
        /// <param name="companyId">The company (tenant) identifier</param>
        /// <param name="id">The category ID to retrieve</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>The matching inventory category or 404 if not found</returns>
        Task<ApiResponse<WMSInventorycategoriesReadDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);


        /// <summary>
        /// check if a category already exist in our system searching by name 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct);
        


            /// <summary>
            /// Creates a new inventory category for the given company.
            /// </summary>
            /// <param name="companyId">The company (tenant) identifier</param>
            /// <param name="dto">Create DTO with inventory category data</param>
            /// <param name="ct">Cancellation token</param>
            /// <returns>ApiResponse with success flag</returns>
            Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSInventorycategoriesCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing inventory category.
        /// </summary>
        /// <param name="companyId">The company (tenant) identifier</param>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="dto">Update DTO with new values</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>ApiResponse with success flag</returns>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSInventorycategoriesUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an inventory category.
        /// </summary>
        /// <param name="companyId">The company (tenant) identifier</param>
        /// <param name="id">The category ID</param>
        /// <param name="isActive">True to activate, false to deactivate</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>ApiResponse indicating whether the update was successful</returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

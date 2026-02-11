using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Repository interface for managing client-level inventory category mappings.
    /// All methods are strictly scoped by CompanyId and CompanyClientId.
    /// </summary>
    public interface ICompanyClientInventoryCategoryWMSAPIRepository
    {
        /// <summary>
        /// Get all active inventory categories assigned to a specific client.
        /// </summary>
        /// <param name="companyId">Parent company ID (from token)</param>
        /// <param name="companyClientId">Client ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of read DTOs</returns>
        Task<List<WMSCompanyClientInventoryCategoryReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Get a specific mapping by ID, validating ownership.
        /// </summary>
        /// <param name="companyId">Parent company ID</param>
        /// <param name="companyClientId">Client ID</param>
        /// <param name="id">Mapping ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Read DTO or null if not found</returns>
        Task<WMSCompanyClientInventoryCategoryReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new mapping between a client and a master inventory category.
        /// </summary>
        /// <param name="dto">Create DTO</param>
        /// <param name="companyId">Parent company ID</param>
        /// <param name="companyClientId">Client ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created read DTO</returns>
        Task<WMSCompanyClientInventoryCategoryReadDTO> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Update an existing client-category mapping.
        /// </summary>
        /// <param name="dto">Update DTO</param>
        /// <param name="companyId">Parent company ID</param>
        /// <param name="companyClientId">Client ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if update succeeded</returns>
        Task<bool> UpdateAsync(
            WMSCompanyClientInventoryCategoryUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a specific mapping.
        /// </summary>
        /// <param name="companyId">Parent company ID</param>
        /// <param name="companyClientId">Client ID</param>
        /// <param name="id">Mapping ID</param>
        /// <param name="isActive">New active status</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if status was changed</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Check if a client already has a mapping to a specific master inventory category.
        /// </summary>
        /// <param name="companyId">Parent company ID</param>
        /// <param name="companyClientId">Client ID</param>
        /// <param name="inventoryCategoryId">Inventory category to validate</param>
        /// <param name="excludeId">Optional ID to exclude for updates</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if mapping exists</returns>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryCategoryId,
            int? excludeId,
            CancellationToken ct);
    }
}

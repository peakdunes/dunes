using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Repository contract for client-level inventory category enablement.
    /// Anti-error design principles:
    /// - CompanyId and CompanyClientId are ALWAYS taken from the token (never from body/query).
    /// - Mapping records are unique by (CompanyId, CompanyClientId, InventoryCategoryId).
    /// - Master catalog must be IsActive=true to allow enabling or to appear in "enabled" results.
    /// - No "Update" method is provided to avoid changing InventoryCategoryId by mistake.
    ///   Changes must be performed via SetActive or SetEnabledSet (replace list).
    /// </summary>
    public interface ICompanyClientInventoryCategoryWMSAPIRepository
    {

        /// <summary>
        /// Returns all categories for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive=true AND
        /// - Master InventoryCategory IsActive=true
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<List<WMSCompanyClientInventoryCategoryReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);


        /// <summary>
        /// Returns the enabled categories for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive=true AND
        /// - Master InventoryCategory IsActive=true
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<List<WMSCompanyClientInventoryCategoryReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Returns a specific mapping by Id, scoped by CompanyId + CompanyClientId.
        /// Recommended behavior: return null if not found.
        /// (If you want "enabled-only" behavior, enforce it consistently here too.)
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="id">Mapping identity.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<WMSCompanyClientInventoryCategoryReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new mapping for the client and a master inventory category.
        /// Service MUST validate:
        /// - Master category exists and IsActive=true
        /// - Mapping does not already exist (unique constraint also protects)
        /// </summary>
        /// <param name="dto">Create DTO (should only include InventoryCategoryId and optional IsActive).</param>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<WMSCompanyClientInventoryCategoryReadDTO> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Enables/disables a mapping by mapping Id.
        /// CRITICAL RULE: activation (IsActive=true) MUST be rejected if master category IsActive=false.
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="id">Mapping identity.</param>
        /// <param name="isActive">New mapping status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the mapping was found and updated.</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Replaces the enabled set for the client (anti-error bulk operation).
        /// Typical UI behavior: user selects categories and clicks Save.
        /// Implementation should:
        /// - Validate all IDs exist and are master-active (Service)
        /// - Upsert missing mappings (create if needed)
        /// - Set IsActive=true for IDs in the provided list
        /// - Set IsActive=false for existing mappings not in the provided list
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="inventoryCategoryIds">Final list of enabled master category IDs.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if operation succeeded.</returns>
        Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryCategoryIds,
            CancellationToken ct);

        /// <summary>
        /// Checks if a mapping exists for the given client and master category.
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="inventoryCategoryId">Master inventory category identity.</param>
        /// <param name="excludeId">Optional mapping identity to exclude (rarely needed if Update is removed).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryCategoryId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Validates that the master inventory category exists and is active.
        /// Supports the business rule: master inactive => cannot be enabled for a client.
        /// </summary>
        /// <param name="companyId">Company scope (from token) if master is tenant-scoped.</param>
        /// <param name="inventoryCategoryId">Master inventory category identity.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<bool> IsMasterActiveAsync(
            int companyId,
            int inventoryCategoryId,
            CancellationToken ct);
    }
}

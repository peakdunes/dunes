using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository contract for client-level item status enablement.
    /// Anti-error principles:
    /// - CompanyId and CompanyClientId are always taken from the token (never from body/query).
    /// - No Update method is exposed to avoid changing ItemStatusId by mistake.
    /// - Master catalog must be IsActive=true to allow enabling and to appear in enabled lists.
    /// </summary>
    public interface ICompanyClientItemStatusWMSAPIRepository
    {
        /// <summary>
        /// Returns only the item statuses enabled for the current client:
        /// - Mapping IsActive=true AND
        /// - Master ItemStatus IsActive=true
        /// </summary>
        Task<List<WMSCompanyClientItemStatusReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a mapping by Id (scoped by CompanyId + CompanyClientId).
        /// Recommended behavior: if master is inactive, treat as not-enabled (return null).
        /// </summary>
        Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new mapping between the current client and a master item status.
        /// Service must enforce:
        /// - Master status exists and IsActive=true
        /// - Uniqueness (CompanyId, CompanyClientId, ItemStatusId)
        /// </summary>
        Task<WMSCompanyClientItemStatusReadDTO> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a mapping by mapping Id.
        /// CRITICAL: activation must be rejected if master item status IsActive=false.
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Replaces the enabled set for the client (bulk, anti-error).
        /// Implementation should:
        /// - Validate all IDs exist and are master-active
        /// - Enable/create mappings for provided IDs
        /// - Disable mappings not included in the provided list
        /// </summary>
        Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> itemStatusIds,
            CancellationToken ct);

        /// <summary>
        /// Checks if a mapping exists for a given client and master item status.
        /// </summary>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Validates that the master item status exists and is active.
        /// Supports the business rule: master inactive => cannot be enabled for a client.
        /// </summary>
        Task<bool> IsMasterActiveAsync(
            int companyId,
            int itemStatusId,
            CancellationToken ct);
    }
}

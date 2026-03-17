using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository contract for client-level item status enablement.
    /// Anti-error design principles:
    /// - CompanyId and CompanyClientId are ALWAYS taken from the token (never from body/query).
    /// - Mapping records are unique by (CompanyId, CompanyClientId, ItemStatusId).
    /// - Master catalog must be IsActive=true to allow enabling or to appear in "enabled" results.
    /// - No "Update" method is provided to avoid changing ItemStatusId by mistake.
    ///   Changes must be performed via SetActive or SetEnabledSet (replace list).
    /// </summary>
    public interface ICompanyClientItemStatusWMSAPIRepository
    {

        /// <summary>
        /// Returns all item statuses for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive=true AND
        /// - Master ItemStatus IsActive=true
        /// </summary>
        Task<List<WMSCompanyClientItemStatusReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);


        /// <summary>
        /// Returns the enabled item statuses for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive=true AND
        /// - Master ItemStatus IsActive=true
        /// </summary>
        Task<List<WMSCompanyClientItemStatusReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);


        /// <summary>
        /// Returns a specific mapping by Id scoped by CompanyId + CompanyClientId.
        /// </summary>
        Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);


        /// <summary>
        /// Creates a new mapping for the client and a master item status.
        /// Service MUST validate:
        /// - Master item status exists and IsActive=true
        /// - Mapping does not already exist
        /// </summary>
        Task<WMSCompanyClientItemStatusReadDTO> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);


        /// <summary>
        /// Enables/disables a mapping by mapping Id.
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);


        /// <summary>
        /// Replaces the enabled set for the client.
        /// </summary>
        Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> itemStatusIds,
            CancellationToken ct);


        /// <summary>
        /// Checks if mapping already exists.
        /// </summary>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct);


        /// <summary>
        /// Validates master item status is active.
        /// </summary>
        Task<bool> IsMasterActiveAsync(
            int companyId,
            int itemStatusId,
            CancellationToken ct);


        /// <summary>
        /// Checks if any client mapping exists before deleting master status.
        /// </summary>
        Task<bool> HasAnyClientMappingAsync(
            int companyId,
            int itemStatusId,
            CancellationToken ct);


        /// <summary>
        /// Deletes the mapping relation (does not delete master).
        /// </summary>
        Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);
    }
}
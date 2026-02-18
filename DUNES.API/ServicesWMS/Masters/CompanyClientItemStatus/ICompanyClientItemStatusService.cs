using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service contract for client-level item status enablement.
    /// Anti-error principles:
    /// - CompanyId and CompanyClientId are always taken from the token (never from body/query).
    /// - No Update method is exposed to avoid changing ItemStatusId accidentally.
    /// - Master catalog must be IsActive=true to allow enabling and to appear in enabled lists.
    /// </summary>
    public interface ICompanyClientItemStatusService
    {
        /// <summary>
        /// Returns only the item statuses enabled for the current client:
        /// - Mapping IsActive=true AND
        /// - Master ItemStatus IsActive=true
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Get a specific mapping by Id (scoped by CompanyId + CompanyClientId).
        /// Recommended behavior: if master is inactive, treat as not-enabled (NotFound).
        /// </summary>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new mapping between the current client and a master item status.
        /// Rules:
        /// - Master must exist and be active
        /// - No duplicates (CompanyId, CompanyClientId, ItemStatusId)
        /// </summary>
        Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientItemStatusCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a mapping by mapping Id.
        /// CRITICAL: activation must be rejected if master item status IsActive=false.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Replace the enabled set for the client (bulk, anti-error).
        /// Body: list of master ItemStatusIds that should be enabled.
        /// </summary>
        Task<ApiResponse<bool>> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> itemStatusIds,
            CancellationToken ct);
    }
}

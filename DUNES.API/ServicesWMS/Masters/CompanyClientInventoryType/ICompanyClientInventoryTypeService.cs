using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// Service contract for client-level inventory type enablement.
    /// Anti-error principles:
    /// - CompanyId and CompanyClientId are always taken from the token (never from body/query).
    /// - No Update method is exposed to avoid changing InventoryTypeId by mistake.
    /// - Master catalog must be IsActive=true to allow enabling and to appear in enabled lists.
    /// </summary>
    public interface ICompanyClientInventoryTypeService
    {
        /// <summary>
        /// Returns only the inventory types enabled for the current client:
        /// - Mapping IsActive=true AND
        /// - Master InventoryType IsActive=true
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a specific mapping by Id (scoped by CompanyId + CompanyClientId).
        /// Recommended behavior: if master is inactive, treat as not-enabled (return NotFound).
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new mapping between the current client and a master inventory type.
        /// Service must enforce:
        /// - Master type exists and IsActive=true
        /// - Uniqueness (CompanyId, CompanyClientId, InventoryTypeId)
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryTypeCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a mapping.
        /// CRITICAL: activation must be rejected if master inventory type IsActive=false.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Replaces the enabled set for the client (bulk, anti-error).
        /// Body should be the final list of enabled master InventoryTypeIds.
        /// Implementation should:
        /// - Validate all IDs exist and are master-active
        /// - Enable/create mappings for provided IDs
        /// - Disable mappings not included in the provided list
        /// </summary>
        Task<ApiResponse<bool>> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryTypeIds,
            CancellationToken ct);
    }
}

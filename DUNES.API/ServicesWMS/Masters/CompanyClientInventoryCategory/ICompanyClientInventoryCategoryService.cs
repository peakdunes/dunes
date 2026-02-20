using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Service contract for client-level inventory category enablement.
    /// Anti-error principles:
    /// - CompanyId and CompanyClientId are always taken from the token (never from body/query).
    /// - No Update method is exposed to avoid changing InventoryCategoryId by mistake.
    /// - Master catalog must be IsActive=true to allow enabling and to appear in enabled lists.
    /// </summary>
    public interface ICompanyClientInventoryCategoryService
    {
        /// <summary>
        /// Returns all categories for the current client:
        /// - Master InventoryCategory IsActive=true
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);


        /// <summary>
        /// Returns only the categories enabled for the current client:
        /// - Mapping IsActive=true AND
        /// - Master InventoryCategory IsActive=true
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a specific mapping by Id (scoped by CompanyId + CompanyClientId).
        /// Recommended behavior: if master is inactive, treat as not-enabled (return NotFound).
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="id">Mapping identity.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new mapping between the current client and a master inventory category.
        /// Service must enforce:
        /// - Master category exists and IsActive=true
        /// - Mapping uniqueness (CompanyId, CompanyClientId, InventoryCategoryId)
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="dto">Create DTO.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> CreateAsync(
            int companyId,
            int companyClientId,
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a mapping.
        /// CRITICAL: activation must be rejected if master category IsActive=false.
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="id">Mapping identity.</param>
        /// <param name="isActive">New mapping status.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Replaces the enabled set for the client (bulk, anti-error).
        /// Typical usage: UI sends final list of enabled InventoryCategoryIds.
        /// Implementation should:
        /// - Validate all IDs exist and are master-active
        /// - Enable/create mappings for provided IDs
        /// - Disable mappings not included in the provided list
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="inventoryCategoryIds">Final list of enabled master category IDs.</param>
        /// <param name="ct">Cancellation token.</param>
        Task<ApiResponse<bool>> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryCategoryIds,
            CancellationToken ct);
    }
}

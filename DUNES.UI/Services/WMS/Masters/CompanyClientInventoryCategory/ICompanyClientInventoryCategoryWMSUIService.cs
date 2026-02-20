using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// UI service interface for managing inventory category enablement for the current client.
    /// CompanyId / CompanyClientId are resolved from the token by the API.
    /// </summary>
    public interface ICompanyClientInventoryCategoryWMSUIService
    {

        /// <summary>
        /// Get all inventory categories for the current client.
        /// Returns only:
        /// - mapping IsActive=true AND
        /// - master catalog IsActive=true
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct);


        /// <summary>
        /// Get enabled inventory categories for the current client.
        /// Returns only:
        /// - mapping IsActive=true AND
        /// - master catalog IsActive=true
        /// </summary>
        Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Get a specific mapping by Id (scoped by token).
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Create a new mapping for the current client (scoped by token).
        /// </summary>
        Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a mapping by mapping Id (scoped by token).
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Replace the enabled set for the current client (bulk).
        /// Body: list of master InventoryCategoryIds to enable.
        /// </summary>
        Task<ApiResponse<bool>> SetEnabledSetAsync(
            List<int> inventoryCategoryIds,
            string token,
            CancellationToken ct);
    }
}

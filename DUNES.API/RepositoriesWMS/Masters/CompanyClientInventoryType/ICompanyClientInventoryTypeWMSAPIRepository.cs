using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// Repository for managing inventory types assigned to a client.
    /// Scoped by CompanyId and CompanyClientId.
    /// </summary>
    public interface ICompanyClientInventoryTypeWMSAPIRepository
    {
        /// <summary>
        /// Get all inventory types assigned to a client.
        /// </summary>
        Task<List<WMSCompanyClientInventoryTypeReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Get a specific client-inventory type mapping by ID.
        /// </summary>
        Task<WMSCompanyClientInventoryTypeReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new mapping between a client and an inventory type.
        /// </summary>
        Task<WMSCompanyClientInventoryTypeReadDTO> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Update the active state of an existing mapping.
        /// </summary>
        Task<bool> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate an inventory type for the client.
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Check if a mapping already exists for a given inventory type.
        /// </summary>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryTypeId,
            int? excludeId,
            CancellationToken ct);
    }
}

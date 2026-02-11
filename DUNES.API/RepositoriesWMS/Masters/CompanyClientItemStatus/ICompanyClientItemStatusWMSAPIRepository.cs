using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository for managing item status mappings per client (CompaniesContract).
    /// All queries must be scoped by CompanyId and CompanyClientId.
    /// </summary>
    public interface ICompanyClientItemStatusWMSAPIRepository
    {
        /// <summary>
        /// Get all item status mappings assigned to the client.
        /// </summary>
        Task<List<WMSCompanyClientItemStatusReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Get a specific item status mapping by ID.
        /// </summary>
        Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if a mapping already exists (excluding a specific ID if provided).
        /// </summary>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new item status mapping for the client.
        /// </summary>
        Task<WMSCompanyClientItemStatusReadDTO> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Update the IsActive flag of an existing mapping.
        /// </summary>
        Task<bool> UpdateAsync(
            WMSCompanyClientItemStatusUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a specific mapping.
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

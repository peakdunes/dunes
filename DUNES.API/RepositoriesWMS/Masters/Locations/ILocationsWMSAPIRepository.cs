using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.Locations
{
    /// <summary>
    /// Locations Repository
    /// 
    /// Scoped by Company (STANDARD COMPANYID)
    /// 
    /// Notes:
    /// - Query methods return WMSLocationsDTO (read-optimized projections)
    /// - Command methods work with entity and expect CompanyId already set
    /// </summary>
    public interface ILocationsWMSAPIRepository
    {
        /// <summary>
        /// Get all locations for a company
        /// (Query DTO projection)
        /// </summary>
        Task<List<WMSLocationsReadDTO>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active locations for a company
        /// (Query DTO projection)
        /// </summary>
        Task<List<WMSLocationsReadDTO>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get location by id validating ownership
        /// (Query DTO projection)
        /// </summary>
        Task<WMSLocationsReadDTO?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if a location name already exists for a company
        /// </summary>
        Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new location
        /// 
        /// IMPORTANT:
        /// - Entity must already contain CompanyId
        /// - Repository must NOT infer ownership
        /// </summary>
        Task<ModelsWMS.Masters.Locations> CreateAsync(
            ModelsWMS.Masters.Locations entity,
            CancellationToken ct);

        /// <summary>
        /// Update an existing location
        /// 
        /// IMPORTANT:
        /// - CompanyId must NOT be changed here
        /// </summary>
        Task<ModelsWMS.Masters.Locations> UpdateAsync(
            ModelsWMS.Masters.Locations entity,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a location
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

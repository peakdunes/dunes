namespace DUNES.API.RepositoriesWMS.Masters.Bins
{
    /// <summary>
    /// Bins Repository
    /// Scoped by Company + Location + Rack
    /// </summary>
    public interface IBinsWMSAPIRepository
    {
        /// <summary>
        /// Get all bins by company, location and rack
        /// </summary>
        Task<List<DUNES.API.ModelsWMS.Masters.Bines>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get all active bins by company, location and rack
        /// </summary>
        Task<List<DUNES.API.ModelsWMS.Masters.Bines>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get bin by id
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.Bines?> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if a bin exists by name (scoped)
        /// </summary>
        Task<bool> ExistsByNameAsync(
            int companyId,
            int locationId,
            int rackId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Add new bin
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.Bines> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Bines entity,
            CancellationToken ct);

        /// <summary>
        /// Update bin
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.Bines> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Bines entity,
            CancellationToken ct);

        /// <summary>
        /// Activate / Deactivate bin
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

namespace DUNES.API.RepositoriesWMS.Masters.Bins
{
    /// <summary>
    /// Bins Repository
    /// 
    /// Scoped by:
    /// Company (tenant) + Location + Rack
    /// 
    /// IMPORTANT:
    /// This repository is the last line of defense for multi-tenant security.
    /// All read/update operations MUST be filtered by CompanyId, LocationId and RackId.
    /// </summary>
    public interface IBinsWMSAPIRepository
    {
        /// <summary>
        /// Get all bins for a specific company, location and rack.
        /// 
        /// IMPORTANT:
        /// - Must always filter by CompanyId, LocationId and RackId
        /// - Must NOT return bins from other racks or tenants
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of bins</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Bines>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get all active bins for a specific company, location and rack.
        /// 
        /// IMPORTANT:
        /// - Must filter by CompanyId, LocationId, RackId and Active = true
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active bins</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Bines>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct);

        /// <summary>
        /// Get a bin by id validating ownership.
        /// 
        /// IMPORTANT:
        /// - Must filter by Id, CompanyId, LocationId and RackId
        /// - Must return null if the bin does not belong to the given scope
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Bin entity or null</returns>
        Task<DUNES.API.ModelsWMS.Masters.Bines?> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Validate if a bin name already exists within the same company, location and rack.
        /// 
        /// IMPORTANT:
        /// - Must filter by CompanyId, LocationId and RackId
        /// - excludeId is used when updating to ignore the current bin
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="name">Bin name</param>
        /// <param name="excludeId">Optional bin id to exclude from validation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByNameAsync(
            int companyId,
            int locationId,
            int rackId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new bin.
        /// 
        /// IMPORTANT:
        /// - Entity must already have CompanyId, LocationId and RackId assigned
        /// - Repository must NOT infer or override ownership fields
        /// </summary>
        /// <param name="entity">Bin entity with ownership already set</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created bin entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.Bines> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Bines entity,
            CancellationToken ct);

        /// <summary>
        /// Update an existing bin.
        /// 
        /// IMPORTANT:
        /// - Entity ownership (CompanyId, LocationId, RackId) must NOT be changed here
        /// </summary>
        /// <param name="entity">Bin entity to update</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated bin entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.Bines> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Bines entity,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a bin.
        /// 
        /// IMPORTANT:
        /// - Must filter by CompanyId, LocationId and RackId
        /// - Must NOT update bins outside the given scope
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="isActive">Active flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

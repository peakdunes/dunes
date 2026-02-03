using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.Racks
{
    /// <summary>
    /// Racks Repository
    /// 
    /// Repository layer is the last line of defense for multi-tenant security.
    /// All read/update operations MUST filter by CompanyId and LocationId.
    /// </summary>
    public interface IRacksWMSAPIRepository
    {
        /// <summary>
        /// Get all racks for a specific company and location.
        /// 
        /// IMPORTANT:
        /// - Must always filter by CompanyId and LocationId
        /// - Must NOT return racks from other companies or locations
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of racks</returns>
        Task<List<WMSRacksQueryDTO>> GetAllAsync(
            int companyId,
            int locationId,
            CancellationToken ct);

        /// <summary>
        /// Get all active racks for a specific company and location.
        /// 
        /// IMPORTANT:
        /// - Must filter by CompanyId, LocationId and Active = true
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active racks</returns>
        Task<List<WMSRacksQueryDTO>> GetActiveAsync(
            int companyId,
            int locationId,
            CancellationToken ct);

        /// <summary>
        /// Get a rack by id validating ownership.
        /// 
        /// IMPORTANT:
        /// - Must filter by Id, CompanyId and LocationId
        /// - Must return null if rack does not belong to the company/location
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="id">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Rack entity or null</returns>
        Task<WMSRacksQueryDTO?> GetByIdAsync(
            int companyId,
            int locationId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Validate if a rack name already exists for the same company and location.
        /// 
        /// IMPORTANT:
        /// - Must filter by CompanyId and LocationId
        /// - excludeId is used when updating to ignore the current rack
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="name">Rack name</param>
        /// <param name="excludeId">Optional rack id to exclude from validation</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByNameAsync(
            int companyId,
            int locationId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new rack.
        /// 
        /// IMPORTANT:
        /// - Entity must already have CompanyId and LocationId set
        /// - Repository must NOT infer or override CompanyId
        /// </summary>
        /// <param name="entity">Rack entity with ownership already assigned</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created rack entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.Racks> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Racks entity,
            CancellationToken ct);

        /// <summary>
        /// Update an existing rack.
        /// 
        /// IMPORTANT:
        /// - Entity must already belong to the correct CompanyId and LocationId
        /// - Repository must NOT change ownership fields
        /// </summary>
        /// <param name="entity">Rack entity to update</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated rack entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.Racks> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Racks entity,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a rack.
        /// 
        /// IMPORTANT:
        /// - Must filter by CompanyId and LocationId
        /// - Must NOT update racks from other tenants
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="id">Rack identifier</param>
        /// <param name="isActive">Active flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int locationId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

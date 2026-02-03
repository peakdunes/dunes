using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

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
    /// Every query MUST be filtered by CompanyId, LocationId and RackId.
    /// </summary>
    public class BinsWMSAPIRepository : IBinsWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        /// <param name="context">WMS DbContext</param>
        public BinsWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new bin.
        /// 
        /// IMPORTANT:
        /// - The entity must already contain CompanyId, LocationId and RackId
        /// - Ownership must NOT be inferred or modified here
        /// </summary>
        /// <param name="entity">Bin entity with ownership already set</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created bin entity</returns>
        public async Task<ModelsWMS.Masters.Bines> CreateAsync(
            ModelsWMS.Masters.Bines entity,
            CancellationToken ct)
        {
            _context.Bines.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Validate if a bin name already exists within the same company, location and rack.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="name">Bin name</param>
        /// <param name="excludeId">Optional bin id to exclude (used on update)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            int locationId,
            int rackId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _context.Bines
                .AsNoTracking()
                .Where(x =>
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.RacksId == rackId &&
                    x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Get all active bins for a specific company, location and rack.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active bins</returns>
        public async Task<List<ModelsWMS.Masters.Bines>> GetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            return await _context.Bines
                .AsNoTracking()
                .Where(x =>
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.RacksId == rackId &&
                    x.Active)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get all bins for a specific company, location and rack.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of bins</returns>
        public async Task<List<ModelsWMS.Masters.Bines>> GetAllAsync(
            int companyId,
            int locationId,
            int rackId,
            CancellationToken ct)
        {
            return await _context.Bines
                .AsNoTracking()
                .Where(x =>
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.RacksId == rackId)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get a bin by id validating ownership.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Bin entity or null if not found in scope</returns>
        public async Task<ModelsWMS.Masters.Bines?> GetByIdAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            CancellationToken ct)
        {
            return await _context.Bines
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.RacksId == rackId,
                    ct);
        }

        /// <summary>
        /// Activate or deactivate a bin.
        /// </summary>
        /// <param name="companyId">Company identifier</param>
        /// <param name="locationId">Location identifier</param>
        /// <param name="rackId">Rack identifier</param>
        /// <param name="id">Bin identifier</param>
        /// <param name="isActive">Active flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if updated, false if not found</returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int locationId,
            int rackId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.Bines
                .FirstOrDefaultAsync(x =>
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.RacksId == rackId &&
                    x.Id == id,
                    ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Update an existing bin.
        /// 
        /// IMPORTANT:
        /// - Entity ownership (CompanyId, LocationId, RackId) must NOT be modified here
        /// </summary>
        /// <param name="entity">Bin entity to update</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated bin entity</returns>
        public async Task<ModelsWMS.Masters.Bines> UpdateAsync(
            ModelsWMS.Masters.Bines entity,
            CancellationToken ct)
        {
            _context.Bines.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

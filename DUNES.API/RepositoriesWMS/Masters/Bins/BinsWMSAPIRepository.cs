using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Bins
{

    /// <summary>
    /// Bins repository
    /// </summary>
    public class BinsWMSAPIRepository : IBinsWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Constructor DI
        /// </summary>
        public BinsWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add new Bin
        /// </summary>
        public async Task<ModelsWMS.Masters.Bines> CreateAsync(
            ModelsWMS.Masters.Bines entity,
            CancellationToken ct)
        {
            _context.Bines.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Check if a Bin exists by name (Company + Location + Rack)
        /// </summary>
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
        /// Get all active bins (Company + Location + Rack)
        /// </summary>
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
        /// Get all bins (Company + Location + Rack)
        /// </summary>
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
        /// Get bin by id (Company + Location + Rack)
        /// </summary>
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
        /// Activate / Deactivate bin
        /// </summary>
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
        /// Update bin
        /// </summary>
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

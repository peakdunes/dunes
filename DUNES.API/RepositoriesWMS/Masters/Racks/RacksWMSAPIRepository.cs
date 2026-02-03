using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Racks
{
    /// <summary>
    /// Racks Repository
    /// 
    /// IMPORTANT:
    /// This layer is the last line of defense for multi-tenant security.
    /// All queries MUST filter by CompanyId and LocationId.
    /// </summary>
    public class RacksWMSAPIRepository : IRacksWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public RacksWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new rack.
        /// 
        /// IMPORTANT:
        /// - Entity must already contain CompanyId and LocationId.
        /// - Repository must not infer ownership.
        /// </summary>
        public async Task<ModelsWMS.Masters.Racks> CreateAsync(
            ModelsWMS.Masters.Racks entity,
            CancellationToken ct)
        {
            _context.Racks.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Check if a rack name already exists for the same company and location.
        /// </summary>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            int locationId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _context.Racks
                .AsNoTracking()
                .Where(x =>
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Get all active racks for a company and location.
        /// </summary>
        public async Task<List<WMSRacksQueryDTO>> GetActiveAsync(
            int companyId,
            int locationId,
            CancellationToken ct)
        {
            return await _context.Racks
         .AsNoTracking()
         .Where(r =>
             r.Idcompany == companyId &&
             r.LocationsId == locationId 
             && r.Active)
         .Select(r => new WMSRacksQueryDTO
         {
             Id = r.Id,
             Name = r.Name,
             Active = r.Active,
             LocationsId = r.LocationsId,
             LocationName = r.Locations.Name,
             Idcompany = r.Idcompany,
             CompanyName = r.IdcompanyNavigation.Name
         })
         .OrderBy(r => r.Name)
         .ToListAsync(ct);
        }

        /// <summary>
        /// Get all racks for a company and location.
        /// </summary>
        public async Task<List<WMSRacksQueryDTO>> GetAllAsync(
            int companyId,
            int locationId,
            CancellationToken ct)
        {
            return await _context.Racks
         .AsNoTracking()
         .Where(r =>
             r.Idcompany == companyId &&
             r.LocationsId == locationId)
         .Select(r => new WMSRacksQueryDTO
         {
             Id = r.Id,
             Name = r.Name,
             Active = r.Active,
             LocationsId = r.LocationsId,
             LocationName = r.Locations.Name,
             Idcompany = r.Idcompany,
             CompanyName = r.IdcompanyNavigation.Name
         })
         .OrderBy(r => r.Name)
         .ToListAsync(ct);
        }

        /// <summary>
        /// Get a rack by id validating company and location ownership.
        /// 
        /// IMPORTANT:
        /// - Must filter by Id, CompanyId and LocationId.
        /// </summary>
        public async Task<WMSRacksQueryDTO?> GetByIdAsync(
            int companyId,
            int locationId,
            int id,
            CancellationToken ct)
        {
            return await _context.Racks
    .AsNoTracking()
    .Where(x =>
        x.Id == id &&
        x.Idcompany == companyId &&
        x.LocationsId == locationId)
    .Select(x => new WMSRacksQueryDTO
    {
        Id = x.Id,
        Name = x.Name,
        Active = x.Active,
        LocationsId = x.LocationsId,
        LocationName = x.Locations.Name,
        Idcompany = x.Idcompany,
        CompanyName = x.IdcompanyNavigation.Name
    })
    .FirstOrDefaultAsync(ct);






        }

        /// <summary>
        /// Activate or deactivate a rack.
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int locationId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.Racks
                .FirstOrDefaultAsync(x =>
                    x.Idcompany == companyId &&
                    x.LocationsId == locationId &&
                    x.Id == id,
                    ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Update an existing rack.
        /// 
        /// IMPORTANT:
        /// - Entity ownership must not be changed here.
        /// </summary>
        public async Task<ModelsWMS.Masters.Racks> UpdateAsync(
            ModelsWMS.Masters.Racks entity,
            CancellationToken ct)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

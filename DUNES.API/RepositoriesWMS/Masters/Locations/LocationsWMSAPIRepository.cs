using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Locations
{
    /// <summary>
    /// Locations Interface Implementation
    /// </summary>
    public class LocationsWMSAPIRepository : ILocationsWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Constructor (DI)
        /// </summary>
        public LocationsWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new location
        /// 
        /// IMPORTANT:
        /// - Entity must already contain CompanyId
        /// </summary>
        public async Task<ModelsWMS.Masters.Locations> CreateAsync(
            ModelsWMS.Masters.Locations entity,
            CancellationToken ct)
        {
            _context.Locations.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Check if a location name already exists for a company
        /// </summary>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _context.Locations
                .AsNoTracking()
                .Where(x =>
                    x.Idcompany == companyId &&
                    x.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Get all active locations for a company
        /// </summary>
        public async Task<List<WMSLocationsReadDTO>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Active)
                .Select(l => new WMSLocationsReadDTO
                {
                    Id = l.Id,
                    Name = l.Name,
                    Idcompany = l.Idcompany,
                    Idcountry = l.Idcountry,
                    Idstate = l.Idstate,
                    Idcity = l.Idcity,
                    Zipcode = l.Zipcode,
                    Address = l.Address,
                    Phone = l.Phone,
                    Active = l.Active,
                    cityname = l.IdcityNavigation.Name,
                    companyname = l.IdcompanyNavigation.Name,
                    countryname = l.IdcountryNavigation.Name,
                    statename = l.IdstateNavigation.Name
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get all locations for a company
        /// </summary>
        public async Task<List<WMSLocationsReadDTO>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId)
                .Select(l => new WMSLocationsReadDTO
                {
                    Id = l.Id,
                    Name = l.Name,
                    Idcompany = l.Idcompany,
                    Idcountry = l.Idcountry,
                    Idstate = l.Idstate,
                    Idcity = l.Idcity,
                    Zipcode = l.Zipcode,
                    Address = l.Address,
                    Phone = l.Phone,
                    Active = l.Active,
                    cityname = l.IdcityNavigation.Name,
                    companyname = l.IdcompanyNavigation.Name,
                    countryname = l.IdcountryNavigation.Name,
                    statename = l.IdstateNavigation.Name
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get location by id validating ownership
        /// </summary>
        public async Task<WMSLocationsReadDTO?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            return await _context.Locations
                .AsNoTracking()
                .Where(l => l.Idcompany == companyId && l.Id == id)
                .Select(l => new WMSLocationsReadDTO
                {
                    Id = l.Id,
                    Name = l.Name,
                    Idcompany = l.Idcompany,
                    Idcountry = l.Idcountry,
                    Idstate = l.Idstate,
                    Idcity = l.Idcity,
                    Zipcode = l.Zipcode,
                    Address = l.Address,
                    Phone = l.Phone,
                    Active = l.Active,
                    cityname = l.IdcityNavigation.Name,
                    companyname = l.IdcompanyNavigation.Name,
                    countryname = l.IdcountryNavigation.Name,
                    statename = l.IdstateNavigation.Name
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Activate or deactivate a location
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.Locations
                .FirstOrDefaultAsync(
                    x => x.Idcompany == companyId && x.Id == id,
                    ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Update an existing location
        /// 
        /// IMPORTANT:
        /// - CompanyId must NOT be changed here
        /// </summary>
        public async Task<ModelsWMS.Masters.Locations> UpdateAsync(
            ModelsWMS.Masters.Locations entity,
            CancellationToken ct)
        {
            _context.Locations.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}


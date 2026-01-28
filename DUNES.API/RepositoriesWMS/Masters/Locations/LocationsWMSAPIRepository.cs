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
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public LocationsWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add new location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ModelsWMS.Masters.Locations> CreateAsync(
            ModelsWMS.Masters.Locations entity,
            CancellationToken ct)
        {
            _context.Locations.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Search location by name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _context.Locations
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// get active locations
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<WMSLocationsDTO>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            var query = await _context.Locations
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.Active)
                .Select(l => new WMSLocationsDTO
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

            return query;
        }

        /// <summary>
        /// get all locations
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<WMSLocationsDTO>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            var query = await _context.Locations
                .AsNoTracking()
                .Where(x => x.Idcompany == companyId)
                .Select(l => new WMSLocationsDTO
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

            return query;
        }

        /// <summary>
        /// get location information by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<WMSLocationsDTO?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            var info = await _context.Locations
                .AsNoTracking()
                .Where(l => l.Idcompany == companyId && l.Id == id)
                .Select(l => new WMSLocationsDTO
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

            return info;
        }

        /// <summary>
        /// Active no active location
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.Locations
                .FirstOrDefaultAsync(x => x.Idcompany == companyId && x.Id == id, ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// update location information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ModelsWMS.Masters.Locations> UpdateAsync(
            ModelsWMS.Masters.Locations entity,
            CancellationToken ct)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

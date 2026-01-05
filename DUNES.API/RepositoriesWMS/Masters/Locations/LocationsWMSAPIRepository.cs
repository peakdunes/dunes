
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
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Locations> CreateAsync(ModelsWMS.Masters.Locations entity, CancellationToken ct)
        {
            _context.Locations.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Search location by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            var query = _context.Locations.AsNoTracking().Where(x => x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }
        /// <summary>
        /// get active locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WMSLocationsDTO>> GetActiveAsync(CancellationToken ct)
        {
            //var query = await (from loc in _context.Locations.AsNoTracking()
            //                   join com in _context.Company.AsNoTracking() on loc.Idcompany equals com.Id
            //                   join coun in _context.Countries.AsNoTracking() on loc.Idcountry equals coun.Id
            //                   join st in _context.StatesCountries.AsNoTracking() on loc.Idstate equals st.Id 
            //                   join city in _context.Cities.AsNoTracking() on loc.Idcity equals city.Id

            var query = await _context.Locations.Where(x => x.Active == true)
                .AsNoTracking()
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


                }).ToListAsync(ct);


            return query;
        }
        /// <summary>
        /// get all locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<WMSLocationsDTO>> GetAllAsync(CancellationToken ct)
        {
            var query = await _context.Locations
               .AsNoTracking()
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


               }).ToListAsync(ct);


            return query;
        }
        /// <summary>
        /// get location information by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<WMSLocationsDTO?> GetByIdAsync(int id, CancellationToken ct)
        {
            var info = await _context.Locations
              .AsNoTracking()
              .Where(l  => l.Id == id)
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


              }).FirstOrDefaultAsync(ct);


            return info;
        }

        /// <summary>
        /// Active no active location
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var entity = await _context.Locations.FirstOrDefaultAsync(x => x.Id == id, ct);
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
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Locations> UpdateAsync(ModelsWMS.Masters.Locations entity, CancellationToken ct)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

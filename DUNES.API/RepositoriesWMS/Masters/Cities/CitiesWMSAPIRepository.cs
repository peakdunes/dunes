using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Cities
{
    /// <summary>
    /// cities repository
    /// </summary>
    public class CitiesWMSAPIRepository: ICitiesWMSAPIRepository
    {
        private readonly appWmsDbContext _context;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public CitiesWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add new city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<ModelsWMS.Masters.Cities> CreateAsync(ModelsWMS.Masters.Cities entity, CancellationToken ct)
        {
            _context.Cities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// exist city name
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsByNameAsync(int countryid, string name, int? excludeId, CancellationToken ct)
        {
            var query = _context.Cities.AsNoTracking().Where(x => x.Idcountry == countryid && x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);


        }

        /// <summary>
        /// get all active cities
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.Cities>> GetActiveAsync(int countryid, CancellationToken ct)
        {
            var query = await _context.Cities
                .Include(x => x.IdcountryNavigation)
                .Include(x => x.IdstateNavigation)
                .Where(x => x.Idcountry == countryid && x.Active == true).ToListAsync();

            return query;
        }

        /// <summary>
        /// get all countries
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.Cities>> GetAllAsync(int countryid, CancellationToken ct)
        {
            var query = await _context.Cities
                .Include(x => x.IdcountryNavigation)
                .Include(x => x.IdstateNavigation)
                .Where(x => x.Idcountry == countryid)
                .ToListAsync();

            return query;
        }

        /// <summary>
        /// get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Cities?> GetByIdAsync(int id, CancellationToken ct)
        {
            var info = await _context.Cities
                .Include(x => x.IdcountryNavigation)
                .Include(x => x.IdstateNavigation)
                .FirstOrDefaultAsync(x => x.Id == id);

            return info;
        }
        /// <summary>
        /// active no active 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var entity = await _context.Cities.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }
        /// <summary>
        /// update country information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Cities> UpdateAsync(ModelsWMS.Masters.Cities entity, CancellationToken ct)
        {
            _context.Cities.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}


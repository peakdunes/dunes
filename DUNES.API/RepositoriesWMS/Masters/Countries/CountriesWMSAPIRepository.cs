
using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DUNES.API.RepositoriesWMS.Masters.Countries
{

    /// <summary>
    /// country repository implementation
    /// </summary>
    public class CountriesWMSAPIRepository : ICountriesWMSAPIRepository
    {

        private readonly appWmsDbContext _context;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public CountriesWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add new country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<ModelsWMS.Masters.Countries> CreateAsync(ModelsWMS.Masters.Countries entity, CancellationToken ct)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// exist country name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            var query = _context.Countries.AsNoTracking().Where(x => x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);

            
        }

        /// <summary>
        /// get all active countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.Countries>> GetActiveAsync(CancellationToken ct)
        {
            var query = await _context.Countries.Where(x => x.Active == true).ToListAsync();

            return query;
        }

        /// <summary>
        /// get all countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.Countries>> GetAllAsync(CancellationToken ct)
        {
            var query = await _context.Countries.ToListAsync();

            return query;
        }

        /// <summary>
        /// get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Countries?> GetByIdAsync(int id, CancellationToken ct)
        {
            var info = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id);

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
            var entity = await _context.Countries.FirstOrDefaultAsync(x => x.Id == id, ct);
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
        public async Task<ModelsWMS.Masters.Countries> UpdateAsync(ModelsWMS.Masters.Countries entity, CancellationToken ct)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

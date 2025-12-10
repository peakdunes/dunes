using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.StateCountries
{

    /// <summary>
    /// state repository
    /// </summary>
    public class StateCountriesWMSAPIRepository : IStateCountriesWMSAPIRepository
    {
        private readonly appWmsDbContext _context;


        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="context"></param>
        public StateCountriesWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add new state
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>

        public async Task<WMSStatesCountriesDTO> CreateAsync(WMSStatesCountriesDTO dto, CancellationToken ct)
        {

            var entity = new DUNES.API.ModelsWMS.Masters.StatesCountries
            {
                Idcountry = dto.Idcountry,
                Name = dto.Name!.Trim(),
                Active = dto.Active,
                Sigla = dto.Sigla

            };

            _context.StatesCountries.Add(entity);
            await _context.SaveChangesAsync();
            return dto;
        }
        /// <summary>
        /// exist by ISO Code
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="isocode"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ModelsWMS.Masters.StatesCountries?> ExistsByISOCodeAsync(int countryid, string isocode, int? excludeId, CancellationToken ct)
        {
            var query = _context.StatesCountries.AsNoTracking().Where(x => x.Idcountry == countryid && x.Sigla == isocode);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// exist state name
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.StatesCountries?> ExistsByNameAsync(int countryid, string name, int? excludeId, CancellationToken ct)
        {
            var query = _context.StatesCountries.AsNoTracking().Where(x => x.Idcountry == countryid && x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

           

            return await query.FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// get all active states
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="countryid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.StatesCountries>> GetActiveAsync(int countryid, CancellationToken ct)
        {
            var query = await _context.StatesCountries.Where(x => x.Idcountry == countryid)
                .Include(x => x.IdcountryNavigation)
                .Where(x => x.Active == true).ToListAsync();

            return query;
        }

        /// <summary>
        /// get all states
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.StatesCountries>> GetAllAsync(int countryid, CancellationToken ct)
        {
            var query = await _context.StatesCountries.Where(x => x.Idcountry == countryid)
                .Include(x => x.IdcountryNavigation)
                 .Include(x => x.IdcountryNavigation)
                .ToListAsync();

            return query;
        }

        /// <summary>
        /// get state by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.StatesCountries?> GetByIdAsync( int id, CancellationToken ct)
        {
            var info = await _context.StatesCountries.Include(x => x.IdcountryNavigation).FirstOrDefaultAsync(x => x.Id == id);

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
            var entity = await _context.StatesCountries.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }
        /// <summary>
        /// update state information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.StatesCountries> UpdateAsync(ModelsWMS.Masters.StatesCountries entity, CancellationToken ct)
        {
            _context.StatesCountries.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

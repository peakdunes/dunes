
using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Racks
{

    /// <summary>
    /// Racks Repository
    /// </summary>
    public class RacksWMSAPIRepository : IRacksWMSAPIRepository
    {


        private readonly appWmsDbContext _context;

        /// <summary>
        /// contructor DI
        /// </summary>
        /// <param name="context"></param>
        public RacksWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add new Racks
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Racks> CreateAsync(ModelsWMS.Masters.Racks entity, CancellationToken ct)
        {
            _context.Racks.Add(entity);
            await _context.SaveChangesAsync();
            return entity;

           
        }

        /// <summary>
        /// check if exist a Rack by Name
        /// </summary>
        ///  <param name="companyId"></param>
        ///  <param name="locationId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsByNameAsync(int companyId, int locationId, string name, int? excludeId, CancellationToken ct)
        {
            var query = _context.Racks.AsNoTracking()
                .Where(x => x.Idcompany == companyId && x.LocationsId == locationId  && x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }
        /// <summary>
        /// Get all acive racks
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.Racks>> GetActiveAsync(int companyId, int locationId, CancellationToken ct)
        {
            var query = await _context.Racks.Where(x => x.Idcompany == companyId && x.LocationsId == locationId && x.Active == true).ToListAsync();

            return query;
        }
        /// <summary>
        /// get all racks
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<ModelsWMS.Masters.Racks>> GetAllAsync(int companyId, int locationId, CancellationToken ct)
        {
            var query = await _context.Racks.Where(x => x.Idcompany == companyId && x.LocationsId == locationId).ToListAsync();

            return query;
        }

        /// <summary>
        /// get a rack by companyid by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Racks?> GetByIdAsync(int companyId, int locationId, int id, CancellationToken ct)
        {
            var info = await _context.Racks.FirstOrDefaultAsync(x => x.Id == id && x.LocationsId == locationId);

            return info;
        }

        /// <summary>
        /// Active no active rack
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SetActiveAsync(int companyId, int locationId, int id, bool isActive, CancellationToken ct)
        {
            var entity = await _context.Racks.FirstOrDefaultAsync(x => x.Idcompany == companyId && x.LocationsId == locationId &&  x.Id == id, ct);
            if (entity is null)
                return false;

            entity.Active = isActive;
            await _context.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// update rack information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ModelsWMS.Masters.Racks> UpdateAsync(ModelsWMS.Masters.Racks entity, CancellationToken ct)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

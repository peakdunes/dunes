using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// inventory category repository implementation
    /// </summary>
    public class InventoryCategoriesWMSAPIRepository : IInventoryCategoriesWMSAPIRepository
    {

        private readonly appWmsDbContext _db;


        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="db"></param>
        public InventoryCategoriesWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// get all categories for company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>

        public async Task<List<Inventorycategories>> GetAllByCompanyAsync(int companyId)
        {
            return await _db.Inventorycategories
                .AsNoTracking()
                .Where(x => x.companyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        /// <summary>
        /// get category by company and id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<Inventorycategories?> GetByIdAsync(int id, int companyId)
        {
            return await _db.Inventorycategories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId);
        }

      /// <summary>
      /// add new category
      /// </summary>
      /// <param name="entity"></param>
      /// <returns></returns>

        public async Task<Inventorycategories> CreateAsync(Inventorycategories entity)
        {
            _db.Inventorycategories.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// update category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>

        public async Task<bool> UpdateAsync(Inventorycategories entity)
        {
            var exists = await _db.Inventorycategories
                .AnyAsync(x => x.Id == entity.Id && x.companyId == entity.companyId);

            if (!exists)
                return false;

            _db.Inventorycategories.Update(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// delete category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int id, int companyId)
        {
            var entity = await _db.Inventorycategories
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId);

            if (entity == null)
                return false;

            _db.Inventorycategories.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

      
        /// <summary>
        /// Exist category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(int id, int companyId)
        {
            return await _db.Inventorycategories
                .AnyAsync(x => x.Id == id && x.companyId == companyId);
        }

        /// <summary>
        /// Exist category by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="excludeId"></param>
        /// <returns></returns>
        public async Task<bool> NameExistsAsync(string name, int companyId, int? excludeId = null)
        {
            var query = _db.Inventorycategories
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.Name != null &&
                    x.Name.ToLower() == name.ToLower()
                );

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync();
        }
    }
}

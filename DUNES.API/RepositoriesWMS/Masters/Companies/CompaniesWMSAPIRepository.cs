using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.Companies
{
    /// <summary>
    /// WMS Companies repository
    /// </summary>
    public class CompaniesWMSAPIRepository : ICompaniesWMSAPIRepository
    {

        private readonly appWmsDbContext _wmscontext;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="wmscontext"></param>
        public CompaniesWMSAPIRepository(appWmsDbContext wmscontext)
        {
            _wmscontext = wmscontext;

        }
        /// <summary>
        /// add new company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Company> CreateAsync(Company entity, CancellationToken ct)
        {
            _wmscontext.Add(entity);
            await _wmscontext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// exist company by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct)
        {
            var query = _wmscontext.Company.AsNoTracking().Where(x => x.Name == name);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// get all active companies
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Company>> GetActiveAsync(CancellationToken ct)
        {
            var query = await _wmscontext.Company.Where(x => x.Active == true).ToListAsync();

            return query;
        }

        /// <summary>
        /// Get all companies information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Company>> GetAllCompaniesInformation(CancellationToken ct)
        {
            var infocompanylist = await _wmscontext.Company
                .Include(x => x.IdcountryNavigation)
                .Include(x => x.IdcityNavigation)
                .Include(x => x.IdstateNavigation)
                .ToListAsync(ct);

            return infocompanylist;
        }
        /// <summary>
        /// get company by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Company?> GetByIdAsync(int id, CancellationToken ct)
        {
            var info = await _wmscontext.Company.FirstOrDefaultAsync(x => x.Id == id);

            return info;
        }


      
        /// <summary>
        /// set active / no active company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct)
        {
            var entity = await _wmscontext.Company.FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null)
                return false;

            entity.Active = isActive;
            await _wmscontext.SaveChangesAsync(ct);
            return true;
        }
        /// <summary>
        /// Update company information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Company> UpdateAsync(Company entity, CancellationToken ct)
        {
            _wmscontext.Company.Update(entity);
            await _wmscontext.SaveChangesAsync(ct);
            return entity;
        }
        /// <summary>
        /// validation for a active company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> IsActiveAsync(int companyId, CancellationToken ct)
        {
            return await _wmscontext.Company
                .AsNoTracking()
                .AnyAsync(x => x.Id == companyId && x.Active, ct);
        }
    }
}

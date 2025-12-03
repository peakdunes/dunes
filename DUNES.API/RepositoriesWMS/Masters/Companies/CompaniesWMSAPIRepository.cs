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
        /// Get all information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Company> GetCompanyInformation(int companyid, CancellationToken ct)
        {
            var infocompany = await _wmscontext.Company.FirstOrDefaultAsync(x => x.Id == companyid, ct);

            return infocompany;


        }
    }
}

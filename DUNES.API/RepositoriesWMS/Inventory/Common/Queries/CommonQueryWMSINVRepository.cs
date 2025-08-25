using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Inventory.Common.Queries
{
    /// <summary>
    /// Get all information used for WMS inventory transactions (general queries)
    /// </summary>
    public class CommonQueryWMSINVRepository : ICommonQueryWMSINVRepository
    {


        private readonly appWmsDbContext _wmscontext;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="wmscontext"></param>
        public CommonQueryWMSINVRepository(appWmsDbContext wmscontext)
        {
            _wmscontext = wmscontext;
        }

        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            var listbines = await _wmscontext.Bines
                .Where(x => x.Idcompany == companyid 
                && x.Idcompanyclient == companyClient 
                && x.Active == true).ToListAsync();

            return listbines;
        }
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {
            var listbines = await _wmscontext.Bines.Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient).ToListAsync();

            return listbines;
        }
    }
}

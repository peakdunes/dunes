using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters
{
    /// <summary>
    /// All common Master Tables queries
    /// </summary>
    public class CommonQueryWMSMasterRepository : ICommonQueryWMSMasterRepository
    {


        private readonly appWmsDbContext _wmscontext;


        /// <summary>
        /// dependency injection
        /// </summary>
        /// <param name="wmscontext"></param>
        public CommonQueryWMSMasterRepository(appWmsDbContext wmscontext)
        {
            _wmscontext = wmscontext;
            
        }
        /// <summary>
        /// Get all information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public async Task<Company> GetCompanyInformation(int companyid)
        {
            var infocompany = await _wmscontext.Company.FirstOrDefaultAsync(x => x.Id == companyid);
                       
            return infocompany;


        }

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public async Task<List<Locations>> GetAllLocationsByCompany(int companyid)
        {
            var infolocations = await _wmscontext.Locations.Where(x => x.Id == companyid).ToListAsync();

            return infolocations;


        }
        /// <summary>
        /// Get all inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient)
        {
            var infotypes = await _wmscontext.InventoryTypes.Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync();

            return infotypes;
        }
        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient)
        {
            var infotypes = await _wmscontext.InventoryTypes
                .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync();

            return infotypes;
        }

        /// <summary>
        /// Get all item status for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllItemStatusByCompanyClient(int companyid, string companyClient)
        {
            var infostatus = await _wmscontext.Itemstatus
            .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync();

            return infostatus;

        }

        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient)
        {
            var infostatus = await _wmscontext.Itemstatus
               .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync();

            return infostatus;
        }



        /// <summary>
        /// Get all rack for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Racks>> GetAllRacksByCompanyClient(int companyid, string companyClient)
        {
            var infofacks = await _wmscontext.Racks
            .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync();

            return infofacks;

        }

        /// <summary>
        /// Get all actives inventory type for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Racks>> GetAllActiveRacksByCompanyClient(int companyid, string companyClient)
        {
            var infofacks = await _wmscontext.Racks
               .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync();

            return infofacks;
        }

        /// <summary>
        /// Get all Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient)
        {
            var infobins = await _wmscontext.Bines
            .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync();

            return infobins;

        }

        /// <summary>
        /// Get all actives Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient)
        {
            var infobins = await _wmscontext.Bines
               .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync();

            return infobins;
        }

    }
}

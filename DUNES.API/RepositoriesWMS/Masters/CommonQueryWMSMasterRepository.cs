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
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Company> GetCompanyInformation(int companyid, CancellationToken ct)
        {
            var infocompany = await _wmscontext.Company.FirstOrDefaultAsync(x => x.Id == companyid, ct);
                       
            return infocompany;


        }

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Locations>> GetAllActiveLocationsByCompany(int companyid, CancellationToken ct)
        {
            var infolocations = await _wmscontext.Locations.Where(x => x.Id == companyid && x.Active == true).ToListAsync(ct);

            return infolocations;


        }
        /// <summary>
        /// Get all inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infotypes = await _wmscontext.InventoryTypes.Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync(ct);

            return infotypes;
        }
        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<InventoryTypes>> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infotypes = await _wmscontext.InventoryTypes
                .Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync(ct);

            return infotypes;
        }

        /// <summary>
        /// Get all item status for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infostatus = await _wmscontext.Itemstatus
            .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync(ct);

            return infostatus;

        }

        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Itemstatus>> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infostatus = await _wmscontext.Itemstatus
               .Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync(ct);

            return infostatus;
        }



        /// <summary>
        /// Get all rack for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Racks>> GetAllRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infofacks = await _wmscontext.Racks
            .Where(x => x.Id == companyid && x.Idcompanyclient == companyClient).ToListAsync(ct);

            return infofacks;

        }

        /// <summary>
        /// Get all actives inventory type for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Racks>> GetAllActiveRacksByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infofacks = await _wmscontext.Racks
               .Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync(ct);

            return infofacks;
        }

        /// <summary>
        /// Get all Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infobins = await _wmscontext.Bines
            .Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient).ToListAsync(ct);

            return infobins;

        }

        /// <summary>
        /// Get all actives Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        public async Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infobins = await _wmscontext.Bines
               .Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient && x.Active == true).ToListAsync(ct);

            return infobins;
        }
        /// <summary>
        /// Get all Company Client actives for a Location id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="locationid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Locationclients>> GetAllActiveClientCompaniesByLocation(int companyid, int locationid, CancellationToken ct)
        {
            var intoclientloclist = await _wmscontext.Locationclients.Where(x => x.Idcompany == companyid
                                                        && x.Idlocation == locationid && x.Active == true).ToListAsync(ct);
            return intoclientloclist;
        }
        /// <summary>
        /// Get all Warehouse Organization for a Client Company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<List<Warehouseorganization>> GetAllWareHouseOrganizationByCompanyClient(int companyid, string companyClient, CancellationToken ct)
        {
            var infoOrg = _wmscontext.warehouseorganization.Where(x => x.Idcompany == companyid && x.Idcompanyclient == companyClient).ToListAsync(ct);

            return infoOrg;
        }
    }
}

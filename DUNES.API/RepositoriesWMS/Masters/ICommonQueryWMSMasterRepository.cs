using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.RepositoriesWMS.Masters
{

    /// <summary>
    /// All common Master Tables queries
    /// </summary>
    public interface ICommonQueryWMSMasterRepository
    {

        /// <summary>
        /// Get all information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        Task<Company> GetCompanyInformation(int companyid);

        /// <summary>
        /// Get all locations for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        Task<List<Locations>> GetAllLocationsByCompany(int companyid);


        /// <summary>
        /// Get all inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllInventoryTypesByCompanyClient(int companyid, string companyClient);


        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<InventoryTypes>> GetAllActiveInventoryTypesByCompanyClient(int companyid, string companyClient);



        /// <summary>
        /// Get all item status for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Itemstatus>> GetAllItemStatusByCompanyClient(int companyid, string companyClient);

        /// <summary>
        /// Get all actives inventory type for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Itemstatus>> GetAllActiveItemStatusByCompanyClient(int companyid, string companyClient);


        /// <summary>
        /// Get all Rack for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Racks>> GetAllRacksByCompanyClient(int companyid, string companyClient);

        /// <summary>
        /// Get all actives Racks for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Racks>> GetAllActiveRacksByCompanyClient(int companyid, string companyClient);



        /// <summary>
        /// Get all Bins for a company client by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient);

        /// <summary>
        /// Get all actives Bins for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient);




    }
}

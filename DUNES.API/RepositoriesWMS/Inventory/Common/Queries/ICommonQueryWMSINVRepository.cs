using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Inventory.Common.Queries
{


    /// <summary>
    /// Get all information used for WMS inventory transactions (general queries)
    /// </summary>
    public interface ICommonQueryWMSINVRepository
    {
        /// <summary>
        /// Get all the bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllBinsByCompanyClient(int companyid, string companyClient);


        /// <summary>
        /// Get all Active bins associated with a client company 
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="companyClient"></param>
        /// <returns></returns>
        Task<List<Bines>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient);



       

    }
}

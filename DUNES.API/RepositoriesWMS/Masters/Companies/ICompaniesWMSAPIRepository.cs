using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.Companies
{

    /// <summary>
    /// WMS Companies repository
    /// </summary>
    public interface ICompaniesWMSAPIRepository
    {

        /// <summary>
        /// Get all company information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<Company>> GetAllCompaniesInformation(CancellationToken ct);

        /// <summary>
        /// Get all information for a company by id
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Company> GetCompanyInformation(int companyid, CancellationToken ct);

    }
}

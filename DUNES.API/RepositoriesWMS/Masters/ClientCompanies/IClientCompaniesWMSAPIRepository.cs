using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.ClientCompanies
{

    /// <summary>
    /// Client Companies Repository
    /// </summary>
    public interface IClientCompaniesWMSAPIRepository
    {

        /// <summary>
        /// Get all client company  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<CompanyClient>> GetAllClientCompaniesInformationAsync(CancellationToken ct);

        /// <summary>
        /// Get all client information for a company by company identification
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<CompanyClient> GetClientCompanyInformationByIdentificationAsync(string companyid, CancellationToken ct);


        /// <summary>
        /// Get all client information for a company by company identification
        /// </summary>
        /// <param name="companyname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<CompanyClient> GetClientCompanyInformationByNameAsync(string companyname, CancellationToken ct);


        /// <summary>
        /// Get all client information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<CompanyClient> GetClientCompanyInformationByIdAsync(int Id, CancellationToken ct);


        /// <summary>
        /// add new client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<CompanyClient> AddClientCompanyAsync(CompanyClient entity, CancellationToken ct);

        /// <summary>
        /// update client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateClientCompanyAsync(CompanyClient entity, CancellationToken ct);

        /// <summary>
        /// delete client company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteClientCompanyAsync(int id, CancellationToken ct);
    }
}

using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{


    /// <summary>
    /// Companies Client contract
    /// </summary>
    public interface ICompaniesContractWMSAPIRepository
    {


        /// <summary>
        /// Get all client company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<API.ModelsWMS.Masters.CompaniesContract>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct);

        /// <summary>
        /// Get all client contract information for a company by company identification
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<API.ModelsWMS.Masters.CompaniesContract> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct);


       

        /// <summary>
        /// Get all client contract information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<API.ModelsWMS.Masters.CompaniesContract> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct);


        /// <summary>
        /// add new client company contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<API.ModelsWMS.Masters.CompaniesContract> AddClientCompanyContractAsync(API.ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct);

        /// <summary>
        /// update client company contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateClientCompanyContractAsync(CompanyClient entity, CancellationToken ct);

        /// <summary>
        /// delete client company contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteClientCompanyContractAsync(int id, CancellationToken ct);
    }
}

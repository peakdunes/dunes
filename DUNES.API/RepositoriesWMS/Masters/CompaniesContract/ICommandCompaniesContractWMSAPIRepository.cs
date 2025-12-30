using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// companies contract repository
    /// </summary>
    public interface ICommandCompaniesContractWMSAPIRepository
    {


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
        Task<bool> UpdateClientCompanyContractAsync(ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct);

        /// <summary>
        /// delete client company contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> DeleteClientCompanyContractAsync(int id, CancellationToken ct);
    }
}

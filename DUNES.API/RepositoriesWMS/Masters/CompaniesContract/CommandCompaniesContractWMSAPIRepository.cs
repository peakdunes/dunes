using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// company client contract CRUD
    /// </summary>
    public class CommandCompaniesContractWMSAPIRepository : ICommandCompaniesContractWMSAPIRepository
    {

        /// <summary>
        /// add new contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ModelsWMS.Masters.CompaniesContract> AddClientCompanyContractAsync(ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// delete contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> DeleteClientCompanyContractAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// update contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> UpdateClientCompanyContractAsync(CompanyClient entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// companies client contracts
    /// </summary>
    public class CompaniesContractWMSAPIService : ICompaniesContractWMSAPIService
    {

        /// <summary>
        /// add new contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<ModelsWMS.Masters.CompaniesContract>> AddClientCompanyContractAsync(ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// delete company contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get all companies contract
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<List<ModelsWMS.Masters.CompaniesContract>>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get company contract by contract id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<ModelsWMS.Masters.CompaniesContract>> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get company contract by contract number
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<ModelsWMS.Masters.CompaniesContract>> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// update company contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(CompanyClient entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

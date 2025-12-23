using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
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
        public Task<ApiResponse<WMSCompaniesContractDTO>> AddClientCompanyContractAsync(WMSCompaniesContractDTO entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// delete company client contract
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
        /// get all contract information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get contract information by id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// get contract information by company id
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetClientCompanyInformationContractByCompanyIdAsync(int companyclientid, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// get contract information by contract number
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// get contract information by contract number and company id
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberCompanyIdAsync(int companyclientid, string contractcode, CancellationToken ct)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// update contract information
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(WMSCompaniesContractDTO entity, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

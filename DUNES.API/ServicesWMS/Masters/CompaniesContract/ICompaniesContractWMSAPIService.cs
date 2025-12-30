using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompaniesContract
{


    /// <summary>
    /// companies client contracts
    /// </summary>
    public interface ICompaniesContractWMSAPIService
    {


        /// <summary>
        /// Get all client company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct);



        /// <summary>
        /// Get all company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationByCompanyIdAsync(int companyid, CancellationToken ct);




        /// <summary>
        /// Get all client company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationByCompanyClientIdAsync(int companyclientid, CancellationToken ct);



        /// <summary>
        /// Get all client contract information for a company by company identification
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByCompanyIdAndNumberAsync(int companyclientid, string contractcode, CancellationToken ct);




        /// <summary>
        /// Get all client contract information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByContractIdAsync(int Id, CancellationToken ct);



        /// <summary>
        /// Get all contract information by contract number
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct);



        /// <summary>
        /// add new client company contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddClientCompanyContractAsync(WMSCompaniesContractDTO entity, CancellationToken ct);

        /// <summary>
        /// update client company contract
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(WMSCompaniesContractDTO entity, CancellationToken ct);

        /// <summary>
        /// delete client company contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, CancellationToken ct);

    }
}

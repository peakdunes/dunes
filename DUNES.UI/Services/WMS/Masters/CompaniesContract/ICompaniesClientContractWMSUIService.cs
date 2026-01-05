using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompaniesContract
{
    public interface ICompaniesClientContractWMSUIService
    {


        /// <summary>
        /// Get all client company  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(string token, CancellationToken ct);

        /// <summary>
        /// Get all client information for a contract by number
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(string contractcode, string token, CancellationToken ct);


        /// <summary>
        /// Get all client information for a contract by companyid and name
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberCompanyIdAsync(int companyclientid, string contractcode, string token, CancellationToken ct);


        /// <summary>
        /// Get all client information for a company by company identification
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetClientCompanyInformationContractByCompanyIdAsync(int companyclientid, string token, CancellationToken ct);



        /// <summary>
        /// Get all client information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByIdAsync(int Id, string token, CancellationToken ct);


        /// <summary>
        /// add new client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddClientCompanyContractAsync(WMSCompaniesContractDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// update client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(WMSCompaniesContractDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// delete client company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, string token, CancellationToken ct);
    }
}

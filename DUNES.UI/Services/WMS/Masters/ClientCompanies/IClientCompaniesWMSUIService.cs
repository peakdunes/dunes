using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.ClientCompanies
{

    /// <summary>
    /// Company Client service
    /// </summary>
    public interface IClientCompaniesWMSUIService
    {

        /// <summary>
        /// Get all client company  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WmsCompanyclientDto>>> GetAllClientCompaniesInformation(string token, CancellationToken ct);

        /// <summary>
        /// Get client company information by company identification
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByIdentificationAsync(string companyid, string token, CancellationToken ct);


        /// <summary>
        /// Get all client information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByIdAsync(int Id, string token, CancellationToken ct);


        /// <summary>
        /// add new client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddClientCompanyAsync(WmsCompanyclientDto entity, string token, CancellationToken ct);

        /// <summary>
        /// update client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyAsync(WmsCompanyclientDto entity, string token, CancellationToken ct);

        /// <summary>
        /// delete client company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyAsync(int id, string token, CancellationToken ct);

    }
}

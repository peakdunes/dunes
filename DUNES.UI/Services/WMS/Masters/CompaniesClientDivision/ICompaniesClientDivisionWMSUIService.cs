using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompaniesClientDivision
{
    public interface ICompaniesClientDivisionWMSUIService
    {
        /// <summary>
        /// add new client company division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// update client company division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// delete client company division
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyDivisionAsync(int id, string token, CancellationToken ct);




        /// <summary>
        /// Get all company client division information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformation(string token, CancellationToken ct);


        /// <summary>
        /// Get all company client division information by company client
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformationByCompanyClient(int companyclientid, string token, CancellationToken ct);


        /// <summary>
        /// Get all division information for a company by id
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionById(int divisionId, string token, CancellationToken ct);



        /// <summary>
        /// Get all division client information for a company by company identification
        /// </summary>
        /// <param name="divisionname"></param>
        /// <param name="companyClientId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionByNameAsync(int companyClientId, string divisionname, string token, CancellationToken ct);



    }
}

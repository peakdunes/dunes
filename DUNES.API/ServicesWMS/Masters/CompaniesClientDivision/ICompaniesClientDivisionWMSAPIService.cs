using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompaniesClientDivision
{

    /// <summary>
    /// company client division service
    /// </summary>
    public interface ICompaniesClientDivisionWMSAPIService
    {


        /// <summary>
        /// add new client company division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, CancellationToken ct);

        /// <summary>
        /// update client company division
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyDivisionAsync(CompanyClientDivision entity, CancellationToken ct);

        /// <summary>
        /// delete client company division
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyDivisionAsync(int id, CancellationToken ct);




        /// <summary>
        /// Get all company information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformation(CancellationToken ct);

        /// <summary>
        /// Get all division information for a company by id
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionById(int divisionId, CancellationToken ct);



        /// <summary>
        /// Get all division client information for a company by company identification
        /// </summary>
        /// <param name="divisionname"></param>
        /// <param name="companyClientId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionByNameAsync(int companyClientId, string divisionname, CancellationToken ct);


    }
}

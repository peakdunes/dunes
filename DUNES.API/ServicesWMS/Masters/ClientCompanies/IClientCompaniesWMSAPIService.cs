using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.ClientCompanies
{

    /// <summary>
    /// Client Company services
    /// </summary>
    public interface IClientCompaniesWMSAPIService
    {

        /// <summary>
        /// Get all client company  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WmsCompanyclientDto>>> GetAllClientCompaniesInformation(CancellationToken ct);

        /// <summary>
        /// Get client company information by company identification
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByIdentificationAsync(string companyid, CancellationToken ct);




        /// <summary>
        /// get company client by name
        /// </summary>
        /// <param name="companyname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByNameAsync(string companyname, CancellationToken ct);



        /// <summary>
        /// Get all client information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByIdAsync(int Id, CancellationToken ct);


        /// <summary>
        /// add new client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddClientCompanyAsync(WmsCompanyclientDto entity, CancellationToken ct);

        /// <summary>
        /// update client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyAsync(CompanyClient entity, CancellationToken ct);

        /// <summary>
        /// delete client company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyAsync(int id, CancellationToken ct);
    }
}

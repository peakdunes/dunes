using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.CompaniesContract
{


    /// <summary>
    /// companies client contracts
    /// </summary>
    public interface ICompaniesContractWMSAPIService
    {


        /// <summary>
        /// Get all client company  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<API.ModelsWMS.Masters.CompaniesContract>>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct);

        /// <summary>
        /// Get all client information for a company by company identification
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<API.ModelsWMS.Masters.CompaniesContract>> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct);




        /// <summary>
        /// Get all client information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<API.ModelsWMS.Masters.CompaniesContract>> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct);


        /// <summary>
        /// add new client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<API.ModelsWMS.Masters.CompaniesContract>> AddClientCompanyContractAsync(API.ModelsWMS.Masters.CompaniesContract entity, CancellationToken ct);

        /// <summary>
        /// update client company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(CompanyClient entity, CancellationToken ct);

        /// <summary>
        /// delete client company
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, CancellationToken ct);
    }
}

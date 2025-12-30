using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Companies
{

    /// <summary>
    /// Companies service
    /// </summary>
    public interface ICompaniesWMSAPIService
    {

        /// <summary>
        /// get all companies
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesDTO>>> GetAllAsync(CancellationToken ct);
        /// <summary>
        /// get all active companies
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCompaniesDTO>>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get company by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCompaniesDTO?>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// add new company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSCompaniesDTO entity, CancellationToken ct);
        /// <summary>
        /// update company
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSCompaniesDTO entity, CancellationToken ct);
        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct);

        /// <summary>
        /// validate if exists a company with the same name
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);
    }
}

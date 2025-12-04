using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Cities
{

    /// <summary>
    /// cities service
    /// </summary>
    public interface ICitiesWMSAPIService
    {
        /// <summary>
        /// get all cities
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCitiesDTO>>> GetAllAsync(CancellationToken ct);
        /// <summary>
        /// get all active cities
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCitiesDTO>>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get city by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCitiesDTO?>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// add new city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSCitiesDTO entity, CancellationToken ct);
        /// <summary>
        /// update city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSCitiesDTO entity, CancellationToken ct);
        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct);

        /// <summary>
        /// validate if exists a city with the same name
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);

    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Locations
{
    /// <summary>
    /// location interface service
    /// </summary>
    public interface ILocationsWMSAPIService
    {
        /// <summary>
        /// get all locations
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationsDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// get all active locations
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationsDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// get location by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSLocationsDTO?>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// add new location
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSLocationsDTO entity,
            CancellationToken ct);

        /// <summary>
        /// update location
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            WMSLocationsDTO entity,
            CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// validate if exists a location with the same name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);
    }
}

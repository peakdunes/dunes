using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.Locations
{

    /// <summary>
    /// Locations UI Service
    /// </summary>
    public interface ILocationsWMSUIService
    {
        /// <summary>
        /// get all locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationsDTO>>> GetAllAsync(string token, CancellationToken ct);
        /// <summary>
        /// get all active locations
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSLocationsDTO>>> GetActiveAsync(string token, CancellationToken ct);

        /// <summary>
        /// get location by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSLocationsDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// add new location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSLocationsDTO entity, string token, CancellationToken ct);
        /// <summary>
        /// update location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSLocationsDTO entity, string token, CancellationToken ct);
        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);

        /// <summary>
        /// validate if exists a location with the same name
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct);
    }
}

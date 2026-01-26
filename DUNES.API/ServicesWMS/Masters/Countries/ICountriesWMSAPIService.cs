using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Countries
{
    /// <summary>
    /// Country service interface
    /// </summary>
    public interface ICountriesWMSAPIService
    {
        /// <summary>
        /// get all countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCountriesDTO>>> GetAllAsync(CancellationToken ct);
        /// <summary>
        /// get all active countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCountriesDTO>>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCountriesDTO>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// add new country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSCountriesDTO entity, CancellationToken ct);
        /// <summary>
        /// update country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSCountriesDTO entity, CancellationToken ct);
        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct);

        /// <summary>
        /// validate if exists a country with the same name
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);

    }
}

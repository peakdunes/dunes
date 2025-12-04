using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.StateCountries
{

    /// <summary>
    /// state services
    /// </summary>
    public interface IStateCountriesWMSAPIService
    {
        /// <summary>
        /// get all states
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetAllAsync(CancellationToken ct);
        /// <summary>
        /// get all active states
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSStatesCountriesDTO?>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// add new state
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSStatesCountriesDTO entity, CancellationToken ct);
        /// <summary>
        /// update state
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSStatesCountriesDTO entity, CancellationToken ct);
        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, CancellationToken ct);

        /// <summary>
        /// validate if exists a state with the same name
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);

    }
}

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
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSStatesCountriesReadDTO>>> GetAllAsync(int countryId, CancellationToken ct);
        /// <summary>
        /// get all active states
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSStatesCountriesReadDTO>>> GetActiveAsync(int countryId, CancellationToken ct);

        /// <summary>
        /// get state by id for a country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSStatesCountriesReadDTO?>> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// get state by name for a country
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="name"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSStatesCountriesReadDTO?>> GetByNameAsync(int countryid, string name, CancellationToken ct);



        /// <summary>
        /// get state by id ISO Code an country
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="isocode"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSStatesCountriesReadDTO?>> GetByISOCodeAsync(int countryid, string isocode, int? excludeId, CancellationToken ct);


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

    

    }
}

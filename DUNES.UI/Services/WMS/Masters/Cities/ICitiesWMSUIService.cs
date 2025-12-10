using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.Cities
{
    public interface ICitiesWMSUIService
    {
        /// <summary>
        /// Get all cities  information
        /// </summary>
        /// <param name="countryid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCitiesReadDTO>>> GetAllCitiesInformation(int countryid, string token, CancellationToken ct);

        /// <summary>
        /// Get city by country name
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCitiesReadDTO>> GetCityInformationByIdentificationAsync(string entityid, string token, CancellationToken ct);


        /// <summary>
        /// Get all city information by country Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCitiesReadDTO>> GetCityInformationByIdAsync(int Id, string token, CancellationToken ct);


        /// <summary>
        /// add new city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddCityAsync(WMSCitiesDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// update city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateCityAsync(WMSCitiesDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// delete city
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteCityAsync(int id, string token, CancellationToken ct);


    }
}

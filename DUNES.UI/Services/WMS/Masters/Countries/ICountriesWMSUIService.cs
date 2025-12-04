using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.Countries
{
    public interface ICountriesWMSUIService
    {

        /// <summary>
        /// Get all Countries  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSCountriesDTO>>> GetAllCountriesInformation(string token, CancellationToken ct);

        /// <summary>
        /// Get country by country name
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCountriesDTO>> GetCountryInformationByIdentificationAsync(string countryid, string token, CancellationToken ct);


        /// <summary>
        /// Get all country information by country Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSCountriesDTO>> GetCountryInformationByIdAsync(int Id, string token, CancellationToken ct);


        /// <summary>
        /// add new country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddCountryAsync(WMSCountriesDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// update country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateCountryAsync(WMSCountriesDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// delete country
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteCountryAsync(int id, string token, CancellationToken ct);


    }
}

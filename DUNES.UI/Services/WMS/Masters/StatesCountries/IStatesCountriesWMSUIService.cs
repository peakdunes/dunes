using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.StatesCountries
{
    public interface IStatesCountriesWMSUIService
    {

        /// <summary>
        /// Get all States  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetAllStatesCountryInformation(string token, CancellationToken ct);

        /// <summary>
        /// Get State by country name
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSStatesCountriesDTO>> GetStateCountryInformationByIdentificationAsync(string entityid, string token, CancellationToken ct);


        /// <summary>
        /// Get all State information by country Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSStatesCountriesDTO>> GetStateCountryInformationByIdAsync(int Id, string token, CancellationToken ct);


        /// <summary>
        /// add new State
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> AddStateCountryAsync(WMSStatesCountriesDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// update State
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateStateCountryAsync(WMSStatesCountriesDTO entity, string token, CancellationToken ct);

        /// <summary>
        /// delete State
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> DeleteStateCountryAsync(int id, string token, CancellationToken ct);


    }
}

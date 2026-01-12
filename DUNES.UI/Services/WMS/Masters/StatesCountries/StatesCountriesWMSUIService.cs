using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.StatesCountries
{
    public class StatesCountriesWMSUIService
        : UIApiServiceBase, IStatesCountriesWMSUIService
    {
        public StatesCountriesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> AddStateCountryAsync(
            WMSStatesCountriesDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSStatesCountriesDTO>(
                "/api/StatesCountriesWMS/create-state-country",
                entity,
                token,
                ct);

        public Task<ApiResponse<List<WMSStatesCountriesReadDTO>>> GetAllStatesCountryInformation(
            int countryId,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSStatesCountriesReadDTO>>(
                $"/api/StatesCountriesWMS/all-states-by-countries/{countryId}",
                token,
                ct);

        public Task<ApiResponse<WMSStatesCountriesReadDTO>> GetStateCountryInformationByIdentificationAsync(
            int countryId,
            string statename,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSStatesCountriesReadDTO>(
                $"/api/StatesCountriesWMS/state-country-by-name/{countryId}/{Uri.EscapeDataString(statename)}",
                token,
                ct);

        public Task<ApiResponse<WMSStatesCountriesReadDTO>> GetStateCountryInformationByISOCodeAsync(
            int countryId,
            string isocode,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSStatesCountriesReadDTO>(
                $"/api/StatesCountriesWMS/state-country-by-isocode/{countryId}/{Uri.EscapeDataString(isocode)}",
                token,
                ct);

        public Task<ApiResponse<WMSStatesCountriesReadDTO>> GetStateCountryInformationByIdAsync(
            int countryId,
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSStatesCountriesReadDTO>(
                $"/api/StatesCountriesWMS/state-country-by-id/{countryId}/{id}",
                token,
                ct);

        public Task<ApiResponse<bool>> DeleteStateCountryAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<bool>> UpdateStateCountryAsync(
            WMSStatesCountriesDTO entity,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();
    }
}

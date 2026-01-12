using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Lookups
{
    public class GeoLookupUIService
        : UIApiServiceBase, IGeoLookupUIService
    {
        public GeoLookupUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<List<WMSCountriesDTO>>> GetCountriesAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCountriesDTO>>(
                "/api/CountriesWMS/active-countries",
                token,
                ct);

        public Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetStatesByCountryAsync(
            int countryId,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSStatesCountriesDTO>>(
                $"/api/StatesCountriesWMS/active-states-countries/{countryId}",
                token,
                ct);

        public Task<ApiResponse<List<WMSCitiesDTO>>> GetCitiesByStateAsync(
            int stateId,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCitiesDTO>>(
                $"/api/CitiesWMS/active-cities/{stateId}",
                token,
                ct);
    }
}

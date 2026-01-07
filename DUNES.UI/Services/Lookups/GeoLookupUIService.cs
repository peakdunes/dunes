using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using DUNES.UI.Services.WMS.Masters.Cities;
using DUNES.UI.Services.WMS.Masters.Countries;
using DUNES.UI.Services.WMS.Masters.StatesCountries;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.Lookups
{
    public class GeoLookupUIService : IGeoLookupUIService
    {

        private readonly HttpClient _httpClient;


        public GeoLookupUIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("DUNES_API");
        }

        public async Task<ApiResponse<List<WMSCountriesDTO>>> GetCountriesAsync(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var respapi = await _httpClient.GetAsync($"/api/CountriesWMS/active-countries", ct);

            return await respapi.ReadAsApiResponseAsync<List<WMSCountriesDTO>>(ct);
        }

        public async Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetStatesByCountryAsync(int countryId, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var respapi = await _httpClient.GetAsync($"/api/StatesCountriesWMS/active-states-countries/{countryId}", ct);

            return await respapi.ReadAsApiResponseAsync<List<WMSStatesCountriesDTO>>(ct);
        }


        public async Task<ApiResponse<List<WMSCitiesDTO>>> GetCitiesByStateAsync(int stateId, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var respapi = await _httpClient.GetAsync($"/api/CitiesWMS/active-cities/{stateId}", ct);

            return await respapi.ReadAsApiResponseAsync<List<WMSCitiesDTO>>(ct);

        }

    

     
    }
}

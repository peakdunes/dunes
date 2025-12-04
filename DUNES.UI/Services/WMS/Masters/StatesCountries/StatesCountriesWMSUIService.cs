using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.WMS.Masters.StatesCountries
{
    public class StatesCountriesWMSUIService : IStatesCountriesWMSUIService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public StatesCountriesWMSUIService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<ApiResponse<bool>> AddStateCountryAsync(WMSStatesCountriesDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"api/StatesCountriesWMS/create-state-country", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteStateCountryAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetAllStatesCountryInformation(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/StatesCountriesWMS/all-states-by-countries");

            return await resp.ReadAsApiResponseAsync<List<WMSStatesCountriesDTO>>(ct);
        }

        public Task<ApiResponse<WMSStatesCountriesDTO>> GetStateCountryInformationByIdAsync(int Id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSStatesCountriesDTO>> GetStateCountryInformationByIdentificationAsync(string countryid, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateStateCountryAsync(WMSStatesCountriesDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

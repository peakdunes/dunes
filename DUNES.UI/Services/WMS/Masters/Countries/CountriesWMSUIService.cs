using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.WMS.Masters.Countries
{
    public class CountriesWMSUIService : ICountriesWMSUIService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public CountriesWMSUIService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<ApiResponse<bool>> AddCountryAsync(WMSCountriesDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"api/CountriesWMS/create-country", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteCountryAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WMSCountriesDTO>>> GetAllCountriesInformation(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CountriesWMS/all-countries");

            return await resp.ReadAsApiResponseAsync<List<WMSCountriesDTO>>(ct);
        }

        public Task<ApiResponse<WMSCountriesDTO>> GetCountryInformationByIdAsync(int Id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<bool>> GetCountryInformationByIdentificationAsync(string countryid, int? excludeId, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            var nameEncoded = Uri.EscapeDataString(countryid);

            resp = await _httpClient.GetAsync($"/api/CountriesWMS/country-by-name?name={nameEncoded}&excludeId={excludeId}");

            return await resp.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> UpdateCountryAsync(WMSCountriesDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

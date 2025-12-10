using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Mono.TextTemplating;
using Newtonsoft.Json;
using System.Diagnostics.Metrics;
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
                                                     
            var respapi = await _httpClient.PostAsync($"/api/StatesCountriesWMS/create-state-country", content);
                                 
            

            var body = await respapi.Content.ReadAsStringAsync(ct);


            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteStateCountryAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WMSStatesCountriesReadDTO>>> GetAllStatesCountryInformation(int countryId, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/StatesCountriesWMS/all-states-by-countries/{countryId}");

            return await resp.ReadAsApiResponseAsync<List<WMSStatesCountriesReadDTO>>(ct);
        }

        public async Task<ApiResponse<WMSStatesCountriesReadDTO>> GetStateCountryInformationByIdAsync(int countryid, int Id, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/StatesCountriesWMS/all-states-by-countries");

            return await resp.ReadAsApiResponseAsync<WMSStatesCountriesReadDTO>(ct);

        }

        public async Task<ApiResponse<WMSStatesCountriesReadDTO>> GetStateCountryInformationByIdentificationAsync(int countryId, string statename, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/StatesCountriesWMS/state-country-by-name/{countryId}/{statename}");

            return await resp.ReadAsApiResponseAsync<WMSStatesCountriesReadDTO>(ct);
        }

        public async Task<ApiResponse<WMSStatesCountriesReadDTO>> GetStateCountryInformationByISOCodeAsync(int countryId, string isocode, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/StatesCountriesWMS/state-country-by-isocode/{countryId}/{isocode}");

            return await resp.ReadAsApiResponseAsync<WMSStatesCountriesReadDTO>(ct);
        }

        public Task<ApiResponse<bool>> UpdateStateCountryAsync(WMSStatesCountriesDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

      
    }
}

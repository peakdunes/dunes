using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.WMS.Masters.Cities
{
    public class CitiesWMSUIService: ICitiesWMSUIService
    {
        private readonly HttpClient _httpClient;
      


        public CitiesWMSUIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("DUNES_API");
        }

        public async Task<ApiResponse<bool>> AddCityAsync(WMSCitiesDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"api/CitiesWMS/create-country", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteCityAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WMSCitiesReadDTO>>> GetAllCitiesInformation(int countryid,string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CitiesWMS/all-cities/{countryid}");

            return await resp.ReadAsApiResponseAsync<List<WMSCitiesReadDTO>>(ct);
        }

        public Task<ApiResponse<WMSCitiesReadDTO>> GetCityInformationByIdAsync(int Id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSCitiesReadDTO>> GetCityInformationByIdentificationAsync(string countryid, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateCityAsync(WMSCitiesDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace DUNES.UI.Services.WMS.Masters.Locations
{
    public class LocationsWMSUIService : ILocationsWMSUIService
    {

        private readonly HttpClient _httpClient;


        public LocationsWMSUIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("DUNES_API");
        }

        public async Task<ApiResponse<bool>> CreateAsync(WMSLocationsDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"/api/LocationsWMS/wms-create-location", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public async Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var respapi = await _httpClient.GetAsync($"/api/LocationsWMS/wms-location-exists-by-name?name={Uri.EscapeDataString(name)}&excludeId={excludeId}", ct);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetActiveAsync(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var respapi = await _httpClient.GetAsync($"/api/LocationsWMS/wms-active-locations", ct);

            return await respapi.ReadAsApiResponseAsync<List<WMSLocationsDTO>>(ct);
        }

        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetAllAsync(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var respapi = await _httpClient.GetAsync($"/api/LocationsWMS/wms-all-locations", ct);

            return await respapi.ReadAsApiResponseAsync<List<WMSLocationsDTO>>(ct);
        }

        public async Task<ApiResponse<WMSLocationsDTO?>> GetByIdAsync(int id, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var respapi = await _httpClient.GetAsync($"/api/LocationsWMS/wms-location-by-id/{id}", ct);

            return await respapi.ReadAsApiResponseAsync<WMSLocationsDTO?>(ct);
        }

        public async Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var content = new StringContent(string.Empty);
            var respapi = await _httpClient.PutAsync($"/api/LocationsWMS/wms-set-active-location/{id}?isActive={isActive.ToString().ToLower()}", content,ct);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public async Task<ApiResponse<bool>> UpdateAsync(WMSLocationsDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"/api/LocationsWMS/wms-update-location", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }
    }
}

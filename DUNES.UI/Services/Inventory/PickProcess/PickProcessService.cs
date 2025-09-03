using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.Inventory.PickProcess
{
    public class PickProcessService : IPickProcessService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public PickProcessService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<ApiResponse<PickProcessDto>> GetPickProcessAllInfo(string DeliveryId, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CommonQueryPickProcessINV/pickprocess-info/{DeliveryId}");

            return await resp.ReadAsApiResponseAsync<PickProcessDto>(ct);

            //throw new NotImplementedException();
        }
    }
}

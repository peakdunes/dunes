using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.WebService
{
    public class WebServiceUIService : IWebServiceUIService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public WebServiceUIService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }
        /// <summary>

        public async Task<ApiResponse<bool>> UpsertHourlyAsync(MvcWebServiceHourlySummaryDto dto, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync(
                $"api/WebService/hourly/",
                content
            );

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }
    }
}

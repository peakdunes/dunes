using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.WMS.Masters.Companies
{
    public class CompaniesWMSUIService : ICompaniesWMSUIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;

        public CompaniesWMSUIService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }


        public async Task<ApiResponse<List<WMSCompaniesDTO>>> GetAllCompaniesInformation(string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSMaster/companynies-information");

            return await resp.ReadAsApiResponseAsync<List<WMSCompaniesDTO>>(ct);
        }
    }
}

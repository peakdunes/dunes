using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Infrastructure;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DUNES.UI.Services.Inventory.ASN
{
    public class ASNService : IASNService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public ASNService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }
              

        public async Task<ApiResponse<ASNWm>> GetAsnInfo(string asnNumber, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;
                                                   
            resp = await _httpClient.GetAsync($"/api/CommonQueryASNINV/asn-info/{asnNumber}");

            return await resp.ReadAsApiResponseAsync<ASNWm>(ct);
        }

    



     

      
     

      

    }
}

using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DUNES.UI.Services.Inventory.ASN
{
    public class ASNUIService : IASNUIService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public ASNUIService(IConfiguration config)
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

        public async Task<ApiResponse<ASNResponseDto>> ProcessASNTransaction(string asnNumber, ProcessAsnRequestTm objInvData, string trackingNumber,  string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(objInvData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var respapi = await _httpClient.PostAsync(
                $"/api/CommonQueryASNINV/asn-process/{asnNumber}/{trackingNumber}",
                content
            );
                       
            return await respapi.ReadAsApiResponseAsync<ASNResponseDto>(ct);
        }

      
    }
}

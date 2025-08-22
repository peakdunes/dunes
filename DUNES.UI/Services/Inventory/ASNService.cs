using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DUNES.UI.Services.Inventory
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


        public async Task<ApiResponse<ASNDto>> GetAsnInfo(string asnNumber, string token, CancellationToken ct)
        {


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryINV/asn-info/{asnNumber}");

            return await resp.ReadAsApiResponseAsync<ASNDto>(ct);


            ////se obtiene el string de la respuesta
            //var RespJsonString = await resp.Content.ReadAsStringAsync();

            //var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            ////se deserializa la respuesta al objeto tipo ApiReponse
            //ApiResponse<ASNDto>? result = JsonSerializer.Deserialize<ApiResponse<ASNDto>>(RespJsonString, opts);


            //return result!;

        }

        public async Task<ApiResponse<List<WMSClientCompanies>>> GetClientCompanies(string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/WmsCompanyclient/GetAll");

            return await resp.ReadAsApiResponseAsync<List<WMSClientCompanies>>(ct);

        }
    }
}

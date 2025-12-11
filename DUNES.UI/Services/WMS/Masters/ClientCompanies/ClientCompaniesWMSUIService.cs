using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.WMS.Masters.ClientCompanies
{
    public class ClientCompaniesWMSUIService : IClientCompaniesWMSUIService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public ClientCompaniesWMSUIService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<ApiResponse<bool>> AddClientCompanyAsync(WmsCompanyclientDto entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"/api/ClientCompaniesWMS/wms-create-client-company",content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteClientCompanyAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WmsCompanyclientDto>>> GetAllClientCompaniesInformation(string token ,CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/ClientCompaniesWMS/all-client-companies");

            return await resp.ReadAsApiResponseAsync<List<WmsCompanyclientDto>>(ct);
        }

        public async Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByIdAsync(int Id, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/ClientCompaniesWMS/wms-client-company-by-id/{Id}");

            return await resp.ReadAsApiResponseAsync<WmsCompanyclientDto>(ct);
        }

        public async Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByIdentificationAsync(string companyid, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/ClientCompaniesWMS/wms-client-company-by-identification/{companyid}");

            return await resp.ReadAsApiResponseAsync<WmsCompanyclientDto>(ct);
        }


        public async Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByNameAsync(string companyname, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/ClientCompaniesWMS/wms-client-company-by-name/{companyname}");

            return await resp.ReadAsApiResponseAsync<WmsCompanyclientDto>(ct);
        }


        public Task<ApiResponse<bool>> UpdateClientCompanyAsync(WmsCompanyclientDto entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Microsoft.Build.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Linq;

namespace DUNES.UI.Services.WMS.Masters.CompaniesClientDivision
{
    public class CompaniesClientDivisionWMSUIService : ICompaniesClientDivisionWMSUIService
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;


        /// <summary>
        /// contructor (DI)
        /// </summary>
        /// <param name="_config"></param>
        public CompaniesClientDivisionWMSUIService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<ApiResponse<bool>> AddClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"/api/CompanyClientDivisionWMS/wms-create-client-company-division", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteClientCompanyDivisionAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformation(string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformationByCompanyClient(int companyclientid, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CompanyClientDivisionWMS/all-client-company-divisions-by-company/{companyclientid}");
             return await resp.ReadAsApiResponseAsync<List<WMSCompanyClientDivisionReadDTO>>(ct);
        }

        public Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionById(int divisionId, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionByNameAsync(int companyClientId, string divisionname, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CompanyClientDivisionWMS/wms-client-company-division-by-name/{companyClientId}/{divisionname}");

              return await resp.ReadAsApiResponseAsync<WMSCompanyClientDivisionReadDTO>(ct);
        }

        public Task<ApiResponse<bool>> UpdateClientCompanyDivisionAsync(WMSCompanyClientDivisionDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Infrastructure;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace DUNES.UI.Services.WMS.Masters.CompaniesContract
{
    public class CompaniesClientContractWMSUIService : ICompaniesClientContractWMSUIService
    {


        private readonly HttpClient _httpClient;
      

        public CompaniesClientContractWMSUIService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("DUNES_API");
        }

        public async Task<ApiResponse<bool>> AddClientCompanyContractAsync(WMSCompaniesContractDTO entity, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(entity);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var respapi = await _httpClient.PostAsync($"/api/LocationsWMS/wms-create-location", content);

            return await respapi.ReadAsApiResponseAsync<bool>(ct);
        }

        public Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

           

            var respapi = await _httpClient.GetAsync($"/api/CompaniesContractWMS/all-client-contracts");

            return await respapi.ReadAsApiResponseAsync<List<WMSCompaniesContractReadDTO>>(ct);
        }

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByIdAsync(int Id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetClientCompanyInformationContractByCompanyIdAsync(int companyclientid, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(string contractcode, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberCompanyIdAsync(int companyclientid, string contractcode, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(WMSCompaniesContractDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

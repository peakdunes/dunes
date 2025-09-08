using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Infrastructure;
using System.Net.Http.Headers;

namespace DUNES.UI.Services.Inventory.Common
{
    public class CommonINVService: ICommonINVService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _config;


        public CommonINVService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config["ApiSettings:BaseUrl"]!;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };
        }

        public async Task<ApiResponse<List<WMSBinsDto>>> GetAllActiveBinsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/wms-act-bins/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSBinsDto>>(ct);
        }

  
        public async Task<ApiResponse<List<WMSClientCompaniesDto>>> GetClientCompanies(string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/WmsCompanyclient/GetAll");

            return await resp.ReadAsApiResponseAsync<List<WMSClientCompaniesDto>>(ct);

        }



        public async Task<ApiResponse<List<WMSConceptsDto>>> GetAllActiveConceptsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-transaction-concepts/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSConceptsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSInputTransactionsDto>>> GetAllActiveInputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-input-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSInputTransactionsDto>>(ct);
        }

        public async Task<ApiResponse<List<InventoryTypeDto>>> GetAllActiveInventoryTypes(string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/TzebB2bInventoryType/GetAll");

            return await resp.ReadAsApiResponseAsync<List<InventoryTypeDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSInventoryTypeDto>>> GetAllActiveWmsInventoryTypes(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/inventorytype/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSInventoryTypeDto>>(ct);
        }
        public async Task<ApiResponse<List<itemstatusDto>>> GetAllActiveItemStatus(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-itemstatus/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<itemstatusDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItem(int companyid, string companyClient, string parnumber, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/inventoryByPartNumber/{companyid}/{companyClient}/{parnumber}");

            return await resp.ReadAsApiResponseAsync<List<WMSInventoryDetailByPartNumberDto>>(ct);
        }


        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItemInventoryType(int companyid, string companyClient, string parnumber, int typeid, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/inventoryByPartNumberInvType/{companyid}/{companyClient}/{parnumber}/{typeid}");

            return await resp.ReadAsApiResponseAsync<List<WMSInventoryDetailByPartNumberDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSItemByBinsDto>>> GetItemBinsDistribution(int companyid, string companyClient, string parnumber, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/itemBinDistribution/{companyid}/{companyClient}/{parnumber}");

            return await resp.ReadAsApiResponseAsync<List<WMSItemByBinsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSInputTransactionsDto>>> GetAllActiveOutputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-output-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSInputTransactionsDto>>(ct);
        }
    }
}

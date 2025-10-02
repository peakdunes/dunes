using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.UI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Transactions;

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
        /// <summary>
        /// Get all information (Header and Detail) for a pick process number
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PickProcessRequestDto>> GetPickProcessAllInfo(string DeliveryId, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/PickProcessINV/pickprocess-info/{DeliveryId}");

            return await resp.ReadAsApiResponseAsync<PickProcessRequestDto>(ct);

            //throw new NotImplementedException();
        }

       

        public async Task<ApiResponse<WMSTransactionTm>> GetAllTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;
                                                
            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/all-transactions/{companyid}/{companyClient}/{DocumentNumber}");

            return await resp.ReadAsApiResponseAsync<WMSTransactionTm>(ct);
        }

        public async Task<ApiResponse<TorderRepairTm>> GetAllTablesOrderRepairCreatedByPickProcessAsync(string ConsignRequestId, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/PickProcessINV/repair-info/{ConsignRequestId}");

            return await resp.ReadAsApiResponseAsync<TorderRepairTm>(ct);
        }

        public async Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(string DeliveryId, NewInventoryTransactionTm objInvData, string lpnid, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            var json = JsonConvert.SerializeObject(objInvData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

          


            //resp = await _httpClient.PostAsync($"/api/PickProcessINV/create-pickprocess-transaction/{DeliveryId}/{objInvData}/{lpnid}/{token}");

            var respapi = await _httpClient.PostAsync(
                $"api/PickProcessINV/create-pickprocess-transaction/{DeliveryId}/{lpnid}",
                content
            );

            return await respapi.ReadAsApiResponseAsync<PickProcessResponseDto>(ct);


        }
    }
}

using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Infrastructure;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Transactions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

  
        public async Task<ApiResponse<List<WMSLocationclientsDTO>>> GetAllActiveClientCompaniesByLocation( int companyid, int locationid, string token, CancellationToken ct)
        {

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSMaster/companyclients-active-by-locations/{companyid}/{locationid}");

            return await resp.ReadAsApiResponseAsync<List<WMSLocationclientsDTO>>(ct);

        }



        public async Task<ApiResponse<List<WMSConceptsDto>>> GetAllActiveConceptsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-transaction-concepts/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSConceptsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveInputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-input-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSTransactionsDto>>(ct);
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


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-inventorytype/{companyid}/{companyClient}");

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


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/onHand-InvByPartNumber/{companyid}/{companyClient}/{parnumber}");

            return await resp.ReadAsApiResponseAsync<List<WMSInventoryDetailByPartNumberDto>>(ct);
        }


        public async Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItemInventoryType(int companyid, string companyClient, string parnumber, int typeid, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/onHand-InvByPartNumber-Type/{companyid}/{companyClient}/{parnumber}/{typeid}");

            return await resp.ReadAsApiResponseAsync<List<WMSInventoryDetailByPartNumberDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSItemByBinsDto>>> GetItemBinsDistribution(int companyid, string companyClient, string parnumber, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/itemBinDistribution/{companyid}/{companyClient}/{parnumber}");

            return await resp.ReadAsApiResponseAsync<List<WMSItemByBinsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveOutputTransactionsByCompanyClient(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-output-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSTransactionsDto>>(ct);
        }

        public async Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(string companyclient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryINV/division-by-companyclient/{companyclient}");

            return await resp.ReadAsApiResponseAsync<List<TdivisionCompanyDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsInputType(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-transfer-input-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSTransactionsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsInputType(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/all-transfer-input-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSTransactionsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllActiveTransferTransactionsOutputType(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/active-transfer-output-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSTransactionsDto>>(ct);
        }

        public async Task<ApiResponse<List<WMSTransactionsDto>>> GetAllTransferTransactionsOutputType(int companyid, string companyClient, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/all-transfer-output-transaction/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSTransactionsDto>>(ct);
        }
        
        public async Task<ApiResponse<WMSTransactionsDto>> GetTransactionsTypeById(int companyid, string companyClient, int id, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/transaction-by-id/{companyid}/{companyClient}/{id}");

            return await resp.ReadAsApiResponseAsync<WMSTransactionsDto>(ct);
        }
        /// <summary>
        /// Get All Active Locations for a company
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<List<WMSLocationsDTO>>> GetAllActiveLocationsByCompany(int companyid, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSMaster/company-locations/{companyid}");

            return await resp.ReadAsApiResponseAsync<List<WMSLocationsDTO>>(ct);
        }

        public async Task<ApiResponse<List<WMSWarehouseorganizationDto>>> GetAllWareHouseOrganizationByCompanyClient(int companyid, string companyClient, string token ,CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSMaster/companyClient-warehouse-organization/{companyid}/{companyClient}");

            return await resp.ReadAsApiResponseAsync<List<WMSWarehouseorganizationDto>>(ct);
        }

        public async Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByName(string companyname, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;
                                                           

            resp = await _httpClient.GetAsync($"/api/WmsCompanyclient/client-company-information-by-name/{companyname}");

            return await resp.ReadAsApiResponseAsync<WmsCompanyclientDto>(ct);

        }

        public async Task<ApiResponse<TzebB2bMasterPartDefinitionDto>> GetByPartNumber(string partnumber, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;


            resp = await _httpClient.GetAsync($"/api/MasterInventory/GetByPartNumber/{partnumber}");

            return await resp.ReadAsApiResponseAsync<TzebB2bMasterPartDefinitionDto>(ct);
        }

        public async Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocumentStartDate(string DocumentNumber, DateTime startDate, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CommonQueryINV/inv-transactions-by-document-date/{DocumentNumber}?startDate={startDate:O}");

           
            return await resp.ReadAsApiResponseAsync<List<TzebB2bReplacementPartsInventoryLogDto>>(ct);
        }

        /// <summary>
        /// Get all input - output call for a ASN - Pick Process Document
        /// </summary>
        /// <param name="DeliveryId"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PickProcessCallsReadDto>> GetAllCalls(string DocumentId, string processtype, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;



            resp = await _httpClient.GetAsync($"/api/CommonQueryINV/all-calls/{DocumentId}/{processtype}");

            return await resp.ReadAsApiResponseAsync<PickProcessCallsReadDto>(ct);
        }

        public async Task<ApiResponse<WMSTransactionTm>> GetAllWMSTransactionByDocumentNumber(int companyid, string companyClient, string DocumentNumber, string token, CancellationToken ct)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage resp;

            resp = await _httpClient.GetAsync($"/api/CommonQueryWMSINV/all-transactions/{companyid}/{companyClient}/{DocumentNumber}");

            return await resp.ReadAsApiResponseAsync<WMSTransactionTm>(ct);
        }
    }
}

using DUNES.API.ReadModels.Inventory;
using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.Shared.WiewModels.Inventory;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Inventory.Common
{
    public class CommonINVUIService
        : UIApiServiceBase, ICommonINVUIService
    {
        public CommonINVUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<List<WMSBinsCreateDto>>> GetAllActiveBinsByCompanyClient(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSBinsCreateDto>>(
                $"/api/CommonQueryWMSINV/wms-act-bins/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSLocationclientsDTO>>> GetAllActiveClientCompaniesByLocation(
            int companyid,
            int locationid,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSLocationclientsDTO>>(
                $"/api/CommonQueryWMSMaster/companyclients-active-by-locations/{companyid}/{locationid}",
                token,
                ct);

        public Task<ApiResponse<List<WMSConceptsDto>>> GetAllActiveConceptsByCompanyClient(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)

            => GetApiAsync<List<WMSConceptsDto>>(
                $"/api/CommonQueryWMSINV/active-transaction-concepts/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactionTypesUpdateDTO>>> GetAllActiveInputTransactionsByCompanyClient(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypesUpdateDTO>>(
                $"/api/CommonQueryWMSINV/active-input-transaction/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<InventoryTypeDto>>> GetAllActiveInventoryTypes(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<InventoryTypeDto>>(
                "/api/TzebB2bInventoryType/GetAll",
                token,
                ct);

        public Task<ApiResponse<List<WMSInventoryTypeDto>>> GetAllActiveWmsInventoryTypes(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSInventoryTypeDto>>(
                $"/api/CommonQueryWMSINV/active-inventorytype/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<itemstatusDto>>> GetAllActiveItemStatus(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<itemstatusDto>>(
                $"/api/CommonQueryWMSINV/active-itemstatus/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItem(
            int companyid,
            string companyClient,
            string parnumber,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSInventoryDetailByPartNumberDto>>(
                $"/api/CommonQueryWMSINV/onHand-InvByPartNumber/{companyid}/{companyClient}/{parnumber}",
                token,
                ct);

        public Task<ApiResponse<List<WMSInventoryDetailByPartNumberDto>>> GetInventoryByItemInventoryType(
            int companyid,
            string companyClient,
            string parnumber,
            int typeid,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSInventoryDetailByPartNumberDto>>(
                $"/api/CommonQueryWMSINV/onHand-InvByPartNumber-Type/{companyid}/{companyClient}/{parnumber}/{typeid}",
                token,
                ct);

        public Task<ApiResponse<List<WMSItemByBinsDto>>> GetItemBinsDistribution(
            int companyid,
            string companyClient,
            string parnumber,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSItemByBinsDto>>(
                $"/api/CommonQueryWMSINV/itemBinDistribution/{companyid}/{companyClient}/{parnumber}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactionTypesUpdateDTO>>> GetAllActiveOutputTransactionsByCompanyClient(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypesUpdateDTO>>(
                $"/api/CommonQueryWMSINV/active-output-transaction/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<TdivisionCompanyDto>>> GetDivisionByCompanyClient(
            string companyclient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<TdivisionCompanyDto>>(
                $"/api/CommonQueryINV/division-by-companyclient/{companyclient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactionTypesUpdateDTO>>> GetAllActiveTransferTransactionsInputType(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypesUpdateDTO>>(
                $"/api/CommonQueryWMSINV/active-transfer-input-transaction/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactionTypesUpdateDTO>>> GetAllTransferTransactionsInputType(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypesUpdateDTO>>(
                $"/api/CommonQueryWMSINV/all-transfer-input-transaction/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactionTypesUpdateDTO>>> GetAllActiveTransferTransactionsOutputType(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypesUpdateDTO>>(
                $"/api/CommonQueryWMSINV/active-transfer-output-transaction/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<List<WMSTransactionTypesUpdateDTO>>> GetAllTransferTransactionsOutputType(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypesUpdateDTO>>(
                $"/api/CommonQueryWMSINV/all-transfer-output-transaction/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<WMSTransactionTypesUpdateDTO>> GetTransactionsTypeById(
            int companyid,
            string companyClient,
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSTransactionTypesUpdateDTO>(
                $"/api/CommonQueryWMSINV/transaction-by-id/{companyid}/{companyClient}/{id}",
                token,
                ct);

        public Task<ApiResponse<List<WMSLocationsUpdateDTO>>> GetAllActiveLocationsByCompany(
            int companyid,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSLocationsUpdateDTO>>(
                $"/api/CommonQueryWMSMaster/company-locations/{companyid}",
                token,
                ct);

        public Task<ApiResponse<List<WMSWarehouseorganizationDto>>> GetAllWareHouseOrganizationByCompanyClient(
            int companyid,
            string companyClient,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSWarehouseorganizationDto>>(
                $"/api/CommonQueryWMSMaster/companyClient-warehouse-organization/{companyid}/{companyClient}",
                token,
                ct);

        public Task<ApiResponse<WmsCompanyclientDto>> GetClientCompanyInformationByName(
            string companyname,
            string token,
            CancellationToken ct)
            => GetApiAsync<WmsCompanyclientDto>(
                $"/api/WmsCompanyclient/client-company-information-by-name/{companyname}",
                token,
                ct);

        public Task<ApiResponse<TzebB2bMasterPartDefinitionDto>> GetByPartNumber(
            string partnumber,
            string token,
            CancellationToken ct)
            => GetApiAsync<TzebB2bMasterPartDefinitionDto>(
                $"/api/MasterInventory/GetByPartNumber/{partnumber}",
                token,
                ct);

        public Task<ApiResponse<List<TzebB2bReplacementPartsInventoryLogDto>>> GetAllInventoryTransactionsByDocumentStartDate(
            string DocumentNumber,
            DateTime startDate,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<TzebB2bReplacementPartsInventoryLogDto>>(
                $"/api/CommonQueryINV/inv-transactions-by-document-date/{DocumentNumber}?startDate={startDate:O}",
                token,
                ct);

        public Task<ApiResponse<PickProcessCallsReadDto>> GetAllCalls(
            string DocumentId,
            string processtype,
            string token,
            CancellationToken ct)
            => GetApiAsync<PickProcessCallsReadDto>(
                $"/api/CommonQueryINV/all-calls/{DocumentId}/{processtype}",
                token,
                ct);

        public Task<ApiResponse<WMSTransactionTm>> GetAllWMSTransactionByDocumentNumber(
            int companyid,
            string companyClient,
            string DocumentNumber,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSTransactionTm>(
                $"/api/CommonQueryWMSINV/all-transactions/{companyid}/{companyClient}/{DocumentNumber}",
                token,
                ct);
    }
}

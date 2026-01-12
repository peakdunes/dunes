using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.Inventory.PickProcess
{
    public class PickProcessUIService
        : UIApiServiceBase, IPickProcessUIService
    {
        public PickProcessUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Get all information (Header and Detail) for a pick process number
        /// </summary>
        public Task<ApiResponse<PickProcessRequestDto>> GetPickProcessAllInfo(
            string DeliveryId,
            string token,
            CancellationToken ct)
            => GetApiAsync<PickProcessRequestDto>(
                $"/api/PickProcessINV/pickprocess-info/{DeliveryId}",
                token,
                ct);

        public Task<ApiResponse<WMSTransactionTm>> GetAllTransactionByDocumentNumber(
            int companyid,
            string companyClient,
            string DocumentNumber,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSTransactionTm>(
                $"/api/CommonQueryWMSINV/all-transactions/{companyid}/{companyClient}/{DocumentNumber}",
                token,
                ct);

        public Task<ApiResponse<TorderRepairTm>> GetAllTablesOrderRepairCreatedByPickProcessAsync(
            string ConsignRequestId,
            string token,
            CancellationToken ct)
            => GetApiAsync<TorderRepairTm>(
                $"/api/PickProcessINV/repair-info/{ConsignRequestId}",
                token,
                ct);

        public Task<ApiResponse<PickProcessResponseDto>> CreatePickProccessTransaction(
            string DeliveryId,
            NewInventoryTransactionTm objInvData,
            string lpnid,
            string token,
            CancellationToken ct)
            => PostApiAsync<PickProcessResponseDto, NewInventoryTransactionTm>(
                $"/api/PickProcessINV/create-pickprocess-transaction/{DeliveryId}/{lpnid}",
                objInvData,
                token,
                ct);

        public Task<ApiResponse<List<PickProcessHdrDto>>> GetAllPickProcessByStartDate(
            DateTime dateSearch,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<PickProcessHdrDto>>(
                $"/api/PickProcessINV/list-pickprocess-startdate/{dateSearch:yyyy-MM-dd}",
                token,
                ct);
    }
}

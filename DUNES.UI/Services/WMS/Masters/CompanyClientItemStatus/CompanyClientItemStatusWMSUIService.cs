using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// UI service implementation for client-specific Item Status configuration.
    /// </summary>
    public class CompanyClientItemStatusWMSUIService
        : UIApiServiceBase, ICompanyClientItemStatusWMSUIService
    {
        public CompanyClientItemStatusWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientItemStatusReadDTO>>(
                "/api/wms/masters/company-client/item-statuses/GetAll",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSCompanyClientItemStatusReadDTO>(
                $"/api/wms/masters/company-client/item-statuses/GetById/{id}",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<WMSCompanyClientItemStatusReadDTO, WMSCompanyClientItemStatusCreateDTO>(
                "/api/wms/masters/company-client/item-statuses/Create",
                dto,
                token,
                ct);

        public Task<ApiResponse<bool>> UpdateAsync(
            WMSCompanyClientItemStatusUpdateDTO dto,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, WMSCompanyClientItemStatusUpdateDTO>(
                "/api/wms/masters/company-client/item-statuses/Update",
                dto,
                token,
                ct);

        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, object>(
                $"/api/wms/masters/company-client/item-statuses/SetActive/{id}?isActive={isActive.ToString().ToLower()}",
                new { },
                token,
                ct);
    }
}

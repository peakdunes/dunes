using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientInventoryType
{
    /// <summary>
    /// UI service implementation for client-specific Inventory Types configuration.
    /// </summary>
    public class CompanyClientInventoryTypeWMSUIService
        : UIApiServiceBase, ICompanyClientInventoryTypeWMSUIService
    {
        public CompanyClientInventoryTypeWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientInventoryTypeReadDTO>>(
                "/api/wms/masters/company-client/inventory-types/GetAll",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSCompanyClientInventoryTypeReadDTO>(
                $"/api/wms/masters/company-client/inventory-types/GetById/{id}",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<WMSCompanyClientInventoryTypeReadDTO, WMSCompanyClientInventoryTypeCreateDTO>(
                "/api/wms/masters/company-client/inventory-types/Create",
                dto,
                token,
                ct);

        public Task<ApiResponse<bool>> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, WMSCompanyClientInventoryTypeUpdateDTO>(
                "/api/wms/masters/company-client/inventory-types/Update",
                dto,
                token,
                ct);

        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, object>(
                $"/api/wms/masters/company-client/inventory-types/SetActive/{id}?isActive={isActive.ToString().ToLower()}",
                new { },
                token,
                ct);
    }
}

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
                "/api/wms/masters/company-client/type-inventory/GetAll",
                token,
                ct);

       
        /// <inheritdoc/>
        public Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientInventoryTypeReadDTO>>(
                "/api/wms/masters/company-client/type-inventory/GetEnabled",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSCompanyClientInventoryTypeReadDTO>(
                $"/api/wms/masters/company-client/type-inventory/GetById/{id}",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<WMSCompanyClientInventoryTypeReadDTO, WMSCompanyClientInventoryTypeCreateDTO>(
                "/api/wms/masters/company-client/type-inventory/Create",
                dto,
                token,
                ct);

        public Task<ApiResponse<bool>> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO dto,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, WMSCompanyClientInventoryTypeUpdateDTO>(
                "/api/wms/masters/company-client/type-inventory/Update",
                dto,
                token,
                ct);

        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PatchApiAsync<bool>(
                $"/api/wms/masters/company-client/type-inventory/SetActive/{id}?isActive={isActive.ToString().ToLower()}",
                token,
                ct);


        public Task<ApiResponse<bool>> DeleteAsync(
         int id,
         string token,
         CancellationToken ct)
             => DeleteApiAsync<bool>(

                      $"/api/wms/masters/company-client/type-inventory/Delete/{id}",
                     token,
                     ct);
    }
}

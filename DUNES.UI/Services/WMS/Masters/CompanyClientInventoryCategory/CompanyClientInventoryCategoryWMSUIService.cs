using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// UI service implementation for managing inventory category enablement for the current client.
    /// CompanyId / CompanyClientId are resolved from token by the API.
    /// </summary>
    public class CompanyClientInventoryCategoryWMSUIService
        : UIApiServiceBase, ICompanyClientInventoryCategoryWMSUIService
    {
        public CompanyClientInventoryCategoryWMSUIService(IHttpClientFactory factory)
            : base(factory) { }


        /// <inheritdoc/>
        public Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientInventoryCategoryReadDTO>>(
                "/api/wms/masters/company-client/inventory-categories/GetAll",
                token,
                ct);

        /// <inheritdoc/>
        public Task<ApiResponse<List<WMSCompanyClientInventoryCategoryReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientInventoryCategoryReadDTO>>(
                "/api/wms/masters/company-client/inventory-categories/GetEnabled",
                token,
                ct);

        /// <inheritdoc/>
        public Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSCompanyClientInventoryCategoryReadDTO>(
                $"/api/wms/masters/company-client/inventory-categories/GetById/{id}",
                token,
                ct);

        /// <inheritdoc/>
        public Task<ApiResponse<WMSCompanyClientInventoryCategoryReadDTO>> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<WMSCompanyClientInventoryCategoryReadDTO, WMSCompanyClientInventoryCategoryCreateDTO>(
                "/api/wms/masters/company-client/inventory-categories/Create",
                dto,
                token,
                ct);

        /// <inheritdoc/>
        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PatchApiAsync<bool>(
                $"/api/wms/masters/company-client/inventory-categories/SetActive/{id}?isActive={isActive.ToString().ToLower()}",
                token,
                ct);

        /// <inheritdoc/>
        public Task<ApiResponse<bool>> SetEnabledSetAsync(
            List<int> inventoryCategoryIds,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, List<int>>(
                "/api/wms/masters/company-client/inventory-categories/SetEnabledSet",
                inventoryCategoryIds ?? new List<int>(),
                token,
                ct);
    }
}

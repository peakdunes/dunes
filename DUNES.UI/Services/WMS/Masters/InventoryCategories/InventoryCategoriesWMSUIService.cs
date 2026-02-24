using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;
using System.Xml.Linq;

namespace DUNES.UI.Services.WMS.Masters.InventoryCategories
{
    public class InventoryCategoriesWMSUIService : UIApiServiceBase, IInventoryCategoriesWMSUIService
    {

        public InventoryCategoriesWMSUIService(IHttpClientFactory factory)
           : base(factory)
        {
        }

        /// <summary>
        /// add new inventory category
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="token"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<ApiResponse<bool>> CreateAsync(WMSInventorycategoriesCreateDTO entity, string token, CancellationToken ct)
                    => PostApiAsync<bool, WMSInventorycategoriesCreateDTO>(
                "/api/LocationsWMS/wms-create-location",
                entity,
                token,
                ct);
        

        public Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct)
       
            => GetApiAsync<bool>(
                $"/api/LocationsWMS/wms-location-exists-by-name?name={Uri.EscapeDataString(name)}&excludeId={excludeId}",
                token,
                ct);
        

        public Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetActiveAsync(string token, CancellationToken ct)
        
            => GetApiAsync<List<WMSInventorycategoriesReadDTO>>(
                $"/api/wms/masters/inventory-categories/GetAll",
                token,
                ct);
        

        public Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetAllAsync(string token, CancellationToken ct)
        => GetApiAsync<List<WMSInventorycategoriesReadDTO>>(
                $"/api/wms/masters/inventory-categories/GetAll",
                token,
                ct);

        public Task<ApiResponse<WMSInventorycategoriesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateAsync(WMSInventorycategoriesUpdateDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

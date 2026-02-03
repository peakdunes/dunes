using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.Locations
{
    public class LocationsWMSUIService
        : UIApiServiceBase, ILocationsWMSUIService
    {
        public LocationsWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> CreateAsync(
            WMSLocationsUpdateDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSLocationsUpdateDTO>(
                "/api/LocationsWMS/wms-create-location",
                entity,
                token,
                ct);

        public Task<ApiResponse<bool>> ExistsByNameAsync(
            string name,
            int? excludeId,
            string token,
            CancellationToken ct)
            => GetApiAsync<bool>(
                $"/api/LocationsWMS/wms-location-exists-by-name?name={Uri.EscapeDataString(name)}&excludeId={excludeId}",
                token,
                ct);

        public Task<ApiResponse<List<WMSLocationsUpdateDTO>>> GetActiveAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSLocationsUpdateDTO>>(
                "/api/LocationsWMS/wms-active-locations",
                token,
                ct);

        public Task<ApiResponse<List<WMSLocationsUpdateDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSLocationsUpdateDTO>>(
                "/api/LocationsWMS/wms-all-locations",
                token,
                ct);

        public Task<ApiResponse<WMSLocationsUpdateDTO?>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSLocationsUpdateDTO?>(
                $"/api/LocationsWMS/wms-location-by-id/{id}",
                token,
                ct);

        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, object>(
                $"/api/LocationsWMS/wms-set-active-location/{id}?isActive={isActive.ToString().ToLower()}",
                body: new { }, // PUT sin payload real
                token,
                ct);

        public Task<ApiResponse<bool>> UpdateAsync(
            WMSLocationsUpdateDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSLocationsUpdateDTO>(
                "/api/LocationsWMS/wms-update-location",
                entity,
                token,
                ct);
    }
}

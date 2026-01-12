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
            WMSLocationsDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSLocationsDTO>(
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

        public Task<ApiResponse<List<WMSLocationsDTO>>> GetActiveAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSLocationsDTO>>(
                "/api/LocationsWMS/wms-active-locations",
                token,
                ct);

        public Task<ApiResponse<List<WMSLocationsDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSLocationsDTO>>(
                "/api/LocationsWMS/wms-all-locations",
                token,
                ct);

        public Task<ApiResponse<WMSLocationsDTO?>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSLocationsDTO?>(
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
            WMSLocationsDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSLocationsDTO>(
                "/api/LocationsWMS/wms-update-location",
                entity,
                token,
                ct);
    }
}

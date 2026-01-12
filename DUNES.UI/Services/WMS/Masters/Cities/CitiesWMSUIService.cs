using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.Cities
{
    public class CitiesWMSUIService
        : UIApiServiceBase, ICitiesWMSUIService
    {
        public CitiesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> AddCityAsync(
            WMSCitiesDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSCitiesDTO>(
                "/api/CitiesWMS/create-country",
                entity,
                token,
                ct);

        public Task<ApiResponse<bool>> DeleteCityAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<List<WMSCitiesReadDTO>>> GetAllCitiesInformation(
            int countryid,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCitiesReadDTO>>(
                $"/api/CitiesWMS/all-cities/{countryid}",
                token,
                ct);

        public Task<ApiResponse<WMSCitiesReadDTO>> GetCityInformationByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<WMSCitiesReadDTO>> GetCityInformationByIdentificationAsync(
            string countryid,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<bool>> UpdateCityAsync(
            WMSCitiesDTO entity,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();
    }
}

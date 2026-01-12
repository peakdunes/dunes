using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.Countries
{
    public class CountriesWMSUIService
        : UIApiServiceBase, ICountriesWMSUIService
    {
        public CountriesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> AddCountryAsync(
            WMSCountriesDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSCountriesDTO>(
                "/api/CountriesWMS/create-country",
                entity,
                token,
                ct);

        public Task<ApiResponse<List<WMSCountriesDTO>>> GetAllCountriesInformation(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCountriesDTO>>(
                "/api/CountriesWMS/all-countries",
                token,
                ct);

        public Task<ApiResponse<bool>> GetCountryInformationByIdentificationAsync(
            string countryid,
            int? excludeId,
            string token,
            CancellationToken ct)
        {
            var nameEncoded = Uri.EscapeDataString(countryid);

            return GetApiAsync<bool>(
                $"/api/CountriesWMS/country-by-name?name={nameEncoded}&excludeId={excludeId}",
                token,
                ct);
        }

        public Task<ApiResponse<bool>> DeleteCountryAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<WMSCountriesDTO>> GetCountryInformationByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<bool>> UpdateCountryAsync(
            WMSCountriesDTO entity,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();
    }
}

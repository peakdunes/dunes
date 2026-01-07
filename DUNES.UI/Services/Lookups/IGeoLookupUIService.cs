using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DUNES.UI.Services.Lookups
{

    /// <summary>
    /// load Countries, States, Cities
    /// </summary>
    public interface IGeoLookupUIService
    {
        Task<ApiResponse<List<WMSCountriesDTO>>> GetCountriesAsync(string token, CancellationToken ct);

        Task<ApiResponse<List<WMSStatesCountriesDTO>>> GetStatesByCountryAsync(int countryId, string token, CancellationToken ct);

        Task<ApiResponse<List<WMSCitiesDTO>>> GetCitiesByStateAsync(int stateId, string token, CancellationToken ct);

    }
}

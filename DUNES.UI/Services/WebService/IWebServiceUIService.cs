using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;
using DUNES.Shared.TemporalModels;

namespace DUNES.UI.Services.WebService
{
    public interface IWebServiceUIService
    {


        Task<ApiResponse<bool>> UpsertHourlyAsync(MvcWebServiceHourlySummaryDto dto, string token, CancellationToken ct);

       

    }
}

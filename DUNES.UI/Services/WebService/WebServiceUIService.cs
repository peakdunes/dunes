using DUNES.Shared.DTOs.WebService;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WebService
{
    public class WebServiceUIService
        : UIApiServiceBase, IWebServiceUIService
    {
        public WebServiceUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> UpsertHourlyAsync(
            MvcWebServiceHourlySummaryDto dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, MvcWebServiceHourlySummaryDto>(
                "/api/WebService/hourly/",
                dto,
                token,
                ct);
    }
}

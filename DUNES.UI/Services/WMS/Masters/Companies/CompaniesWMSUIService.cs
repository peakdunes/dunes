using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.Companies
{
    public class CompaniesWMSUIService
        : UIApiServiceBase, ICompaniesWMSUIService
    {
        public CompaniesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<List<WMSCompaniesDTO>>> GetAllCompaniesInformation(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompaniesDTO>>(
                "/api/CommonQueryWMSMaster/companynies-information",
                token,
                ct);
    }
}

using DUNES.Shared.DTOs.Inventory;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.Inventory
{
    public interface IASNService
    {

        Task<ApiResponse<ASNDto>> GetAsnInfo(string asnNumber, string token,CancellationToken ct);

        Task<ApiResponse<List<WMSClientCompanies>>> GetClientCompanies(string token, CancellationToken ct);

        Task<ApiResponse<List<WMSBins>>> GetAllActiveBinsByCompanyClient (int companyid, string companyClient, string token, CancellationToken ct);
    }
}

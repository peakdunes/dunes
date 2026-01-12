using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.CompaniesContract
{
    public class CompaniesClientContractWMSUIService
        : UIApiServiceBase, ICompaniesClientContractWMSUIService
    {
        public CompaniesClientContractWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> AddClientCompanyContractAsync(
            WMSCompaniesContractDTO entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WMSCompaniesContractDTO>(
                "/api/LocationsWMS/wms-create-location",
                entity,
                token,
                ct);

        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompaniesContractReadDTO>>(
                "/api/CompaniesContractWMS/all-client-contracts",
                token,
                ct);

        public Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetClientCompanyInformationContractByCompanyIdAsync(
            int companyclientid,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(
            string contractcode,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberCompanyIdAsync(
            int companyclientid,
            string contractcode,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(
            WMSCompaniesContractDTO entity,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();
    }
}

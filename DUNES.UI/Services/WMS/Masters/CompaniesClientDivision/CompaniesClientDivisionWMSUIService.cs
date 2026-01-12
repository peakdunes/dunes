using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.CompaniesClientDivision
{
    public class CompaniesClientDivisionWMSUIService
        : UIApiServiceBase, ICompaniesClientDivisionWMSUIService
    {
        public CompaniesClientDivisionWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> AddClientCompanyDivisionAsync(
                WMSCompanyClientDivisionDTO entity,
                string token,
                CancellationToken ct)
                => PostApiAsync<bool, WMSCompanyClientDivisionDTO>(
                    "/api/wms-create-client-company-division",
                    entity,
                    token,
                    ct);

        public Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformationByCompanyClient(
            int companyclientid,
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSCompanyClientDivisionReadDTO>>(
                $"/api/CompanyClientDivisionWMS/all-client-company-divisions-by-company/{companyclientid}",
                token,
                ct);

        public Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionByNameAsync(
            int companyClientId,
            string divisionname,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSCompanyClientDivisionReadDTO?>(
                $"/api/CompanyClientDivisionWMS/wms-client-company-division-by-name/{companyClientId}/{divisionname}",
                token,
                ct);

        // Pendientes (se dejan explícitos)
        public Task<ApiResponse<bool>> DeleteClientCompanyDivisionAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<List<WMSCompanyClientDivisionReadDTO>>> GetAllCompaniesClientDivisionInformation(
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<WMSCompanyClientDivisionReadDTO?>> GetCompanyClientDivisionById(
            int divisionId,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<bool>> UpdateClientCompanyDivisionAsync(
            WMSCompanyClientDivisionDTO entity,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();
    }
}

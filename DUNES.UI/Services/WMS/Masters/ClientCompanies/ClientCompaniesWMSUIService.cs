using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.ClientCompanies
{
    public class ClientCompaniesWMSUIService
        : UIApiServiceBase, IClientCompaniesWMSUIService
    {
        public ClientCompaniesWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        public Task<ApiResponse<bool>> AddClientCompanyAsync(
            WmsCompanyclientDto entity,
            string token,
            CancellationToken ct)
            => PostApiAsync<bool, WmsCompanyclientDto>(
                "/api/ClientCompaniesWMS/wms-create-client-company",
                entity,
                token,
                ct);

        public Task<ApiResponse<bool>> DeleteClientCompanyAsync(
            int id,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();

        public Task<ApiResponse<List<WMSClientCompaniesReadDTO>>> GetAllClientCompaniesInformation(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSClientCompaniesReadDTO>>(
                "/api/ClientCompaniesWMS/all-client-companies",
                token,
                ct);

        public Task<ApiResponse<WMSClientCompaniesReadDTO>> GetClientCompanyInformationByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSClientCompaniesReadDTO>(
                $"/api/ClientCompaniesWMS/wms-client-company-by-id/{id}",
                token,
                ct);

        public Task<ApiResponse<WMSClientCompaniesReadDTO>> GetClientCompanyInformationByIdentificationAsync(
            string companyid,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSClientCompaniesReadDTO>(
                $"/api/ClientCompaniesWMS/wms-client-company-by-identification/{companyid}",
                token,
                ct);

        public Task<ApiResponse<WMSClientCompaniesReadDTO>> GetClientCompanyInformationByNameAsync(
            string companyname,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSClientCompaniesReadDTO>(
                $"/api/ClientCompaniesWMS/wms-client-company-by-name/{companyname}",
                token,
                ct);

        public Task<ApiResponse<bool>> UpdateClientCompanyAsync(
            WmsCompanyclientDto entity,
            string token,
            CancellationToken ct)
            => throw new NotImplementedException();
    }
}

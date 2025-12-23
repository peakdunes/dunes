using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.CompaniesContract
{
    public class CompaniesClientContractWMSUIService : ICompaniesClientContractWMSUIService
    {
        public Task<ApiResponse<WMSCompaniesContractDTO>> AddClientCompanyContractAsync(WMSCompaniesContractDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> DeleteClientCompanyContractAsync(int id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetAllClientCompaniesContractInformationAsync(string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyContractInformationByIdAsync(int Id, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<List<WMSCompaniesContractReadDTO>>> GetClientCompanyInformationContractByCompanyIdAsync(int companyclientid, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberAsync(string contractcode, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<WMSCompaniesContractReadDTO>> GetClientCompanyInformationContractByNumberCompanyIdAsync(int companyclientid, string contractcode, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<bool>> UpdateClientCompanyContractAsync(WMSCompaniesContractDTO entity, string token, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

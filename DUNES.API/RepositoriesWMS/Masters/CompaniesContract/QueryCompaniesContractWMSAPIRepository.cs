using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{

    /// <summary>
    /// common query company client contracts
    /// </summary>
    public class QueryCompaniesContractWMSAPIRepository : IQueryCompaniesContractWMSAPIRepository
    {
        public Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationByCompanyAsync(int companyclientid, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<WMSCompaniesContractReadDTO> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<WMSCompaniesContractReadDTO> GetClientCompanyInformationContractByNumberAsync(int companyclientid, string contractcode, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}

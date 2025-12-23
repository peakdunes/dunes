using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesContract
{


    /// <summary>
    /// Companies Client contract
    /// </summary>
    public interface IQueryCompaniesContractWMSAPIRepository
    {


        /// <summary>
        /// Get all client company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationAsync(CancellationToken ct);


        /// <summary>
        /// Get all client company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>
        /// <returns></returns>
        Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationByCompanyAsync(int companyclientid, CancellationToken ct);



        /// <summary>
        /// Get all client contract information for a company by company identification
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompaniesContractReadDTO> GetClientCompanyInformationContractByNumberAsync(int companyclientid, string contractcode, CancellationToken ct);


       

        /// <summary>
        /// Get all client contract information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompaniesContractReadDTO> GetClientCompanyContractInformationByIdAsync(int Id, CancellationToken ct);


    }
}

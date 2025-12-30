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
        /// Get all company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyid"></param>
        /// <returns></returns>
        Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationByCompanyIdAsync(int companyid, CancellationToken ct);




        /// <summary>
        /// Get all client company contract  information
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="companyclientid"></param>
        /// <returns></returns>
        Task<List<WMSCompaniesContractReadDTO>> GetAllClientCompaniesContractInformationByCompanyClientIdAsync(int companyclientid, CancellationToken ct);



        /// <summary>
        /// Get all client contract information for a company by company identification and contract number 
        /// </summary>
        /// <param name="companyclientid"></param>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompaniesContractReadDTO?> GetClientCompanyInformationContractByNumberAndCompanyIdAsync(int companyclientid, string contractcode, CancellationToken ct);


       

        /// <summary>
        /// Get all client contract information for a company by company Id
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompaniesContractReadDTO?> GetClientCompanyContractInformationByContractIdAsync(int Id, CancellationToken ct);


        /// <summary>
        /// Get all client contract information for a company by company identification and contract number 
        /// </summary>
        /// <param name="contractcode"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompaniesContractReadDTO?> GetClientCompanyInformationContractByNumberAsync(string contractcode, CancellationToken ct);


    }
}

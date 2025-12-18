using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision
{

    /// <summary>
    /// CompaniesClientDivision repository
    /// </summary>
    public interface IQueryCompaniesClientDivisionWMSAPIRepository
    {

        /// <summary>
        /// Get all company information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<WMSCompanyClientDivisionReadDTO>> GetAllCompaniesClientDivisionInformation(CancellationToken ct);

        /// <summary>
        /// Get all division information for a company by id
        /// </summary>
        /// <param name="divisionId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompanyClientDivisionReadDTO?> GetCompanyClientDivisionById(int divisionId, CancellationToken ct);



        /// <summary>
        /// Get all division client information for a company by company identification
        /// </summary>
        /// <param name="divisionname"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSCompanyClientDivisionReadDTO?> GetCompanyClientDivisionByNameAsync(string divisionname, CancellationToken ct);


       
    }
}

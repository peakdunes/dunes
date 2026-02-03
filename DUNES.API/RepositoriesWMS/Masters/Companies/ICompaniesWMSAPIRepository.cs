using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.Companies
{

    /// <summary>
    /// WMS Companies repository
    /// </summary>
    public interface ICompaniesWMSAPIRepository
    {

        /// <summary>
        /// Get all company information
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<Company>> GetAllCompaniesInformation(CancellationToken ct);

    
        /// <summary>
        /// get all active companies
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Company>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Company?> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// exist country by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);

        /// <summary>
        /// add new country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Company> CreateAsync(DUNES.API.ModelsWMS.Masters.Company entity, CancellationToken ct);

        /// <summary>
        /// update country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Company> UpdateAsync(DUNES.API.ModelsWMS.Masters.Company entity, CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct);


        /// <summary>
        /// validate if a company is active
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> IsActiveAsync(int companyId, CancellationToken ct);

    }
}

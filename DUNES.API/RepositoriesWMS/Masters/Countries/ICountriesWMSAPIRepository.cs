using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.Countries
{

    /// <summary>
    /// WMS Countries repository
    /// </summary>
    public interface ICountriesWMSAPIRepository
    {

        /// <summary>
        /// get all countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Countries>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// get all active countries
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Countries>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get country by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Countries?> GetByIdAsync(int id, CancellationToken ct);

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
        Task<DUNES.API.ModelsWMS.Masters.Countries> CreateAsync(DUNES.API.ModelsWMS.Masters.Countries entity, CancellationToken ct);
        
        /// <summary>
        /// update country
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Countries> UpdateAsync(DUNES.API.ModelsWMS.Masters.Countries entity, CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct);
    }
}

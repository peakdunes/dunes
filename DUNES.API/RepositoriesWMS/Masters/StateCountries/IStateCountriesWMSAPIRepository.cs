namespace DUNES.API.RepositoriesWMS.Masters.StateCountries
{
    /// <summary>
    /// state repository
    /// </summary>
    public interface IStateCountriesWMSAPIRepository
    {

        /// <summary>
        /// get all states
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.StatesCountries>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// get all active states
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.StatesCountries>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get state by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.StatesCountries?> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// exist state by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);

        /// <summary>
        /// add new state
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.StatesCountries> CreateAsync(DUNES.API.ModelsWMS.Masters.StatesCountries entity, CancellationToken ct);

        /// <summary>
        /// update state
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.StatesCountries> UpdateAsync(DUNES.API.ModelsWMS.Masters.StatesCountries entity, CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct);
    }
}

namespace DUNES.API.RepositoriesWMS.Masters.Cities
{

    /// <summary>
    /// cities repository
    /// </summary>
    public interface ICitiesWMSAPIRepository
    {


        /// <summary>
        /// get all cities
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Cities>> GetAllAsync(CancellationToken ct);

        /// <summary>
        /// get all active cities
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Cities>> GetActiveAsync(CancellationToken ct);

        /// <summary>
        /// get city by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Cities?> GetByIdAsync(int id, CancellationToken ct);

        /// <summary>
        /// exist city by id
        /// </summary>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string name, int? excludeId, CancellationToken ct);

        /// <summary>
        /// add new city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Cities> CreateAsync(DUNES.API.ModelsWMS.Masters.Cities entity, CancellationToken ct);

        /// <summary>
        /// update city
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Cities> UpdateAsync(DUNES.API.ModelsWMS.Masters.Cities entity, CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<bool> SetActiveAsync(int id, bool isActive, CancellationToken ct);
    }
}



namespace DUNES.API.RepositoriesWMS.Masters.Racks
{

    /// <summary>
    /// Racks Repository
    /// </summary>
    public interface IRacksWMSAPIRepository
    {

        /// <summary>
        /// get all Racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Racks>> GetAllAsync(int companyId, int locationId, CancellationToken ct);



        /// <summary>
        /// get all active Racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<DUNES.API.ModelsWMS.Masters.Racks>> GetActiveAsync(int companyId, int locationId, CancellationToken ct);

        /// <summary>
        /// get Rack by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Racks?> GetByIdAsync(int companyId, int locationId, int id, CancellationToken ct);

        /// <summary>
        /// exist Rack by name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(int companyId, int locationId, string name, int? excludeId, CancellationToken ct);

        /// <summary>
        /// add new Rack
        /// </summary>
        /// 
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Racks> CreateAsync(DUNES.API.ModelsWMS.Masters.Racks entity, CancellationToken ct);

        /// <summary>
        /// update Rack
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Racks> UpdateAsync(DUNES.API.ModelsWMS.Masters.Racks entity, CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> SetActiveAsync(int companyId, int locationId,  int id, bool isActive, CancellationToken ct);
    }
}

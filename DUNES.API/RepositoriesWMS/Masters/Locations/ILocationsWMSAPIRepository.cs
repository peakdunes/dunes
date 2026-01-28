using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.Locations
{
    /// <summary>
    /// Locations Interface
    /// </summary>
    public interface ILocationsWMSAPIRepository
    {
        /// <summary>
        /// get all Locations
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<WMSLocationsDTO>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// get all active Locations
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<List<WMSLocationsDTO>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// get location by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WMSLocationsDTO?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// exist location by Name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// add new location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Locations> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Locations entity,
            CancellationToken ct);

        /// <summary>
        /// update location
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<DUNES.API.ModelsWMS.Masters.Locations> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Locations entity,
            CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

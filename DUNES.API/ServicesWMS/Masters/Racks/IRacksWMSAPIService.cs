using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.Racks
{

    /// <summary>
    /// Racks Services
    /// </summary>
    public interface IRacksWMSAPIService
    {

        /// <summary>
        /// get all racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSRacksDTO>>> GetAllAsync(int companyId, int locationId,  CancellationToken ct);
        /// <summary>
        /// get all active racks by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSRacksDTO>>> GetActiveAsync(int companyId, int locationId, CancellationToken ct);

        /// <summary>
        /// get rack by id
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSRacksDTO>> GetByIdAsync(int companyId, int locationId, int id, CancellationToken ct);

        /// <summary>
        /// add new rack
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSRacksDTO entity, CancellationToken ct);
        /// <summary>
        /// update rack
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSRacksDTO entity, CancellationToken ct);

        /// <summary>
        /// Active / No active.
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="id"></param>
        /// <param name="isActive"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> SetActiveAsync(int companyId, int locationId,  int id, bool isActive, CancellationToken ct);

        /// <summary>
        /// validate if exists a rack with the same name
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="locationId"></param>
        /// <param name="name"></param>
        /// <param name="excludeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> ExistsByNameAsync(int companyId, int locationId, string name, int? excludeId, CancellationToken ct);

    }
}

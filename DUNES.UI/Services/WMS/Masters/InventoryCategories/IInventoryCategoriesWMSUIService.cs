using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.InventoryCategories
{
    public interface IInventoryCategoriesWMSUIService
    {

        /// <summary>
        /// get all Inventory Categories
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetAllAsync(string token, CancellationToken ct);
        /// <summary>
        /// get all Inventory Categories
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<List<WMSInventorycategoriesReadDTO>>> GetActiveAsync(string token, CancellationToken ct);

        /// <summary>
        /// get inventory category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<WMSInventorycategoriesReadDTO?>> GetByIdAsync(int id, string token, CancellationToken ct);

        /// <summary>
        /// add new Inventory Category
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> CreateAsync(WMSInventorycategoriesCreateDTO entity, string token, CancellationToken ct);
        /// <summary>
        /// update Inventory Category
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ApiResponse<bool>> UpdateAsync(WMSInventorycategoriesUpdateDTO entity, string token, CancellationToken ct);
        /// <summary>
        /// Active / No active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(int id, bool isActive, string token, CancellationToken ct);

        /// <summary>
        /// validate if exists a Inventory Category with the same name
        /// </summary>
        Task<ApiResponse<bool>> ExistsByNameAsync(string name, int? excludeId, string token, CancellationToken ct);
    }
}

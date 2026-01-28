using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.InventoryCategories
{

    /// <summary>
    /// Inventory categories repository interface
    /// </summary>
    public interface IInventoryCategoriesWMSAPIRepository
    {
        // ============================
        // READ
        // ============================
        /// <summary>
        /// get all inventory categories
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>

        Task<List<Inventorycategories>> GetAllByCompanyAsync(int companyId);



        /// <summary>
        /// get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<Inventorycategories?> GetByIdAsync(int id, int companyId);

      
        // ============================
        // WRITE
        // ============================

        /// <summary>
        /// add inventory category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<Inventorycategories> CreateAsync(Inventorycategories entity);

        /// <summary>
        /// update inventory category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Inventorycategories entity);

        /// <summary>
        /// delete inventory category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id, int companyId);

        // ============================
        // VALIDATIONS / HELPERS
        // ============================

        /// <summary>
        /// exits category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int id, int companyId);

        /// <summary>
        /// exist category by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="companyId"></param>
        /// <param name="excludeId"></param>
        /// <returns></returns>
        Task<bool> NameExistsAsync(string name, int companyId, int? excludeId = null);
    }
}

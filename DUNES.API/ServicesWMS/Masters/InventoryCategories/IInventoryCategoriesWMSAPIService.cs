using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory categories interface service
    /// </summary>
    public interface IInventoryCategoriesWMSAPIService
    {
        /// <summary>
        /// Inventory categories service interface
        /// </summary>
        public interface IInventoryCategoriesWMSAPIService
        {
           
            /// <summary>
            /// Get all inventory categories by company
            /// </summary>
            ApiResponse<List<Inventorycategories>> GetAllByCompany(int companyId);

            /// <summary>
            /// Get inventory category by id and company
            /// </summary>
            ApiResponse<Inventorycategories> GetById(int id, int companyId);
          

            /// <summary>
            /// Create inventory category
            /// </summary>
            ApiResponse<Inventorycategories> Create(Inventorycategories entity);

            /// <summary>
            /// Update inventory category
            /// </summary>
            ApiResponse<bool> Update(Inventorycategories entity);

            /// <summary>
            /// Delete inventory category
            /// </summary>
            ApiResponse<bool> Delete(int id, int companyId);
        }
    }
}

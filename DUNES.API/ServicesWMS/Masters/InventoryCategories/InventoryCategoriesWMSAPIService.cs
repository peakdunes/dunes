using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.InventoryCategories;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// inventory categories service implementation
    /// </summary>
    public class InventoryCategoriesWMSAPIService : IInventoryCategoriesWMSAPIService
    {
        private readonly IInventoryCategoriesWMSAPIRepository _repository;
        private readonly ILogger<InventoryCategoriesWMSAPIService> _logger;

        /// <summary>
        /// constructor (DI)
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public InventoryCategoriesWMSAPIService(
            IInventoryCategoriesWMSAPIRepository repository,
            ILogger<InventoryCategoriesWMSAPIService> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        /// <summary>
        /// get all categories by company
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>

        public ApiResponse<List<Inventorycategories>> GetAllByCompany(int companyId)
        {

            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<Inventorycategories>>("Company is required");
            }

            var data = _repository
                .GetAllByCompanyAsync(companyId)
                .GetAwaiter()
                .GetResult();

            return ApiResponseFactory.Success(
                data,
                "Inventory categories retrieved successfully."
            );

        }

        /// <summary>
        /// get category by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ApiResponse<Inventorycategories> GetById(int id, int companyId)
        {

            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<Inventorycategories>("Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<Inventorycategories>("Category Id is required");
            }


            var entity = _repository
                .GetByIdAsync(id, companyId)
                .GetAwaiter()
                .GetResult();

            if (entity == null)
            {
                return ApiResponseFactory.NotFound<Inventorycategories>("Inventory category not found.");

            }

            return ApiResponseFactory.Success(
                entity,
                "Inventory category retrieved successfully."
            );

        }


        /// <summary>
        /// add new inventory category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ApiResponse<Inventorycategories> Create(Inventorycategories entity)
        {

            if (entity.companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<Inventorycategories>("Company is required");
            }
            if (entity.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<Inventorycategories>("Category Id is required");
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                return ApiResponseFactory.BadRequest<Inventorycategories>("Category Name is required");
            }


            var nameExists = _repository
                .NameExistsAsync(entity.Name ?? string.Empty, entity.companyId)
                .GetAwaiter()
                .GetResult();

            if (nameExists)
            {

                return ApiResponseFactory.Fail<Inventorycategories>("DUPLICATE_CATEGORY", "An inventory category with the same name already exists.",
                       Convert.ToInt32(StatusCodes.Status409Conflict));
            }

            var created = _repository
                .CreateAsync(entity)
                .GetAwaiter()
                .GetResult();

            return ApiResponseFactory.Success(
                created,
                "Inventory category created successfully."
            );

        }
        /// <summary>
        /// update category
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ApiResponse<bool> Update(Inventorycategories entity)
        {

            if (entity.companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Company is required");
            }
            if (entity.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Category Id is required");
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                return ApiResponseFactory.BadRequest<bool>("Category Name is required");
            }

            var exists = _repository
                    .ExistsAsync(entity.Id, entity.companyId)
                    .GetAwaiter()
                    .GetResult();

                if (!exists)
                {
                    return ApiResponseFactory.NotFound<bool>(
                        "Inventory category not found."
                    );
                }

                var nameExists = _repository
                    .NameExistsAsync(
                        entity.Name ?? string.Empty,
                        entity.companyId,
                        entity.Id)
                    .GetAwaiter()
                    .GetResult();

                if (nameExists)
                {
                return ApiResponseFactory.Fail<bool>("DUPLICATE_CATEGORY", "An inventory category with the same name already exists.",
                    Convert.ToInt32(StatusCodes.Status409Conflict));
            }

                var updated = _repository
                    .UpdateAsync(entity)
                    .GetAwaiter()
                    .GetResult();

                if (!updated)
                {

                return ApiResponseFactory.Fail<bool>("UPDATE_ERROR", "Inventory category could not be updated..",
                   Convert.ToInt32(StatusCodes.Status409Conflict));

                
                }

                return ApiResponseFactory.Success(
                    true,
                    "Inventory category updated successfully."
                );
         
        }
        /// <summary>
        /// delete category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public ApiResponse<bool> Delete(int id, int companyId)
        {

            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Company is required");
            }
            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Category Id is required");
            }

         

            var exists = _repository
                    .ExistsAsync(id, companyId)
                    .GetAwaiter()
                    .GetResult();

                if (!exists)
                {
                    return ApiResponseFactory.NotFound<bool>(
                        "Inventory category not found."
                    );
                }

                var deleted = _repository
                    .DeleteAsync(id, companyId)
                    .GetAwaiter()
                    .GetResult();

                if (!deleted)
                {
                    return ApiResponseFactory.Fail<bool>("DELETE_ERROR",
                        "Inventory category could not be deleted.", StatusCodes.Status409Conflict
                    );
                }

                return ApiResponseFactory.Success(
                    true,
                    "Inventory category deleted successfully."
                );
            
        }
    }
}


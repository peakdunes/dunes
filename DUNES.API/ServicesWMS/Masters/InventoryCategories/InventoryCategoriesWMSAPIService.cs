using AutoMapper;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.InventoryCategories;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.InventoryCategories
{
    /// <summary>
    /// Inventory Categories Service
    /// Business logic layer for inventory categories.
    /// Scoped strictly by Company (STANDARD COMPANYID).
    /// </summary>
    public class InventoryCategoriesWMSAPIService : IInventoryCategoriesWMSAPIService
    {
        private readonly IInventoryCategoriesWMSAPIRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor (Dependency Injection)
        /// </summary>
        /// <param name="repository">Inventory categories repository</param>
        /// <param name="mapper">AutoMapper instance</param>
        public InventoryCategoriesWMSAPIService(
            IInventoryCategoriesWMSAPIRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all inventory categories for a specific company
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of inventory categories</returns>
        public async Task<ApiResponse<List<WMSInventoryCategoriesDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<WMSInventoryCategoriesDTO>>(
                    "Company is required");
            }

            var data = await _repository.GetAllAsync(companyId, ct);

            if (data == null || data.Count == 0)
            {
                return ApiResponseFactory.NotFound<List<WMSInventoryCategoriesDTO>>(
                    "No inventory categories found.");
            }

            var result = _mapper.Map<List<WMSInventoryCategoriesDTO>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get inventory category by id (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Inventory category id</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Inventory category data</returns>
        public async Task<ApiResponse<WMSInventoryCategoriesDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSInventoryCategoriesDTO>(
                    "Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSInventoryCategoriesDTO>(
                    "Category Id is required");
            }

            var entity = await _repository.GetByIdAsync(companyId, id, ct);

            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSInventoryCategoriesDTO>(
                    "Inventory category not found.");
            }

            var result = _mapper.Map<WMSInventoryCategoriesDTO>(entity);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Create a new inventory category
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="dto">Inventory category DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        public async Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSInventoryCategoriesDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Company is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Category name is required");
            }

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                dto.Name,
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_CATEGORY",
                    message: $"An inventory category with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            var entity = _mapper.Map<Inventorycategories>(dto);
            entity.companyId = companyId;
            //entity.Active = true;

            await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Ok(
                true,
                "Inventory category created successfully.");
        }

        /// <summary>
        /// Update an existing inventory category
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Inventory category id</param>
        /// <param name="dto">Inventory category DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSInventoryCategoriesDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Category Id is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Category name is required");
            }

            var current = await _repository.GetByIdAsync(companyId, id, ct);

            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Inventory category not found.");
            }

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                dto.Name,
                id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "DUPLICATE_CATEGORY",
                    message: $"An inventory category with the name '{dto.Name}' already exists.",
                    statusCode: (int)HttpStatusCode.Conflict);
            }

            current.Name = dto.Name;
            current.Observations = dto.Observations;
            current.Active = dto.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(
                true,
                "Inventory category updated successfully.");
        }

        /// <summary>
        /// Activate or deactivate an inventory category
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Inventory category id</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Category Id is required");
            }

            var ok = await _repository.SetActiveAsync(
                companyId,
                id,
                isActive,
                ct);

            if (!ok)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Inventory category not found.");
            }

            var message = isActive
                ? "Inventory category activated successfully."
                : "Inventory category deactivated successfully.";

            return ApiResponseFactory.Ok(true, message);
        }
    }
}

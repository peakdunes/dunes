using AutoMapper;
using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.InventoryCategories;
using DUNES.API.RepositoriesWMS.Masters.Items;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.Items
{
    /// <summary>
    /// Items Service
    /// Business logic layer for company-owned inventory items.
    /// Scoped strictly by Company (STANDARD COMPANYID).
    /// </summary>
    public class ItemsWMSAPIService : IItemsWMSAPIService
    {
        private readonly IItemsWMSAPIRepository _repository;
        private readonly IInventoryCategoriesWMSAPIRepository _categoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor (Dependency Injection)
        /// </summary>
        /// <param name="repository">Items repository</param>
        /// <param name="categoryRepository">Inventory categories repository</param>
        /// <param name="mapper">AutoMapper instance</param>
        public ItemsWMSAPIService(
            IItemsWMSAPIRepository repository,
            IInventoryCategoriesWMSAPIRepository categoryRepository,
            IMapper mapper)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all items for a company
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of items</returns>
        public async Task<ApiResponse<List<WMSItemsDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<WMSItemsDTO>>(
                    "Company is required");
            }

            var data = await _repository.GetAllAsync(companyId, ct);

            if (data == null || data.Count == 0)
            {
                return ApiResponseFactory.NotFound<List<WMSItemsDTO>>(
                    "No items found.");
            }

            var result = _mapper.Map<List<WMSItemsDTO>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get all active items for a company
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active items</returns>
        public async Task<ApiResponse<List<WMSItemsDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<List<WMSItemsDTO>>(
                    "Company is required");
            }

            var data = await _repository.GetActiveAsync(companyId, ct);

            if (data == null || data.Count == 0)
            {
                return ApiResponseFactory.NotFound<List<WMSItemsDTO>>(
                    "No active items found.");
            }

            var result = _mapper.Map<List<WMSItemsDTO>>(data);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Get item by id (scoped by company)
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Item identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Item data</returns>
        public async Task<ApiResponse<WMSItemsDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSItemsDTO>(
                    "Company is required");
            }

            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSItemsDTO>(
                    "Item Id is required");
            }

            var entity = await _repository.GetByIdAsync(companyId, id, ct);

            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSItemsDTO>(
                    $"Item with Id {id} was not found.");
            }

            var result = _mapper.Map<WMSItemsDTO>(entity);
            return ApiResponseFactory.Ok(result);
        }

        /// <summary>
        /// Create a new item
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="dto">Item DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        public async Task<ApiResponse<bool>> CreateAsync(
            int companyId,
            WMSItemsDTO dto,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Company is required");
            }

            if (dto.InventorycategoriesId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Inventory category is required");
            }

            if (string.IsNullOrWhiteSpace(dto.Sku))
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "SKU is required");
            }

            if (string.IsNullOrWhiteSpace(dto.ItemDescription))
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Item description is required");
            }

            // Validate inventory category exists and belongs to company
            var category = await _categoryRepository.GetByIdAsync(
                companyId,
                dto.InventorycategoriesId,
                ct);

            if (category is null || !category.Active)
            {
                return ApiResponseFactory.Fail<bool>(
                    "INVALID_CATEGORY",
                    "Inventory category does not exist or is inactive.",
                    (int)HttpStatusCode.BadRequest);
            }

            // Validate duplicate SKU
            var skuExists = await _repository.ExistsBySkuAsync(
                companyId,
                dto.Sku,
                null,
                ct);

            if (skuExists)
            {
                return ApiResponseFactory.Fail<bool>(
                    "DUPLICATE_SKU",
                    $"An item with SKU '{dto.Sku}' already exists.",
                    (int)HttpStatusCode.Conflict);
            }

            // Validate duplicate Barcode (if provided)
            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var barcodeExists = await _repository.ExistsByBarcodeAsync(
                    companyId,
                    dto.Barcode,
                    null,
                    ct);

                if (barcodeExists)
                {
                    return ApiResponseFactory.Fail<bool>(
                        "DUPLICATE_BARCODE",
                        $"An item with Barcode '{dto.Barcode}' already exists.",
                        (int)HttpStatusCode.Conflict);
                }
            }

            var entity = _mapper.Map<DUNES.API.ModelsWMS.Masters.Items>(dto);
            entity.companyId = companyId;
            entity.CompanyClientId = null;
            entity.active = true;

            await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Ok(
                true,
                "Item created successfully.");
        }

        /// <summary>
        /// Update an existing item
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Item identifier</param>
        /// <param name="dto">Item DTO</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Operation result</returns>
        public async Task<ApiResponse<bool>> UpdateAsync(
            int companyId,
            int id,
            WMSItemsDTO dto,
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
                    "Item Id is required");
            }

            var current = await _repository.GetByIdAsync(companyId, id, ct);

            if (current is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Item not found.");
            }

            // Validate inventory category
            var category = await _categoryRepository.GetByIdAsync(
                companyId,
                dto.InventorycategoriesId,
                ct);

            if (category is null || !category.Active)
            {
                return ApiResponseFactory.Fail<bool>(
                    "INVALID_CATEGORY",
                    "Inventory category does not exist or is inactive.",
                    (int)HttpStatusCode.BadRequest);
            }

            // Validate duplicate SKU
            var skuExists = await _repository.ExistsBySkuAsync(
                companyId,
                dto.Sku,
                id,
                ct);

            if (skuExists)
            {
                return ApiResponseFactory.Fail<bool>(
                    "DUPLICATE_SKU",
                    $"An item with SKU '{dto.Sku}' already exists.",
                    (int)HttpStatusCode.Conflict);
            }

            // Validate duplicate Barcode
            if (!string.IsNullOrWhiteSpace(dto.Barcode))
            {
                var barcodeExists = await _repository.ExistsByBarcodeAsync(
                    companyId,
                    dto.Barcode,
                    id,
                    ct);

                if (barcodeExists)
                {
                    return ApiResponseFactory.Fail<bool>(
                        "DUPLICATE_BARCODE",
                        $"An item with Barcode '{dto.Barcode}' already exists.",
                        (int)HttpStatusCode.Conflict);
                }
            }

            // Apply updates
            current.InventorycategoriesId = dto.InventorycategoriesId;
            current.sku = dto.Sku;
            current.itemDescription = dto.ItemDescription;
            current.Barcode = dto.Barcode;
            current.serialnumber = dto.SerialNumber;
            current.isRepairable = dto.IsRepairable;
            current.isSerialized = dto.IsSerialized;
            current.active = dto.Active;

            await _repository.UpdateAsync(current, ct);

            return ApiResponseFactory.Ok(
                true,
                "Item updated successfully.");
        }

        /// <summary>
        /// Activate or deactivate an item
        /// </summary>
        /// <param name="companyId">Company identifier (from token)</param>
        /// <param name="id">Item identifier</param>
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
                    "Item Id is required");
            }

            var ok = await _repository.SetActiveAsync(
                companyId,
                id,
                isActive,
                ct);

            if (!ok)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Item not found.");
            }

            var message = isActive
                ? "Item activated successfully."
                : "Item deactivated successfully.";

            return ApiResponseFactory.Ok(true, message);
        }

        /// <summary>
        /// Validate if an item exists with the same SKU
        /// </summary>
        public async Task<ApiResponse<bool>> ExistsBySkuAsync(
            int companyId,
            string sku,
            int? excludeId,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Company is required");
            }

            if (string.IsNullOrWhiteSpace(sku))
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "SKU is required");
            }

            var exists = await _repository.ExistsBySkuAsync(
                companyId,
                sku,
                excludeId,
                ct);

            return ApiResponseFactory.Ok(exists);
        }

        /// <summary>
        /// Validate if an item exists with the same Barcode
        /// </summary>
        public async Task<ApiResponse<bool>> ExistsByBarcodeAsync(
            int companyId,
            string barcode,
            int? excludeId,
            CancellationToken ct)
        {
            if (companyId <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Company is required");
            }

            if (string.IsNullOrWhiteSpace(barcode))
            {
                return ApiResponseFactory.BadRequest<bool>(
                    "Barcode is required");
            }

            var exists = await _repository.ExistsByBarcodeAsync(
                companyId,
                barcode,
                excludeId,
                ct);

            return ApiResponseFactory.Ok(exists);
        }
    }
}


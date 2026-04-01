using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.Items;
using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.Items
{
    /// <summary>
    /// Service implementation for managing Items within WMS.
    /// Applies tenant scope, ownership mode rules, validation,
    /// and business constraints on top of the repository layer.
    /// </summary>
    public class ItemsWMSAPIService : IItemsWMSAPIService
    {
        private readonly IItemsWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository for Items.</param>
        public ItemsWMSAPIService(IItemsWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSItemsReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var (includeMasterItems, includeClientItems) =
                await ResolveOwnershipModeAsync(companyId, companyClientId, ct);

            var data = await _repository.GetAllAsync(
                companyId,
                companyClientId,
                includeMasterItems,
                includeClientItems,
                ct);

            return ApiResponseFactory.Success(
                data,
                "Items retrieved successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSItemsReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var (includeMasterItems, includeClientItems) =
                await ResolveOwnershipModeAsync(companyId, companyClientId, ct);

            var data = await _repository.GetByIdAsync(
                id,
                companyId,
                companyClientId,
                includeMasterItems,
                includeClientItems,
                ct);

            if (data is null)
            {
                return ApiResponseFactory.NotFound<WMSItemsReadDTO>(
                    "Item was not found.");
            }

            return ApiResponseFactory.Success(
                data,
                "Item retrieved successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSItemsReadDTO>> CreateAsync(
            WMSItemsCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            NormalizeDto(dto);

            var existsPartNumber = await _repository.ExistsPartNumberAsync(
                dto.PartNumber,
                excludeId: null,
                ct);

            if (existsPartNumber)
            {
                return ApiResponseFactory.Fail<WMSItemsReadDTO>(
                    error: "PARTNUMBER_EXISTS",
                    message: "An item with the same Part Number already exists.",
                    statusCode: 400);
            }

            var ownerCompanyClientId = ResolveCreateOwnership(dto.CompanyClientId, companyClientId);

            var entity = new ModelsWMS.Masters.Items
            {
                CompanyId = companyId,
                CompanyClientId = ownerCompanyClientId,
                InventoryCategoryId = dto.InventoryCategoryId,
                PartNumber = dto.PartNumber,
                Sku = string.IsNullOrWhiteSpace(dto.Sku) ? null : dto.Sku.Trim(),
                ItemDescription = dto.ItemDescription.Trim(),
                Barcode = string.IsNullOrWhiteSpace(dto.Barcode) ? null : dto.Barcode.Trim(),
                IsRepairable = dto.IsRepairable,
                IsSerialized = dto.IsSerialized,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);

            var (includeMasterItems, includeClientItems) =
                await ResolveOwnershipModeAsync(companyId, companyClientId, ct);

            var createdDto = await _repository.GetByIdAsync(
                created.Id,
                companyId,
                companyClientId,
                includeMasterItems: true,
                includeClientItems: true,
                ct);

            return ApiResponseFactory.Success(
                createdDto!,
                "Item created successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSItemsReadDTO>> UpdateAsync(
            int id,
            WMSItemsUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            NormalizeDto(dto);

            var (includeMasterItems, includeClientItems) =
                await ResolveOwnershipModeAsync(companyId, companyClientId, ct);

            var entity = await _repository.GetEntityByIdAsync(
                id,
                companyId,
                companyClientId,
                includeMasterItems,
                includeClientItems,
                ct);

            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSItemsReadDTO>(
                    "Item was not found.");
            }

            var existsPartNumber = await _repository.ExistsPartNumberAsync(
                dto.PartNumber,
                excludeId: id,
                ct);

            if (existsPartNumber)
            {
                return ApiResponseFactory.Fail<WMSItemsReadDTO>(
                    error: "PARTNUMBER_EXISTS",
                    message: "Another item with the same Part Number already exists.",
                    statusCode: 400);
            }

            entity.InventoryCategoryId = dto.InventoryCategoryId;
            entity.PartNumber = dto.PartNumber;
            entity.Sku = string.IsNullOrWhiteSpace(dto.Sku) ? null : dto.Sku.Trim();
            entity.ItemDescription = dto.ItemDescription.Trim();
            entity.Barcode = string.IsNullOrWhiteSpace(dto.Barcode) ? null : dto.Barcode.Trim();
            entity.IsRepairable = dto.IsRepairable;
            entity.IsSerialized = dto.IsSerialized;
            entity.Active = dto.Active;

            var updated = await _repository.UpdateAsync(entity, ct);

            if (!updated)
            {
                return ApiResponseFactory.Fail<WMSItemsReadDTO>(
                    error: "NOT_UPDATE",
                    message: "The item could not be updated.",
                    statusCode: 500);
            }

            var updatedDto = await _repository.GetByIdAsync(
                id,
                companyId,
                companyClientId,
                includeMasterItems,
                includeClientItems,
                ct);

            return ApiResponseFactory.Success(
                updatedDto!,
                "Item updated successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var (includeMasterItems, includeClientItems) =
                await ResolveOwnershipModeAsync(companyId, companyClientId, ct);

            var updated = await _repository.SetActiveAsync(
                id,
                companyId,
                companyClientId,
                includeMasterItems,
                includeClientItems,
                isActive,
                ct);

            if (!updated)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Item was not found.");
            }

            return ApiResponseFactory.Success(
                true,
                "Item status updated successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var (includeMasterItems, includeClientItems) =
                await ResolveOwnershipModeAsync(companyId, companyClientId, ct);

            var entity = await _repository.GetEntityByIdAsync(
                id,
                companyId,
                companyClientId,
                includeMasterItems,
                includeClientItems,
                ct);

            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Item was not found.");
            }

            var deleted = await _repository.DeleteAsync(entity, ct);

            if (!deleted)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "NOT_DELETE",
                    message: "The item could not be deleted.",
                    statusCode: 500);
            }

            return ApiResponseFactory.Success(
                true,
                "Item deleted successfully.");
        }

        /// <summary>
        /// Resolves the ownership mode for the current client context.
        /// Temporary implementation:
        /// - master only by default.
        /// Replace this logic with the actual client configuration source.
        /// </summary>
        private Task<(bool includeMasterItems, bool includeClientItems)> ResolveOwnershipModeAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // TODO:
            // Replace with the real source of truth for item ownership mode.
            // Example expected outcomes:
            // Master only -> (true, false)
            // Client only -> (false, true)
            // Mixed       -> (true, true)

            return Task.FromResult((includeMasterItems: true, includeClientItems: false));
        }

        /// <summary>
        /// Resolves the owner CompanyClientId for a new item.
        /// If dto.CompanyClientId is null, the item is treated as a master/company item.
        /// If dto.CompanyClientId has a value, it must match the current client context.
        /// </summary>
        private static int? ResolveCreateOwnership(int? dtoCompanyClientId, int currentCompanyClientId)
        {
            if (!dtoCompanyClientId.HasValue)
                return null;

            if (dtoCompanyClientId.Value != currentCompanyClientId)
            {
                throw new InvalidOperationException("Invalid CompanyClientId for the current tenant scope.");
            }

            return dtoCompanyClientId;
        }

        /// <summary>
        /// Normalizes create DTO values before validation and persistence.
        /// </summary>
        private static void NormalizeDto(WMSItemsCreateDTO dto)
        {
            dto.PartNumber = dto.PartNumber.Trim().ToUpperInvariant();
            dto.ItemDescription = dto.ItemDescription.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Sku))
                dto.Sku = dto.Sku.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
                dto.Barcode = dto.Barcode.Trim();
        }

        /// <summary>
        /// Normalizes update DTO values before validation and persistence.
        /// </summary>
        private static void NormalizeDto(WMSItemsUpdateDTO dto)
        {
            dto.PartNumber = dto.PartNumber.Trim().ToUpperInvariant();
            dto.ItemDescription = dto.ItemDescription.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Sku))
                dto.Sku = dto.Sku.Trim();

            if (!string.IsNullOrWhiteSpace(dto.Barcode))
                dto.Barcode = dto.Barcode.Trim();
        }
    }
}
using DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Service implementation for CompanyClientInventoryType mappings.
    /// Applies tenant-scoped business rules and delegates persistence to repository layer.
    /// </summary>
    public class CompanyClientInventoryTypeWMSAPIService : ICompanyClientInventoryTypeWMSAPIService
    {
        private readonly ICompanyClientInventoryTypeWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientInventoryTypeWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository for CompanyClientInventoryType mappings.</param>
        public CompanyClientInventoryTypeWMSAPIService(ICompanyClientInventoryTypeWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSCompanyClientInventoryTypeReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, companyClientId, ct);

            // Ajusta esta línea al factory real de tu solución si el nombre difiere.
            return ApiResponseFactory.Success(data, "Inventory types retrieved successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Invalid mapping Id.");
            }

            var data = await _repository.GetByIdAsync(id, companyId, companyClientId, ct);

            if (data is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryTypeReadDTO>(
                    "Inventory type mapping not found for the current client.");
            }

            return ApiResponseFactory.Success(data, "Inventory type mapping retrieved successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> CreateAsync(
            WMSCompanyClientInventoryTypeCreateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (request is null)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Request body is required.");
            }

            if (request.InventoryTypeId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("InventoryTypeId is required.");
            }

            // Rule 1: Master must exist
            var masterExists = await _repository.MasterExistsAsync(request.InventoryTypeId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryTypeReadDTO>(
                    "The selected InventoryType does not exist in master catalog.");
            }

            // Rule 2: No duplicate mappings in same tenant scope
            var existsDuplicate = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                request.InventoryTypeId,
                excludeId: null,
                ct);

            if (existsDuplicate)
            {
                return ApiResponseFactory.Conflict<WMSCompanyClientInventoryTypeReadDTO>(
                    "This InventoryType is already mapped to the current client.");
            }

            // Rule 3: If enabling mapping, master must be active
            if (request.IsActive)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(request.InventoryTypeId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>(
                        "Cannot enable mapping because the master InventoryType is inactive.");
                }
            }

            var entity = new  DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                InventoryTypeId = request.InventoryTypeId,
                IsActive = request.IsActive
            };

            var created = await _repository.CreateAsync(entity, ct);

            // Re-read to return normalized DTO (with master name)
            var createdDto = await _repository.GetByIdAsync(created.Id, companyId, companyClientId, ct);

            if (createdDto is null)
            {
                // Esto no debería pasar, pero si pasa lo manejamos limpio.
                return ApiResponseFactory.Error<WMSCompanyClientInventoryTypeReadDTO>(
                    "The mapping was created, but the response could not be loaded.");
            }

            return ApiResponseFactory.Success(createdDto, "Inventory type mapping created successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> UpdateAsync(
            WMSCompanyClientInventoryTypeUpdateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (request is null)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Request body is required.");
            }

            if (request.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Mapping Id is required.");
            }

            if (request.InventoryTypeId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("InventoryTypeId is required.");
            }

            // Rule 1: Mapping must exist in tenant scope
            var entity = await _repository.GetEntityByIdAsync(request.Id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryTypeReadDTO>(
                    "Inventory type mapping not found for the current client.");
            }

            // Rule 2: Master must exist
            var masterExists = await _repository.MasterExistsAsync(request.InventoryTypeId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryTypeReadDTO>(
                    "The selected InventoryType does not exist in master catalog.");
            }

            // Rule 3: No duplicates (excluding current record)
            var existsDuplicate = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                request.InventoryTypeId,
                excludeId: request.Id,
                ct);

            if (existsDuplicate)
            {
                return ApiResponseFactory.Conflict<WMSCompanyClientInventoryTypeReadDTO>(
                    "This InventoryType is already mapped to the current client.");
            }

            // Rule 4: If enabling mapping, master must be active
            if (request.IsActive)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(request.InventoryTypeId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>(
                        "Cannot enable mapping because the master InventoryType is inactive.");
                }
            }

            // Apply changes
            entity.InventoryTypeId = request.InventoryTypeId;
            entity.IsActive = request.IsActive;

            var updated = await _repository.UpdateAsync(entity, ct);
            if (!updated)
            {
                return ApiResponseFactory.Error<WMSCompanyClientInventoryTypeReadDTO>(
                    "No changes were saved for the inventory type mapping.");
            }

            var updatedDto = await _repository.GetByIdAsync(entity.Id, companyId, companyClientId, ct);

            if (updatedDto is null)
            {
                return ApiResponseFactory.Error<WMSCompanyClientInventoryTypeReadDTO>(
                    "The mapping was updated, but the response could not be loaded.");
            }

            return ApiResponseFactory.Success(updatedDto, "Inventory type mapping updated successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<bool>("Invalid mapping Id.");
            }

            var entity = await _repository.GetEntityByIdAsync(id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Inventory type mapping not found for the current client.");
            }

            var deleted = await _repository.DeleteAsync(entity, ct);

            if (!deleted)
            {
                return ApiResponseFactory.Error<bool>("The inventory type mapping could not be deleted.");
            }

            return ApiResponseFactory.Success(true, "Inventory type mapping deleted successfully.");
        }
        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientInventoryTypeReadDTO>> SetActiveAsync(
            WMSCompanyClientInventoryTypeSetActiveDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (request is null)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Request body is required.");
            }

            if (request.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>("Mapping Id is required.");
            }

            // Mapping must exist in tenant scope
            var entity = await _repository.GetEntityByIdAsync(request.Id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientInventoryTypeReadDTO>(
                    "Inventory type mapping not found for the current client.");
            }

            // If activating mapping, master must be active
            if (request.IsActive)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(entity.InventoryTypeId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.BadRequest<WMSCompanyClientInventoryTypeReadDTO>(
                        "Cannot activate mapping because the master InventoryType is inactive.");
                }
            }

            var changed = await _repository.SetActiveAsync(
                request.Id,
                companyId,
                companyClientId,
                request.IsActive,
                ct);

            if (!changed)
            {
                return ApiResponseFactory.Error<WMSCompanyClientInventoryTypeReadDTO>(
                    "The mapping status could not be updated.");
            }

            var dto = await _repository.GetByIdAsync(request.Id, companyId, companyClientId, ct);

            if (dto is null)
            {
                return ApiResponseFactory.Error<WMSCompanyClientInventoryTypeReadDTO>(
                    "Status updated, but the updated mapping could not be loaded.");
            }

            return ApiResponseFactory.Success(
                dto,
                request.IsActive
                    ? "Inventory type mapping activated successfully."
                    : "Inventory type mapping deactivated successfully.");
        }


    }
}


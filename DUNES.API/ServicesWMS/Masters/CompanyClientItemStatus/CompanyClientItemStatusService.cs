using DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service implementation for CompanyClientItemStatus mappings.
    /// Applies tenant-scoped business rules and delegates persistence operations to the repository layer.
    /// </summary>
    public class CompanyClientItemStatusWMSAPIService : ICompanyClientItemStatusWMSAPIService
    {
        private readonly ICompanyClientItemStatusWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository for CompanyClientItemStatus mappings.</param>
        public CompanyClientItemStatusWMSAPIService(ICompanyClientItemStatusWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, companyClientId, ct);
            return ApiResponseFactory.Success(data, "Item status mappings retrieved successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Invalid mapping Id.");
            }

            var data = await _repository.GetByIdAsync(id, companyId, companyClientId, ct);

            if (data is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>(
                    "Item status mapping not found for the current client.");
            }

            return ApiResponseFactory.Success(data, "Item status mapping retrieved successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (request is null)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Request body is required.");
            }

            if (request.ItemStatusId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("ItemStatusId is required.");
            }

            // Rule 1: master must exist
            var masterExists = await _repository.MasterExistsAsync(request.ItemStatusId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>(
                    "The selected ItemStatus does not exist in master catalog.");
            }

            // Rule 2: if enabling mapping, master must be active
            if (request.IsActive)
            {
                var masterActive = await _repository.MasterIsActiveAsync(request.ItemStatusId, ct);
                if (!masterActive)
                {
                    return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                        "Cannot enable mapping because the master ItemStatus is inactive.");
                }
            }

            // Rule 3: no duplicates
            var exists = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                request.ItemStatusId,
                excludeId: null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Conflict<WMSCompanyClientItemStatusReadDTO>(
                    "This ItemStatus is already mapped to the current client.");
            }

            var entity = new DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                ItemStatusId = request.ItemStatusId,
                IsActive = request.IsActive
            };

            var created = await _repository.CreateAsync(entity, ct);

            var createdDto = await _repository.GetByIdAsync(created.Id, companyId, companyClientId, ct);
            if (createdDto is null)
            {
                return ApiResponseFactory.Error<WMSCompanyClientItemStatusReadDTO>(
                    "The mapping was created, but the response could not be loaded.");
            }

            return ApiResponseFactory.Success(createdDto, "Item status mapping created successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> UpdateAsync(
            WMSCompanyClientItemStatusUpdateDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (request is null)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Request body is required.");
            }

            if (request.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Mapping Id is required.");
            }

            if (request.ItemStatusId <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("ItemStatusId is required.");
            }

            // Rule 1: mapping must exist in tenant scope
            var entity = await _repository.GetEntityByIdAsync(request.Id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>(
                    "Item status mapping not found for the current client.");
            }

            // Rule 2: master must exist
            var masterExists = await _repository.MasterExistsAsync(request.ItemStatusId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>(
                    "The selected ItemStatus does not exist in master catalog.");
            }

            // Rule 3: if enabling mapping, master must be active
            if (request.IsActive)
            {
                var masterActive = await _repository.MasterIsActiveAsync(request.ItemStatusId, ct);
                if (!masterActive)
                {
                    return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                        "Cannot enable mapping because the master ItemStatus is inactive.");
                }
            }

            // Rule 4: no duplicates (excluding current mapping)
            var exists = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                request.ItemStatusId,
                excludeId: request.Id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Conflict<WMSCompanyClientItemStatusReadDTO>(
                    "This ItemStatus is already mapped to the current client.");
            }

            entity.ItemStatusId = request.ItemStatusId;
            entity.IsActive = request.IsActive;

            var updated = await _repository.UpdateAsync(entity, ct);
            if (!updated)
            {
                return ApiResponseFactory.Error<WMSCompanyClientItemStatusReadDTO>(
                    "No changes were saved for the item status mapping.");
            }

            var updatedDto = await _repository.GetByIdAsync(entity.Id, companyId, companyClientId, ct);
            if (updatedDto is null)
            {
                return ApiResponseFactory.Error<WMSCompanyClientItemStatusReadDTO>(
                    "The mapping was updated, but the response could not be loaded.");
            }

            return ApiResponseFactory.Success(updatedDto, "Item status mapping updated successfully.");
        }

        /// <inheritdoc />
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> SetActiveAsync(
            WMSCompanyClientItemStatusSetActiveDTO request,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            if (request is null)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Request body is required.");
            }

            if (request.Id <= 0)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>("Mapping Id is required.");
            }

            var entity = await _repository.GetEntityByIdAsync(request.Id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>(
                    "Item status mapping not found for the current client.");
            }

            if (request.IsActive)
            {
                var masterActive = await _repository.MasterIsActiveAsync(entity.ItemStatusId, ct);
                if (!masterActive)
                {
                    return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                        "Cannot activate mapping because the master ItemStatus is inactive.");
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
                return ApiResponseFactory.Error<WMSCompanyClientItemStatusReadDTO>(
                    "The mapping status could not be updated.");
            }

            var dto = await _repository.GetByIdAsync(request.Id, companyId, companyClientId, ct);
            if (dto is null)
            {
                return ApiResponseFactory.Error<WMSCompanyClientItemStatusReadDTO>(
                    "Status updated, but the updated mapping could not be loaded.");
            }

            return ApiResponseFactory.Success(
                dto,
                request.IsActive
                    ? "Item status mapping activated successfully."
                    : "Item status mapping deactivated successfully.");
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
                    "Item status mapping not found for the current client.");
            }

            var deleted = await _repository.DeleteAsync(entity, ct);
            if (!deleted)
            {
                return ApiResponseFactory.Error<bool>(
                    "The item status mapping could not be deleted.");
            }

            return ApiResponseFactory.Success(true, "Item status mapping deleted successfully.");
        }
    }
}

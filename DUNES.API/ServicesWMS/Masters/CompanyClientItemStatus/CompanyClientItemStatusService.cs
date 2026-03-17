using DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Service implementation for client-level item status enablement.
    /// Business rules enforced here:
    /// - Token-scoped CompanyId and CompanyClientId are mandatory.
    /// - A master item status must be active before it can be enabled for a client.
    /// - Duplicate mappings for the same client and item status are not allowed.
    /// - Delete only removes the client mapping and never deletes the master record.
    /// </summary>
    public class CompanyClientItemStatusWMSAPIService : ICompanyClientItemStatusWMSAPIService
    {
        private readonly ICompanyClientItemStatusWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository for client item status mappings.</param>
        public CompanyClientItemStatusWMSAPIService(ICompanyClientItemStatusWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Returns all enabled item status mappings for the current client.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the list of enabled item statuses.</returns>
        public async Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, companyClientId, ct);

            return ApiResponseFactory.Success(data, "Item statuses retrieved successfully.");
        }

        /// <summary>
        /// Returns enabled item status mappings for the current client.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the list of enabled item statuses.</returns>
        public async Task<ApiResponse<List<WMSCompanyClientItemStatusReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetEnabledAsync(companyId, companyClientId, ct);

            return ApiResponseFactory.Success(data, "Enabled item statuses retrieved successfully.");
        }

        /// <summary>
        /// Returns a specific client item status mapping by Id.
        /// </summary>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="id">Mapping Id.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the requested mapping if found.</returns>
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var data = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);

            if (data == null)
                return ApiResponseFactory.NotFound<WMSCompanyClientItemStatusReadDTO>("The item status mapping was not found.");

            return ApiResponseFactory.Success(data, "Item status mapping retrieved successfully.");
        }

        /// <summary>
        /// Creates a new client mapping for a master item status.
        /// Validation rules:
        /// - Master item status must exist and be active.
        /// - Mapping must not already exist for the current client.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse containing the created mapping.</returns>
        public async Task<ApiResponse<WMSCompanyClientItemStatusReadDTO>> CreateAsync(
            WMSCompanyClientItemStatusCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var masterIsActive = await _repository.IsMasterActiveAsync(companyId, dto.ItemStatusId, ct);
            if (!masterIsActive)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                    "The selected item status does not exist or is inactive.");
            }

            var exists = await _repository.ExistsAsync(
                companyId,
                companyClientId,
                dto.ItemStatusId,
                null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.BadRequest<WMSCompanyClientItemStatusReadDTO>(
                    "This item status is already mapped to the current client.");
            }

            var created = await _repository.CreateAsync(dto, companyId, companyClientId, ct);

            return ApiResponseFactory.Success(created, "Item status mapping created successfully.");
        }

        /// <summary>
        /// Enables or disables an existing client mapping.
        /// If the request attempts to activate a mapping, the corresponding master item status must still be active.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="isActive">New active status for the mapping.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the operation succeeded.</returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var existing = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);
            if (existing == null)
            {
                return ApiResponseFactory.NotFound<bool>("The item status mapping was not found.");
            }

            if (isActive)
            {
                var masterIsActive = await _repository.IsMasterActiveAsync(companyId, existing.ItemStatusId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.BadRequest<bool>(
                        "The mapping cannot be activated because the master item status is inactive.");
                }
            }

            var updated = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);

            if (!updated)
            {
                return ApiResponseFactory.NotFound<bool>("The item status mapping was not found.");
            }

            return ApiResponseFactory.Success(true, "Item status mapping updated successfully.");
        }

        /// <summary>
        /// Replaces the enabled set for the current client.
        /// Validation rules:
        /// - Every provided master id must exist and be active.
        /// - Missing mappings are created before the enabled set is applied.
        /// - Existing mappings not included in the provided list are disabled.
        /// </summary>
        /// <param name="itemStatusIds">Final list of enabled master item status ids.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the operation succeeded.</returns>
        public async Task<ApiResponse<bool>> SetEnabledSetAsync(
            List<int> itemStatusIds,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            itemStatusIds ??= new List<int>();

            var distinctIds = itemStatusIds.Distinct().ToList();

            foreach (var itemStatusId in distinctIds)
            {
                var masterIsActive = await _repository.IsMasterActiveAsync(companyId, itemStatusId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.BadRequest<bool>(
                        $"The item status with id {itemStatusId} does not exist or is inactive.");
                }

                var exists = await _repository.ExistsAsync(
                    companyId,
                    companyClientId,
                    itemStatusId,
                    null,
                    ct);

                if (!exists)
                {
                    var createDto = new WMSCompanyClientItemStatusCreateDTO
                    {
                        ItemStatusId = itemStatusId,
                        IsActive = true
                    };

                    await _repository.CreateAsync(createDto, companyId, companyClientId, ct);
                }
            }

            var updated = await _repository.SetEnabledSetAsync(
                companyId,
                companyClientId,
                distinctIds,
                ct);

            return ApiResponseFactory.Success(updated, "Enabled item status set updated successfully.");
        }

        /// <summary>
        /// Deletes a client mapping by Id.
        /// Important:
        /// - This operation deletes only the client relation.
        /// - It does not delete the master item status.
        /// </summary>
        /// <param name="id">Mapping Id.</param>
        /// <param name="companyId">Company scope from token.</param>
        /// <param name="companyClientId">Company client scope from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the delete succeeded.</returns>
        public async Task<ApiResponse<object>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var exists = await _repository.GetByIdAsync(companyId, companyClientId, id, ct);
            if (exists == null)
            {
                return ApiResponseFactory.NotFound<object>("The item status mapping was not found.");
            }

            var deleted = await _repository.DeleteAsync(companyId, companyClientId, id, ct);

            if (!deleted)
            {
                return ApiResponseFactory.NotFound<object>("The item status mapping was not found.");
            }

            return ApiResponseFactory.Success<object>(null, "Item status mapping deleted successfully.");
        }
    }
}

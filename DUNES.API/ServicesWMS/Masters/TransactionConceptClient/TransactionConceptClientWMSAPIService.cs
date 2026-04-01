using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using System.Net;

namespace DUNES.API.ServicesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Service implementation for managing TransactionConceptClient mappings.
    /// Applies business rules on top of the repository layer for the mapping
    /// between a client and the master TransactionConcept catalog.
    /// </summary>
    public class TransactionConceptClientWMSAPIService : ITransactionConceptClientWMSAPIService
    {
        private readonly ITransactionConceptClientWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository for TransactionConceptClient mappings.</param>
        public TransactionConceptClientWMSAPIService(
            ITransactionConceptClientWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all transaction concept mappings for the current tenant scope.
        /// Includes both active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the list of mapped transaction concepts.
        /// </returns>
        public async Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                data,
                "Transaction concept mappings retrieved successfully.");
        }

        /// <summary>
        /// Gets a transaction concept mapping by Id for the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the requested mapping if found.
        /// </returns>
        public async Task<ApiResponse<WMSTransactionConceptClientReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetByIdAsync(id, companyId, companyClientId, ct);

            if (data is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Transaction concept client mapping was not found.");
            }

            return ApiResponseFactory.Success(
                data,
                "Transaction concept client mapping retrieved successfully.");
        }

        /// <summary>
        /// Creates a new TransactionConceptClient mapping.
        /// Business rules:
        /// - The master TransactionConcept must exist.
        /// - The mapping must not already exist for the same CompanyId + CompanyClientId + TransactionConceptId.
        /// - If the mapping is created as active, the master TransactionConcept must also be active.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the created mapping.
        /// </returns>
        public async Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            WMSTransactionConceptClientCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var masterExists = await _repository.MasterExistsAsync(dto.TransactionConceptId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                    error: "CONCEPT_NOT_EXIST",
                    message: "The selected Transaction Concept does not exist.",
                    statusCode: 400);
            }

            var existsMapping = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                dto.TransactionConceptId,
                excludeId: null,
                ct);

            if (existsMapping)
            {
                return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                 error: "DUPLICATE_MAPPING",
                 message: "A mapping for this Transaction Concept already exists for the selected client.",
                 statusCode: 400);



            }

           



            if (dto.Active)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(dto.TransactionConceptId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                        error: "MASTER_INACTIVE",
                       message: "The mapping cannot be created as active because the master Transaction Concept is inactive.",
                        statusCode: 400);
                }
            }

            var entity = new DUNES.API.ModelsWMS.Masters.TransactionConceptClient
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                TransactionConceptId = dto.TransactionConceptId,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);

            var createdDto = await _repository.GetByIdAsync(created.Id, companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                createdDto!,
                "Transaction concept client mapping created successfully.");
        }

        /// <summary>
        /// Updates an existing TransactionConceptClient mapping.
        /// Business rules:
        /// - The mapping must belong to the current tenant scope.
        /// - The referenced master TransactionConcept must exist.
        /// - The mapping must remain unique by CompanyId + CompanyClientId + TransactionConceptId.
        /// - If the mapping is updated as active, the master TransactionConcept must also be active.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the updated mapping.
        /// </returns>
        public async Task<ApiResponse<WMSTransactionConceptClientReadDTO>> UpdateAsync(
             int id, WMSTransactionConceptClientUpdateDTO dto,int companyId,
             int companyClientId,    CancellationToken ct)
        {
            var entity = await _repository.GetEntityByIdAsync(id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Transaction concept client mapping was not found.");
            }

            if (dto.Active)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(entity.TransactionConceptId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                        error: "MASTER_INACTIVE",
                        message: "The mapping cannot be updated as active because the master Transaction Concept is inactive.",
                        statusCode: 400);
                }
            }

            entity.Active = dto.Active;

            var updated = await _repository.UpdateAsync(entity, ct);

            if (!updated)
            {
                return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                    error: "NOT_UPDATE",
                    message: "The transaction concept client mapping could not be updated.",
                    statusCode: 500);
            }

            var updatedDto = await _repository.GetByIdAsync(id, companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                updatedDto!,
                "Transaction concept client mapping updated successfully.");
        }

        /// <summary>
        /// Deletes a TransactionConceptClient mapping by Id.
        /// Important:
        /// - This deletes only the client mapping.
        /// - It does not delete the master TransactionConcept.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response indicating whether the delete operation succeeded.
        /// </returns>
        public async Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = await _repository.GetEntityByIdAsync(id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Transaction concept client mapping was not found.");
            }

            var deleted = await _repository.DeleteAsync(entity, ct);

            if (!deleted)
            {
                return ApiResponseFactory.Fail<bool>(
                     error: "NOT_DELETE",
                    message: "The transaction concept client mapping could not be deleted.",
                    statusCode: 500);
            }

            return ApiResponseFactory.Success<bool>(
                true,
                message: "Transaction concept client mapping deleted successfully.");
        }

        /// <summary>
        /// Updates the active status of a TransactionConceptClient mapping.
        /// Business rules:
        /// - The mapping must belong to the current tenant scope.
        /// - If the new state is active, the master TransactionConcept must also be active.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="isActive">New mapping active status.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response indicating whether the active state was updated successfully.
        /// </returns>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = await _repository.GetEntityByIdAsync(id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Transaction concept client mapping was not found.");
            }

            if (isActive)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(entity.TransactionConceptId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.Fail<bool>(
                          error: "MASTER_INACTIVE",
                        message: "The mapping cannot be activated because the master Transaction Concept is inactive.",
                        statusCode: 400);
                }
            }

            var updated = await _repository.SetActiveAsync(
                companyId,
                companyClientId,
                id,
                isActive,
                ct);

            if (!updated)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "NOT_UPDATE",
                    message: "The active status could not be updated.",
                    statusCode: 500);
            }

            return ApiResponseFactory.Success(
                true,
                "Transaction concept client mapping status updated successfully.");
        }

        /// <summary>
        /// Gets the enabled transaction concepts for the current tenant scope.
        /// Only returns mappings where:
        /// - Mapping Active = true
        /// - Master TransactionConcept IsActive = true
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the list of enabled transaction concepts.
        /// </returns>
        public async Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetEnabledAsync(companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                data,
                "Enabled transaction concepts retrieved successfully.");
        }
    }
}
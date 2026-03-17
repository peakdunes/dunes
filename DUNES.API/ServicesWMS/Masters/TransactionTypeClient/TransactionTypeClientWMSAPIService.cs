using DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Service implementation for managing TransactionTypeClient mappings.
    /// Applies business rules on top of the repository layer for the mapping
    /// between a client and the master TransactionType catalog.
    /// </summary>
    public class TransactionTypeClientWMSAPIService : ITransactionTypeClientWMSAPIService
    {
        private readonly ITransactionTypeClientWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Repository for TransactionTypeClient mappings.</param>
        public TransactionTypeClientWMSAPIService(
            ITransactionTypeClientWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets all transaction type mappings for the current tenant scope.
        /// Includes both active and inactive mappings.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                data,
                "Transaction type mappings retrieved successfully.");
        }

        /// <summary>
        /// Gets a transaction type mapping by Id for the current tenant scope.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetByIdAsync(id, companyId, companyClientId, ct);

            if (data is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type client mapping was not found.");
            }

            return ApiResponseFactory.Success(
                data,
                "Transaction type client mapping retrieved successfully.");
        }

        /// <summary>
        /// Creates a new TransactionTypeClient mapping.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            WMSTransactionTypeClientCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var masterExists = await _repository.MasterExistsAsync(dto.TransactionTypeId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "TYPE_NOT_EXIST",
                    message: "The selected Transaction Type does not exist.",
                    statusCode: 400);
            }

            var existsMapping = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                dto.TransactionTypeId,
                excludeId: null,
                ct);

            if (existsMapping)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "A mapping for this Transaction Type already exists for the selected client.",
                    statusCode: 400);
            }

            if (dto.Active)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(dto.TransactionTypeId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                        error: "MASTER_INACTIVE",
                        message: "The mapping cannot be created as active because the master Transaction Type is inactive.",
                        statusCode: 400);
                }
            }

            var entity = new DUNES.API.ModelsWMS.Masters.TransactionTypeClient
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                TransactionTypeId = dto.TransactionTypeId,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);
            var createdDto = await _repository.GetByIdAsync(created.Id, companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                createdDto!,
                "Transaction type client mapping created successfully.");
        }

        /// <summary>
        /// Updates an existing TransactionTypeClient mapping.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionTypeClientUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var entity = await _repository.GetEntityByIdAsync(id, companyId, companyClientId, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type client mapping was not found.");
            }

            var masterExists = await _repository.MasterExistsAsync(dto.TransactionTypeId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "TYPE_NOT_FOUND",
                    message: "The selected Transaction Type does not exist.",
                    statusCode: 400);
            }

            var existsMapping = await _repository.ExistsMappingAsync(
                companyId,
                companyClientId,
                dto.TransactionTypeId,
                excludeId: id,
                ct);

            if (existsMapping)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "MAPPING_EXIST",
                    message: "A mapping for this Transaction Type already exists for the selected client.",
                    statusCode: 400);
            }

            if (dto.Active)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(dto.TransactionTypeId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                        error: "MASTER_INACTIVE",
                        message: "The mapping cannot be updated as active because the master Transaction Type is inactive.",
                        statusCode: 400);
                }
            }

            entity.TransactionTypeId = dto.TransactionTypeId;
            entity.Active = dto.Active;

            var updated = await _repository.UpdateAsync(entity, ct);

            if (!updated)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "NOT_UPDATE",
                    message: "The transaction type client mapping could not be updated.",
                    statusCode: 500);
            }

            var updatedDto = await _repository.GetByIdAsync(id, companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                updatedDto!,
                "Transaction type client mapping updated successfully.");
        }

        /// <summary>
        /// Deletes a TransactionTypeClient mapping by Id.
        /// </summary>
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
                    "Transaction type client mapping was not found.");
            }

            var deleted = await _repository.DeleteAsync(entity, ct);

            if (!deleted)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "NOT_DELETE",
                    message: "The transaction type client mapping could not be deleted.",
                    statusCode: 500);
            }

            return ApiResponseFactory.Success(
                true,
                "Transaction type client mapping deleted successfully.");
        }

        /// <summary>
        /// Updates the active status of a TransactionTypeClient mapping.
        /// </summary>
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
                    "Transaction type client mapping was not found.");
            }

            if (isActive)
            {
                var masterIsActive = await _repository.MasterIsActiveAsync(entity.TransactionTypeId, ct);
                if (!masterIsActive)
                {
                    return ApiResponseFactory.Fail<bool>(
                        error: "MASTER_INACTIVE",
                        message: "The mapping cannot be activated because the master Transaction Type is inactive.",
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
                "Transaction type client mapping status updated successfully.");
        }

        /// <summary>
        /// Gets the enabled transaction types for the current tenant scope.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetEnabledAsync(companyId, companyClientId, ct);

            return ApiResponseFactory.Success(
                data,
                "Enabled transaction types retrieved successfully.");
        }
    }
}
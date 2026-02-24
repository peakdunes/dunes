using DUNES.API.RepositoriesWMS.Masters.ClientCompanies;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.TransactionsType;
using DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;
using Microsoft.AspNetCore.Http;

namespace DUNES.API.ServicesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Transaction Type Client service implementation.
    ///
    /// Enforces business rules for mappings between
    /// Transaction Types (master) and Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller (from token).
    /// - The service NEVER reads claims directly.
    /// - The service validates master existence and duplicate mappings.
    /// </summary>
    public class TransactionTypeClientWMSAPIService : ITransactionTypeClientWMSAPIService
    {
        private readonly ITransactionTypeClientWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">Transaction Type Client repository.</param>
        public TransactionTypeClientWMSAPIService(ITransactionTypeClientWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all Transaction Type mappings for a specific CompanyClient.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the list of mappings.</returns>
        public async Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var data = await _repository.GetByClientAsync(companyId, companyClientId, ct);
            return ApiResponseFactory.Success(data);
        }

        /// <summary>
        /// Creates a new Transaction Type mapping for a CompanyClient.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="dto">Create DTO for the mapping.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the created mapping.</returns>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct)
        {
            // Validate master belongs to same tenant
            var masterExists = await _repository.MasterExistsAsync(companyId, dto.TransactionTypeId, ct);
            if (!masterExists)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "MASTER_NOT_FOUND",
                    message: "Transaction type not found for this company.",
                    statusCode: StatusCodes.Status404NotFound);
            }

            // Validate duplicate mapping
            var exists = await _repository.ExistsAsync(
                companyId,
                dto.CompanyClientId,
                dto.TransactionTypeId,
                excludeId: null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This transaction type is already assigned to the client.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            var entity = new ModelsWMS.Masters.TransactionTypeClient
            {
                CompanyId = companyId,
                CompanyClientId = dto.CompanyClientId,
                TransactionTypeId = dto.TransactionTypeId,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);

            // Return consistent ReadDTO using repository projection
            var list = await _repository.GetByClientAsync(companyId, created.CompanyClientId, ct);
            var createdDto = list.FirstOrDefault(x => x.Id == created.Id);

            if (createdDto is null)
            {
                // Fallback (should not happen, but don't crash)
                createdDto = new WMSTransactionTypeClientReadDTO
                {
                    Id = created.Id,
                    CompanyClientId = created.CompanyClientId,
                    TransactionTypeId = created.TransactionTypeId,
                    Active = created.Active
                };
            }

            return ApiResponseFactory.Success(createdDto);
        }

        /// <summary>
        /// Activates or deactivates an existing mapping.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="isActive">New active state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the updated mapping.</returns>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _repository.GetEntityByIdAsync(companyId, companyClientId, id, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type mapping not found.");
            }

            var updated = await _repository.SetActiveAsync(companyId, companyClientId, id, isActive, ct);
            if (!updated)
            {
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type mapping not found.");
            }

            var list = await _repository.GetByClientAsync(companyId, companyClientId, ct);
            var dto = list.FirstOrDefault(x => x.Id == id);

            if (dto is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type mapping not found after update.");
            }

            return ApiResponseFactory.Success(dto);
        }

        /// <summary>
        /// Deletes a mapping physically.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the mapping was deleted.</returns>
        public async Task<ApiResponse<bool>> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var entity = await _repository.GetEntityByIdAsync(companyId, companyClientId, id, ct);
            if (entity is null)
            {
                return ApiResponseFactory.NotFound<bool>("Transaction type mapping not found.");
            }

            var deleted = await _repository.DeleteAsync(companyId, companyClientId, id, ct);
            if (!deleted)
            {
                return ApiResponseFactory.NotFound<bool>("Transaction type mapping not found.");
            }

            return ApiResponseFactory.Success(true);
        }
    }
}

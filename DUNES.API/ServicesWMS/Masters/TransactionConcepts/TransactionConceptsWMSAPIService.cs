using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.TransactionConcepts;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.TransactionConcepts
{
    /// <summary>
    /// Transaction Concepts service implementation.
    /// 
    /// This service is responsible for enforcing all business rules
    /// related to Transaction Concepts before interacting with the
    /// repository layer.
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller.
    /// - The service NEVER reads claims or request headers.
    /// - The service validates ownership and invariants before persistence.
    /// </summary>
    public class TransactionConceptsWMSAPIService : ITransactionConceptsWMSAPIService
    {
        private readonly ITransactionConceptsWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptsWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">
        /// Transaction concepts repository injected via dependency injection.
        /// </param>
        public TransactionConceptsWMSAPIService(ITransactionConceptsWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all transaction concepts for the specified company.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionconceptsReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, ct);

            var result = data
                .Select(MapToReadDto)
                .ToList();

            return ApiResponseFactory.Success(result);
        }

        /// <summary>
        /// Retrieves all active transaction concepts for the specified company.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionconceptsReadDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(companyId, ct);

            var result = data
                .Select(MapToReadDto)
                .ToList();

            return ApiResponseFactory.Success(result);
        }

        /// <summary>
        /// Retrieves a transaction concept by its identifier, validating ownership.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionconceptsReadDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(companyId, id, ct);

            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionconceptsReadDTO>(
                    "Transaction concept not found.");
            }

            return ApiResponseFactory.Success(MapToReadDto(entity));
        }

        /// <summary>
        /// Creates a new transaction concept.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionconceptsReadDTO>> CreateAsync(
            WMSTransactionconceptsCreateDTO request,
            int companyId,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return ApiResponseFactory.Fail<WMSTransactionconceptsReadDTO>(
                    error: "VALIDATION_ERROR",
                    message: "Name is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var normalizedName = request.Name.Trim();

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                normalizedName,
                excludeId: null,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSTransactionconceptsReadDTO>(
                    error: "DUPLICATE_NAME",
                    message: "A transaction concept with the same name already exists.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            var entity = new Transactionconcepts
            {
                Name = normalizedName,
                companyId = companyId, // enforced from token context
                Observations = string.IsNullOrWhiteSpace(request.Observations)
                    ? null
                    : request.Observations.Trim(),
                Active = request.Active // si tu CreateDTO no trae Active, pon true fijo
            };

            var created = await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Success(MapToReadDto(created), "Transaction concept created");
        }

        /// <summary>
        /// Updates an existing transaction concept.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionconceptsReadDTO>> UpdateAsync(
            int id,
            WMSTransactionconceptsUpdateDTO request,
            int companyId,
            CancellationToken ct)
        {
            var existing = await _repository.GetByIdAsync(companyId, id, ct);

            if (existing is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionconceptsReadDTO>(
                    "Transaction concept not found.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return ApiResponseFactory.Fail<WMSTransactionconceptsReadDTO>(
                    error: "VALIDATION_ERROR",
                    message: "Name is required.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var normalizedName = request.Name.Trim();

            var exists = await _repository.ExistsByNameAsync(
                companyId,
                normalizedName,
                excludeId: id,
                ct);

            if (exists)
            {
                return ApiResponseFactory.Fail<WMSTransactionconceptsReadDTO>(
                    error: "DUPLICATE_NAME",
                    message: "A transaction concept with the same name already exists.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            // Apply allowed updates only
            existing.Name = normalizedName;
            existing.Observations = string.IsNullOrWhiteSpace(request.Observations)
                ? null
                : request.Observations.Trim();

            // Si tu UpdateDTO incluye Active, puedes dejar esta línea.
            // Si NO lo incluye, elimínala.
            existing.Active = request.Active;

            var updated = await _repository.UpdateAsync(existing, ct);

            return ApiResponseFactory.Success(MapToReadDto(updated),"Transaction concept updated");
        }

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionconceptsReadDTO>> SetActiveAsync(
            WMSTransactionconceptsSetActiveDTO request,
            int companyId,
            CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(companyId, request.Id, ct);

            if (entity is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionconceptsReadDTO>(
                    "Transaction concept not found.");
            }

            var ok = await _repository.SetActiveAsync(
                companyId,
                request.Id,
                request.Active,
                ct);

            if (!ok)
            {
                return ApiResponseFactory.NotFound<WMSTransactionconceptsReadDTO>(
                    "Transaction concept not found.");
            }

            // Re-read to return current state
            var updated = await _repository.GetByIdAsync(companyId, request.Id, ct);

            if (updated is null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionconceptsReadDTO>(
                    "Transaction concept not found after update.");
            }

            return ApiResponseFactory.Success(MapToReadDto(updated),"Transaction concept updated");
        }

        /// <summary>
        /// Deletes a transaction concept from the master catalog.
        /// </summary>
        public async Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            CancellationToken ct)
        {
            // opcional: validar existencia primero para responder 404 limpio
            var existing = await _repository.GetByIdAsync(companyId, id, ct);
            if (existing is null)
            {
                return ApiResponseFactory.NotFound<bool>(
                    "Transaction concept not found.");
            }

            // Si agregaste HasDependenciesAsync en repo (recomendado), úsalo aquí.
            // Si aún no lo has agregado, comenta este bloque hasta implementarlo.
            var hasDependencies = await _repository.HasDependenciesAsync(companyId, id, ct);
            if (hasDependencies)
            {
                return ApiResponseFactory.Fail<bool>(
                    error: "HAS_DEPENDENCIES",
                    message: "The transaction concept cannot be deleted because it has related records.",
                    statusCode: StatusCodes.Status409Conflict);
            }

            var deleted = await _repository.DeleteAsync(companyId, id, ct);

            return ApiResponseFactory.Success(deleted,"Concept delete successfull");
        }

        /// <summary>
        /// Maps entity to read DTO.
        /// </summary>
        private static WMSTransactionconceptsReadDTO MapToReadDto(Transactionconcepts entity)
        {
            return new WMSTransactionconceptsReadDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                CompanyId = entity.companyId,
                Observations = entity.Observations,
                Active = entity.Active
            };
        }
    }
}
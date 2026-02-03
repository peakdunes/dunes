using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.TransactionConcepts;
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
    public class TransactionConceptsWMSAPIService
        : ITransactionConceptsWMSAPIService
    {
        private readonly ITransactionConceptsWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptsWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">
        /// Transaction concepts repository injected via dependency injection.
        /// </param>
        public TransactionConceptsWMSAPIService(
            ITransactionConceptsWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all transaction concepts for the specified company.
        /// </summary>
        public async Task<ApiResponse<List<Transactionconcepts>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            var data = await _repository.GetAllAsync(companyId, ct);
            return ApiResponseFactory.Success(data);
        }

        /// <summary>
        /// Retrieves all active transaction concepts for the specified company.
        /// </summary>
        public async Task<ApiResponse<List<Transactionconcepts>>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            var data = await _repository.GetActiveAsync(companyId, ct);
            return ApiResponseFactory.Success(data);
        }

        /// <summary>
        /// Retrieves a transaction concept by its identifier,
        /// validating ownership.
        /// </summary>
        public async Task<ApiResponse<Transactionconcepts>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(companyId, id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<Transactionconcepts>(
                    "Transaction concept not found.");

            return ApiResponseFactory.Success(entity);
        }

        /// <summary>
        /// Creates a new transaction concept.
        /// </summary>
        public async Task<ApiResponse<Transactionconcepts>> CreateAsync(
            int companyId,
            Transactionconcepts entity,
            CancellationToken ct)
        {
            // Enforce ownership from authenticated context
            entity.companyId = companyId;

            // Default state
            entity.Active = true;

            // Validate uniqueness
            var exists = await _repository.ExistsByNameAsync(
                companyId,
                entity.Name,
                excludeId: null,
                ct);

            if (exists)
                return ApiResponseFactory.Fail<Transactionconcepts>(
    error: "DUPLICATE_NAME",
    message: "A transaction concept with the same name already exists.",
    statusCode: StatusCodes.Status409Conflict);

            var created = await _repository.CreateAsync(entity, ct);
            return ApiResponseFactory.Success(created);
        }

        /// <summary>
        /// Updates an existing transaction concept.
        /// </summary>
        public async Task<ApiResponse<Transactionconcepts>> UpdateAsync(
            int companyId,
            int id,
            Transactionconcepts entity,
            CancellationToken ct)
        {
            var existing = await _repository.GetByIdAsync(companyId, id, ct);

            if (existing is null)
                return ApiResponseFactory.NotFound<Transactionconcepts>(
                    "Transaction concept not found.");

            // Enforce invariants
            if (existing.companyId != companyId)
                return ApiResponseFactory.Forbidden<Transactionconcepts>(
                    "You are not allowed to modify this record.");

            // Validate uniqueness (exclude current record)
            var exists = await _repository.ExistsByNameAsync(
                companyId,
                entity.Name,
                excludeId: id,
                ct);

            if (exists)
                return ApiResponseFactory.Fail<Transactionconcepts>(
                      error: "DUPLICATE_NAME",
                      message: "A transaction concept with the same name already exists.",
                      StatusCodes.Status409Conflict);

            // Apply allowed updates
            existing.Name = entity.Name;
            existing.Observations = entity.Observations;
            existing.Active = entity.Active;

            var updated = await _repository.UpdateAsync(existing, ct);
            return ApiResponseFactory.Success(updated);
        }

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// </summary>
        public async Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var updated = await _repository.SetActiveAsync(
                companyId,
                id,
                isActive,
                ct);

            if (!updated)
                return ApiResponseFactory.NotFound<bool>(
                    "Transaction concept not found.");

            return ApiResponseFactory.Success(true);
        }
    }
}

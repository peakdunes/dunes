using DUNES.API.ModelsWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.TransactionsType;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;


namespace DUNES.API.ServicesWMS.Masters.TransactionsType
{

    /// <summary>
    /// Transaction Types service implementation.
    ///
    /// This service enforces all business rules related to
    /// Transaction Types before interacting with the repository layer.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller.
    /// - The service NEVER reads claims or headers.
    /// - The service validates ownership and invariants.
    /// </summary>
    public class TransactionsTypeWMSAPIService  : ITransactionsTypeWMSAPIService
    {
        private readonly ITransactionTypesWMSAPIRepository _repository;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TransactionsTypeWMSAPIService"/> class.
        /// </summary>
        /// <param name="repository">
        /// Transaction Types repository injected via DI.
        /// </param>
        public TransactionsTypeWMSAPIService(
            ITransactionTypesWMSAPIRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all transaction types for the specified company.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactiontypesReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            var entities = await _repository.GetAllAsync(companyId, ct);

            var data = entities.Select(MapToReadDto).ToList();

            return ApiResponseFactory.Success(data);
        }

        /// <summary>
        /// Retrieves all active transaction types for the specified company.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactiontypesReadDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            var entities = await _repository.GetActiveAsync(companyId, ct);

            var data = entities.Select(MapToReadDto).ToList();

            return ApiResponseFactory.Success(data);
        }

        /// <summary>
        /// Retrieves a transaction type by its identifier,
        /// validating company ownership.
        /// </summary>
        public async Task<ApiResponse<WMSTransactiontypesReadDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            var entity = await _repository.GetByIdAsync(companyId, id, ct);

            if (entity is null)
                return ApiResponseFactory.NotFound<WMSTransactiontypesReadDTO>(
                    "Transaction type not found.");

            return ApiResponseFactory.Success(MapToReadDto(entity));
        }

        /// <summary>
        /// Creates a new transaction type.
        /// </summary>
        public async Task<ApiResponse<WMSTransactiontypesCreateDTO>> CreateAsync(
            int companyId,
            WMSTransactiontypesCreateDTO dto,
            CancellationToken ct)
        {
            // Validate uniqueness within company
            var exists = await _repository.ExistsByNameAsync(
                companyId,
                dto.Name!,
                excludeId: null,
                ct);

            if (exists)
                return ApiResponseFactory.Fail<WMSTransactiontypesCreateDTO>(
                    error: "DUPLICATE_NAME",
                    message: "A transaction type with the same name already exists.",
                    statusCode: StatusCodes.Status409Conflict);

            var entity = new Transactiontypes
            {
                // 🔐 STANDARD COMPANYID
                companyId = companyId,

                Name = dto.Name,
                Isinput = dto.Isinput,
                Isoutput = dto.Isoutput,
                Active = dto.Active,
                Match = dto.Match
            };

            await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Success(
                dto,
                "Transaction type created successfully.");
        }

        /// <summary>
        /// Updates an existing transaction type.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionTypesUpdateDTO>> UpdateAsync(
            int companyId,
            int id,
            WMSTransactionTypesUpdateDTO dto,
            CancellationToken ct)
        {
            var existing = await _repository.GetByIdAsync(companyId, id, ct);

            if (existing is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypesUpdateDTO>(
                    "Transaction type not found.");

            // Ownership invariant (extra safety)
            if (existing.companyId != companyId)
                return ApiResponseFactory.Forbidden<WMSTransactionTypesUpdateDTO>(
                    "You are not allowed to modify this record.");

            // Validate uniqueness (exclude current)
            var exists = await _repository.ExistsByNameAsync(
                companyId,
                dto.Name!,
                excludeId: id,
                ct);

            if (exists)
                return ApiResponseFactory.Fail<WMSTransactionTypesUpdateDTO>(
                    error: "DUPLICATE_NAME",
                    message: "A transaction type with the same name already exists.",
                    statusCode: StatusCodes.Status409Conflict);

            // Apply allowed updates
            existing.Name = dto.Name;
            existing.Isinput = dto.Isinput;
            existing.Isoutput = dto.Isoutput;
            existing.Active = dto.Active;
            existing.Match = dto.Match;

            await _repository.UpdateAsync(existing, ct);

            return ApiResponseFactory.Success(
                dto,
                "Transaction type updated successfully.");
        }

        /// <summary>
        /// Activates or deactivates a transaction type.
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
                    "Transaction type not found.");

            return ApiResponseFactory.Success(true);
        }

        /// <summary>
        /// Maps a <see cref="Transactiontypes"/> entity
        /// to <see cref="WMSTransactiontypesReadDTO"/>.
        /// </summary>
        private static WMSTransactiontypesReadDTO MapToReadDto(
            Transactiontypes entity)
        {
            return new WMSTransactiontypesReadDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Isinput = entity.Isinput,
                Isoutput = entity.Isoutput,
                Active = entity.Active,
                Match = entity.Match
            };
        }
    }
}

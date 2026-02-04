using DUNES.API.RepositoriesWMS.Masters.ClientCompanies;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient;
using DUNES.API.RepositoriesWMS.Masters.TransactionConcepts;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.Shared.Utils.Reponse;

namespace DUNES.API.ServicesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Transaction Concept Client service implementation.
    ///
    /// This service manages the association between Transaction Concepts
    /// and Company Clients, enforcing all business rules before
    /// interacting with the repository layer.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller.
    /// - Ownership and invariants are validated here.
    /// </summary>
    public class TransactionConceptClientWMSAPIService
        : ITransactionConceptClientWMSAPIService
    {
        private readonly ITransactionConceptClientWMSAPIRepository _repository;
        private readonly ITransactionConceptsWMSAPIRepository _transactionConceptRepository;
        private readonly IClientCompaniesWMSAPIRepository _companyClientRepository;
        private readonly ICompaniesWMSAPIRepository _companyRepository;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TransactionConceptClientWMSAPIService"/> class.
        /// </summary>
        public TransactionConceptClientWMSAPIService(
            ITransactionConceptClientWMSAPIRepository repository,
            ITransactionConceptsWMSAPIRepository transactionConceptRepository,
            IClientCompaniesWMSAPIRepository companyClientRepository,
             ICompaniesWMSAPIRepository companyRepository)
        {
            _repository = repository;
            _transactionConceptRepository = transactionConceptRepository;
            _companyClientRepository = companyClientRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Retrieves all Transaction Concept associations
        /// for a specific CompanyClient.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {

            var companyinfo = await _companyRepository.GetByIdAsync(companyId, ct);

            if (companyinfo == null)
            {
                return ApiResponseFactory.NotFound<List<WMSTransactionConceptClientReadDTO>>(
                   "Company not found.");
            }

            // Validate CompanyClient existence
            var companyClient = await _companyClientRepository
                .GetClientCompanyInformationByIdAsync(companyClientId, ct);

            if (companyClient is null)
                return ApiResponseFactory.NotFound<List<WMSTransactionConceptClientReadDTO>>(
                    "Company client not found.");

            //// Validate active contract
            //var hasActiveContract = await _companyClientRepository
            //    .HasActiveContractAsync(companyId, companyClientId, ct);

            //if (!hasActiveContract)
            //    return ApiResponseFactory.Forbidden<List<WMSTransactionConceptClientReadDTO>>(
            //        "Company client is not associated with this company.");

            var mappings = await _repository.GetAllByClientAsync(
                companyId,
                companyClientId,
                ct);

            var result = new List<WMSTransactionConceptClientReadDTO>();

            foreach (var map in mappings)
            {
                var concept = await _transactionConceptRepository.GetByIdAsync(
                    companyId,
                    map.TransactionConceptId,
                    ct);

                if (concept is null)
                    continue;

                result.Add(new WMSTransactionConceptClientReadDTO
                {
                    Id = map.Id,
                    CompanyId = companyId,
                    CompanyName = companyinfo.Name ?? string.Empty,
                    CompanyClientId = companyClientId,
                    CompanyClientName = companyClient.Name,
                    TransactionConceptId = concept.Id,
                    TransactionConceptName = concept.Name,
                    Active = map.Active
                });
            }

            return ApiResponseFactory.Success(result);
        }

        /// <summary>
        /// Creates a new Transaction Concept association
        /// for a CompanyClient.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionConceptClientCreateDTO dto,
            CancellationToken ct)
        {
            var companyinfo = await _companyRepository.GetByIdAsync(companyId, ct);

            if (companyinfo == null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                   "Company not found.");
            }


            // 1️⃣ Validate CompanyClient existence
            var companyClient = await _companyClientRepository
                .GetClientCompanyInformationByIdAsync(dto.CompanyClientId, ct);

            if (companyClient is null)
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Company client not found.");

            // 2️⃣ Validate CompanyClient is active
            if (!companyClient.Active)
                return ApiResponseFactory.Forbidden<WMSTransactionConceptClientReadDTO>(
                    "Company client is inactive.");

            //// 3️⃣ Validate active contract
            //var hasActiveContract = await _companyClientRepository
            //    .HasActiveContractAsync(companyId, dto.CompanyClientId, ct);

            //if (!hasActiveContract)
            //    return ApiResponseFactory.Forbidden<WMSTransactionConceptClientReadDTO>(
            //        "Company client is not associated with this company.");

            // 4️⃣ Validate Transaction Concept ownership
            var concept = await _transactionConceptRepository.GetByIdAsync(
                companyId,
                dto.TransactionConceptId,
                ct);

            if (concept is null)
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Transaction concept not found.");

            // 5️⃣ Prevent duplicate mapping
            var exists = await _repository.ExistsAsync(
                companyId,
                dto.CompanyClientId,
                dto.TransactionConceptId,
                ct);

            if (exists)
                return ApiResponseFactory.Fail<WMSTransactionConceptClientReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This transaction concept is already assigned to the company client.",
                    statusCode: StatusCodes.Status409Conflict);

            // 6️⃣ Create mapping
            var entity = new DUNES.API.ModelsWMS.Masters.TransactionConceptClient
            {
                CompanyId = companyId,
                CompanyClientId = dto.CompanyClientId,
                TransactionConceptId = dto.TransactionConceptId,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Success(new WMSTransactionConceptClientReadDTO
            {
                Id = created.Id,
                CompanyId = companyId,
                CompanyName = companyinfo.Name ?? string.Empty,
                CompanyClientId = companyClient.Id,
                CompanyClientName = companyClient.Name,
                TransactionConceptId = concept.Id,
                TransactionConceptName = concept.Name,
                Active = created.Active
            });
        }

        /// <summary>
        /// Updates an existing Transaction Concept association.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionConceptClientReadDTO>> UpdateAsync(
            int companyId,
            WMSTransactionConceptClientUpdateDTO dto,
            CancellationToken ct)
        {

            var companyinfo = await _companyRepository.GetByIdAsync(companyId, ct);

            if (companyinfo == null)
            {
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                   "Company not found.");
            }


            // 1️⃣ Validate mapping using full key
            var existing = await _repository.GetByIdAsync(
                companyId,
                dto.CompanyClientId,
                dto.Id,
                ct);

            if (existing is null)
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Transaction concept mapping not found.");

            // 2️⃣ Validate CompanyClient
            var companyClient = await _companyClientRepository
                .GetClientCompanyInformationByIdAsync(dto.CompanyClientId, ct);

            if (companyClient is null)
                return ApiResponseFactory.NotFound<WMSTransactionConceptClientReadDTO>(
                    "Company client not found.");

            if (!companyClient.Active)
                return ApiResponseFactory.Forbidden<WMSTransactionConceptClientReadDTO>(
                    "Company client is inactive.");

            //// 3️⃣ Validate active contract (defensive)
            //var hasActiveContract = await _companyClientRepository
            //    .HasActiveContractAsync(companyId, dto.CompanyClientId, ct);

            //if (!hasActiveContract)
            //    return ApiResponseFactory.Forbidden<WMSTransactionConceptClientReadDTO>(
            //        "Company client is not associated with this company.");

            // 4️⃣ Apply update
            existing.Active = dto.Active;

            var updated = await _repository.UpdateAsync(existing, ct);

            // 5️⃣ Load Transaction Concept
            var concept = await _transactionConceptRepository.GetByIdAsync(
                companyId,
                updated.TransactionConceptId,
                ct);

            return ApiResponseFactory.Success(new WMSTransactionConceptClientReadDTO
            {
                Id = updated.Id,
                CompanyId = companyId,
                CompanyName = companyinfo.Name ?? string.Empty,
                CompanyClientId = updated.CompanyClientId,
                CompanyClientName = companyClient.Name,
                TransactionConceptId = updated.TransactionConceptId,
                TransactionConceptName = concept?.Name ?? string.Empty,
                Active = updated.Active
            });
        }
    }
}

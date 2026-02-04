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
    /// This service manages the association between Transaction Types
    /// and Company Clients, enforcing all business rules before
    /// interacting with the repository layer.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller.
    /// - The service NEVER reads claims or headers.
    /// - Ownership and invariants are validated here.
    /// </summary>
    public class TransactionTypeClientWMSAPIService
        : ITransactionTypeClientWMSAPIService
    {
        private readonly ITransactionTypeClientWMSAPIRepository _repository;
        private readonly ITransactionTypesWMSAPIRepository _transactionTypeRepository;
        private readonly ICompaniesWMSAPIRepository _companyRepository;
        private readonly IClientCompaniesWMSAPIRepository _companyClientRepository;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TransactionTypeClientWMSAPIService"/> class.
        /// </summary>
        public TransactionTypeClientWMSAPIService(
            ITransactionTypeClientWMSAPIRepository repository,
            ITransactionTypesWMSAPIRepository transactionTypeRepository,
            ICompaniesWMSAPIRepository companyRepository,
            IClientCompaniesWMSAPIRepository companyClientRepository)
        {
            _repository = repository;
            _transactionTypeRepository = transactionTypeRepository;
            _companyRepository = companyRepository;
            _companyClientRepository = companyClientRepository;
        }

        /// <summary>
        /// Retrieves all Transaction Type associations
        /// for a specific CompanyClient.
        /// </summary>
        public async Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            var company = await _companyRepository.GetByIdAsync(companyId, ct);
            if (company is null)
                return ApiResponseFactory.NotFound<List<WMSTransactionTypeClientReadDTO>>(
                    "Company not found.");

            var companyClient = await _companyClientRepository
                .GetClientCompanyInformationByIdAsync(companyClientId, ct);

            if (companyClient is null)
                return ApiResponseFactory.NotFound<List<WMSTransactionTypeClientReadDTO>>(
                    "Company client not found.");

            var mappings = await _repository.GetAllByClientAsync(
                companyId,
                companyClientId,
                ct);

            var result = new List<WMSTransactionTypeClientReadDTO>();

            foreach (var map in mappings)
            {
                var type = await _transactionTypeRepository.GetByIdAsync(
                    companyId,
                    map.TransactionTypeId,
                    ct);

                if (type is null)
                    continue;

                result.Add(new WMSTransactionTypeClientReadDTO
                {
                    Id = map.Id,
                    CompanyId = companyId,
                    CompanyName = company.Name,
                    CompanyClientId = companyClientId,
                    CompanyClientName = companyClient.Name,
                    TransactionTypeId = type.Id,
                    TransactionTypeName = type.Name,
                    Active = map.Active
                });
            }

            return ApiResponseFactory.Success(result);
        }

        /// <summary>
        /// Creates a new Transaction Type association
        /// for a CompanyClient.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct)
        {

            var company = await _companyRepository.GetByIdAsync(companyId, ct);
            if (company is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Company client not found.");

            // 2️⃣ Validate CompanyClient is active
            if (!company.Active)
                return ApiResponseFactory.Forbidden<WMSTransactionTypeClientReadDTO>(
                    "Company client is inactive.");


            // 1️⃣ Validate CompanyClient existence
            var companyClient = await _companyClientRepository
                .GetClientCompanyInformationByIdAsync(dto.CompanyClientId, ct);

            if (companyClient is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Company client not found.");

            // 2️⃣ Validate CompanyClient is active
            if (!companyClient.Active)
                return ApiResponseFactory.Forbidden<WMSTransactionTypeClientReadDTO>(
                    "Company client is inactive.");

            //// 3️⃣ Validate active contract Company ↔ CompanyClient
            //var hasActiveContract = await _companyClientRepository
            //    .HasActiveContractAsync(companyId, dto.CompanyClientId, ct);

            //if (!hasActiveContract)
            //    return ApiResponseFactory.Forbidden<WMSTransactionTypeClientReadDTO>(
            //        "Company client is not associated with this company.");

            // 4️⃣ Validate TransactionType ownership
            var transactionType = await _transactionTypeRepository.GetByIdAsync(
                companyId,
                dto.TransactionTypeId,
                ct);

            if (transactionType is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type not found.");

            // 5️⃣ Prevent duplicate mapping
            var exists = await _repository.ExistsAsync(
                companyId,
                dto.CompanyClientId,
                dto.TransactionTypeId,
                ct);

            if (exists)
                return ApiResponseFactory.Fail<WMSTransactionTypeClientReadDTO>(
                    error: "DUPLICATE_MAPPING",
                    message: "This transaction type is already assigned to the company client.",
                    statusCode: StatusCodes.Status409Conflict);

            // 6️⃣ Create mapping
            var entity = new DUNES.API.ModelsWMS.Masters.TransactionTypeClient
            {
                CompanyId = companyId,
                CompanyClientId = dto.CompanyClientId,
                TransactionTypeId = dto.TransactionTypeId,
                Active = dto.Active
            };

            var created = await _repository.CreateAsync(entity, ct);

            return ApiResponseFactory.Success(new WMSTransactionTypeClientReadDTO
            {
                Id = created.Id,
                CompanyId = companyId,
                CompanyName = company.Name ?? string.Empty,
                CompanyClientId = companyClient.Id,
                CompanyClientName = companyClient.Name,
                TransactionTypeId = transactionType.Id,
                TransactionTypeName = transactionType.Name,
                Active = created.Active
            });
        }

        /// <summary>
        /// Updates an existing Transaction Type association.
        /// </summary>
        public async Task<ApiResponse<WMSTransactionTypeClientReadDTO>> UpdateAsync(
            int companyId,
            WMSTransactionTypeClientUpdateDTO dto,
            CancellationToken ct)
        {

            var company = await _companyRepository.GetByIdAsync(companyId, ct);
            if (company is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Company client not found.");

            // 2️⃣ Validate CompanyClient is active
            if (!company.Active)
                return ApiResponseFactory.Forbidden<WMSTransactionTypeClientReadDTO>(
                    "Company client is inactive.");




            // 1️⃣ Validate if existe transactoin type
            var existing = await _repository.GetByIdAsync(
                companyId,
                dto.Id,
                ct);

            if (existing is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Transaction type mapping not found.");

            // 2️⃣ Validate CompanyClient existence
            var companyClient = await _companyClientRepository
                .GetClientCompanyInformationByIdAsync(existing.CompanyClientId, ct);

            if (companyClient is null)
                return ApiResponseFactory.NotFound<WMSTransactionTypeClientReadDTO>(
                    "Company client not found.");

            // 3️⃣ Validate CompanyClient is active
            if (!companyClient.Active)
                return ApiResponseFactory.Forbidden<WMSTransactionTypeClientReadDTO>(
                    "Company client is inactive.");

            //// 4️⃣ Validate active contract (defensive)
            //var hasActiveContract = await _companyClientRepository
            //    .HasActiveContractAsync(companyId, existing.CompanyClientId, ct);

            //if (!hasActiveContract)
            //    return ApiResponseFactory.Forbidden<WMSTransactionTypeClientReadDTO>(
            //        "Company client is not associated with this company.");

            // 5️⃣ Apply update
            existing.Active = dto.Active;

            var updated = await _repository.UpdateAsync(existing, ct);

            // 6️⃣ Load TransactionType for Read DTO
            var transactionType = await _transactionTypeRepository.GetByIdAsync(
                companyId,
                updated.TransactionTypeId,
                ct);

            return ApiResponseFactory.Success(new WMSTransactionTypeClientReadDTO
            {
                Id = updated.Id,
                CompanyId = companyId,
                CompanyName = company.Name ?? string.Empty,
                CompanyClientId = updated.CompanyClientId,
                CompanyClientName = companyClient.Name,
                TransactionTypeId = updated.TransactionTypeId,
                TransactionTypeName = transactionType?.Name ?? string.Empty,
                Active = updated.Active
            });
        }
    }
}

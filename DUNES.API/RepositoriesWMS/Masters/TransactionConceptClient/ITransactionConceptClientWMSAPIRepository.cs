using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Repository for TransactionConceptClient mappings.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - All queries MUST be scoped by CompanyId.
    /// - CompanyClientId must also be validated in every mapping operation.
    /// - Repository never infers tenant scope.
    /// </summary>
    public interface ITransactionConceptClientWMSAPIRepository
    {
        /// <summary>
        /// Gets all mappings for a specific company client.
        /// Includes master concept info for display.
        /// </summary>
        Task<List<WMSTransactionConceptClientReadDTO>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets one mapping entity by Id, scoped by Company and CompanyClient.
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient?> GetEntityByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Validates that the CompanyClient exists and belongs to the same Company.
        /// This is required to enforce tenant/client scope before mapping operations.
        /// </summary>
        Task<bool> CompanyClientExistsAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Checks if the mapping already exists for the combination
        /// (CompanyId, CompanyClientId, TransactionConceptId).
        /// </summary>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionConceptId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Validates that the master Transaction Concept exists
        /// and belongs to the same Company.
        /// </summary>
        Task<bool> MasterExistsAsync(
            int companyId,
            int transactionConceptId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new mapping.
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct);

        /// <summary>
        /// Updates Active flag only (patch style).
        /// </summary>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Deletes a mapping physically (optional, if business allows).
        /// </summary>
        Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);
    }
}

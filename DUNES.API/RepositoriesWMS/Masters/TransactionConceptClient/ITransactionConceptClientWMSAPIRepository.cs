namespace DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Transaction Concept Client repository interface.
    ///
    /// This repository manages the relationship between
    /// Transaction Concepts (master) and Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - All operations MUST be scoped by CompanyId.
    /// - This repository is the last line of defense
    ///   for multi-tenant data isolation.
    /// </summary>
    public interface ITransactionConceptClientWMSAPIRepository
    {
        /// <summary>
        /// Retrieves all Transaction Concept mappings
        /// for a specific CompanyClient.
        /// </summary>
        Task<List<DUNES.API.ModelsWMS.Masters.TransactionConceptClient>> GetAllByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves a Transaction Concept mapping
        /// by its identifier, validating Company ownership.
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient?> GetByIdAsync(
            int companyId,
             int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a Transaction Concept
        /// is already assigned to a CompanyClient.
        /// </summary>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionConceptId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new Transaction Concept mapping.
        ///
        /// IMPORTANT:
        /// - Entity MUST already contain CompanyId.
        /// - Repository MUST NOT infer or override identifiers.
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing Transaction Concept mapping.
        /// </summary>
        Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct);
    }
}

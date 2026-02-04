namespace DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Transaction Type Client repository interface.
    ///
    /// This repository manages the relationship between
    /// Transaction Types (master) and Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - All operations MUST be scoped by CompanyId.
    /// - This repository represents the last line of defense
    ///   for multi-tenant data isolation.
    /// </summary>
    public interface ITransactionTypeClientWMSAPIRepository
    {
        /// <summary>
        /// Retrieves all Transaction Type mappings
        /// for a specific CompanyClient.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="companyClientId">Company client identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of TransactionTypeClient mappings</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.TransactionTypeClient>> GetAllByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves all active Transaction Type mappings
        /// for a specific CompanyClient.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="companyClientId">Company client identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active TransactionTypeClient mappings</returns>
        Task<List<DUNES.API.ModelsWMS.Masters.TransactionTypeClient>> GetActiveByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves a Transaction Type mapping by its identifier,
        /// validating Company ownership.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="id">TransactionTypeClient identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>TransactionTypeClient entity or null if not found</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Retrieves a specific Transaction Type mapping
        /// by CompanyClient and TransactionType identifiers.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="companyClientId">Company client identifier</param>
        /// <param name="transactionTypeId">Transaction Type identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>TransactionTypeClient entity or null if not found</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetByClientAndTypeAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a Transaction Type is already
        /// mapped to a specific CompanyClient.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="companyClientId">Company client identifier</param>
        /// <param name="transactionTypeId">Transaction Type identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if mapping exists; otherwise false</returns>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new Transaction Type mapping
        /// for a CompanyClient.
        ///
        /// IMPORTANT:
        /// - Entity MUST already contain CompanyId and CompanyClientId.
        /// - Repository MUST NOT infer or override identifiers.
        /// </summary>
        /// <param name="entity">TransactionTypeClient entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing Transaction Type mapping.
        ///
        /// IMPORTANT:
        /// - Ownership identifiers MUST NOT be modified here.
        /// </summary>
        /// <param name="entity">TransactionTypeClient entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated entity</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a Transaction Type
        /// for a specific CompanyClient.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="companyClientId">Company client identifier</param>
        /// <param name="transactionTypeId">Transaction Type identifier</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if updated; false if mapping not found</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            bool isActive,
            CancellationToken ct);
    }
}

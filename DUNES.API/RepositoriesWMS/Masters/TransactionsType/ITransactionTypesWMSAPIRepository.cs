using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionsType
{
    /// <summary>
    /// Transaction Types Repository Interface
    /// 
    /// Scoped by:
    /// Company (STANDARD COMPANYID)
    /// 
    /// IMPORTANT:
    /// This repository is the last line of defense for multi-tenant security.
    /// ALL operations MUST be filtered by CompanyId.
    /// </summary>
    public interface ITransactionTypesWMSAPIRepository
    {
        /// <summary>
        /// Get all transaction types for a company.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of transaction types</returns>
        Task<List<Transactiontypes>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active transaction types for a company.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>List of active transaction types</returns>
        Task<List<Transactiontypes>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get a transaction type by id validating company ownership.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="id">Transaction type identifier</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Transaction type or null if not found</returns>
        Task<Transactiontypes?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Check if a transaction type with the same name already exists for a company.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="name">Transaction type name</param>
        /// <param name="excludeId">
        /// Optional transaction type id to exclude (used during update).
        /// </param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Create a new transaction type.
        /// 
        /// IMPORTANT:
        /// - Entity MUST already contain CompanyId.
        /// - Repository MUST NOT infer or override CompanyId.
        /// </summary>
        /// <param name="entity">Transaction type entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Created entity</returns>
        Task<Transactiontypes> CreateAsync(
            Transactiontypes entity,
            CancellationToken ct);

        /// <summary>
        /// Update an existing transaction type.
        /// 
        /// IMPORTANT:
        /// - Ownership (CompanyId) MUST NOT be modified here.
        /// </summary>
        /// <param name="entity">Transaction type entity</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>Updated entity</returns>
        Task<Transactiontypes> UpdateAsync(
            Transactiontypes entity,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a transaction type.
        /// </summary>
        /// <param name="companyId">Company identifier (tenant)</param>
        /// <param name="id">Transaction type identifier</param>
        /// <param name="isActive">Activation flag</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>True if updated, false if not found</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }

}

using DUNES.API.ModelsWMS.Masters;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionConcepts
{
    /// <summary>
    /// Transaction Concepts repository.
    /// 
    /// Scoped by:
    /// Company (tenant).
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - This repository is the last line of defense for multi-tenant security.
    /// - All queries MUST be explicitly filtered by CompanyId.
    /// - The repository must NEVER infer, override, or modify CompanyId.
    /// </summary>
    public interface ITransactionConceptsWMSAPIRepository
    {
        /// <summary>
        /// Retrieves all transaction concepts for the specified company.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope the query.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>A list of transaction concepts belonging to the specified company.</returns>
        Task<List<Transactionconcepts>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves all active transaction concepts for the specified company.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope the query.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>A list of active transaction concepts belonging to the specified company.</returns>
        Task<List<Transactionconcepts>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves a transaction concept by its identifier, validating ownership.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Internal identifier of the transaction concept.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// The transaction concept if found and owned by the company; otherwise, null.
        /// </returns>
        Task<Transactionconcepts?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Determines whether a transaction concept with the specified name
        /// already exists for the given company.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope the validation.</param>
        /// <param name="name">Transaction concept name to validate.</param>
        /// <param name="excludeId">
        /// Optional transaction concept identifier to exclude from the check
        /// (used in update scenarios).
        /// </param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// True if a transaction concept with the same name already exists
        /// for the company; otherwise, false.
        /// </returns>
        Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new transaction concept record.
        /// 
        /// IMPORTANT:
        /// - The entity must already contain a valid CompanyId.
        /// - The repository must NOT infer or override ownership.
        /// </summary>
        /// <param name="entity">Transaction concept entity to be created.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>The newly created transaction concept entity.</returns>
        Task<Transactionconcepts> CreateAsync(
            Transactionconcepts entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing transaction concept record.
        /// 
        /// IMPORTANT:
        /// - Ownership (CompanyId) must remain unchanged.
        /// - Validation of ownership and business rules must occur before calling this method.
        /// </summary>
        /// <param name="entity">Transaction concept entity with updated values.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>The updated transaction concept entity.</returns>
        Task<Transactionconcepts> UpdateAsync(
            Transactionconcepts entity,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a transaction concept (soft state change).
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Internal identifier of the transaction concept.</param>
        /// <param name="isActive">Target active state.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// True if the record was found and updated; otherwise, false.
        /// </returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the transaction concept has related records
        /// that prevent physical deletion.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope validation.</param>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// True if dependencies exist; otherwise, false.
        /// </returns>
        Task<bool> HasDependenciesAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Deletes a transaction concept record permanently.
        /// 
        /// IMPORTANT:
        /// - Must be called only after validating there are no dependencies.
        /// - Ownership must be validated by CompanyId.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token to cancel the asynchronous operation.</param>
        /// <returns>
        /// True if the record was found and deleted; otherwise, false.
        /// </returns>
        Task<bool> DeleteAsync(
            int companyId,
            int id,
            CancellationToken ct);
    }
}

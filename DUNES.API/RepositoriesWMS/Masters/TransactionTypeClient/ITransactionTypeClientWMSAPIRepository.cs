using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Repository for TransactionTypeClient mappings.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - All queries MUST be scoped by CompanyId.
    /// - CompanyClientId must also be validated in every mapping operation.
    /// - Repository never infers tenant scope.
    /// </summary>
    public interface ITransactionTypeClientWMSAPIRepository
    {
        /// <summary>
        /// Gets all mappings for a specific company client.
        /// Includes master transaction type info for display.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of mappings for the client.</returns>
        Task<List<WMSTransactionTypeClientReadDTO>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets one mapping entity by Id, scoped by Company and CompanyClient.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The mapping entity if found; otherwise null.</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetEntityByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a mapping already exists for the combination
        /// (CompanyId, CompanyClientId, TransactionTypeId).
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="transactionTypeId">Master transaction type identifier.</param>
        /// <param name="excludeId">
        /// Optional mapping Id to exclude (useful in update scenarios).
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if a duplicate mapping exists; otherwise false.</returns>
        Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Validates that the master Transaction Type exists
        /// and belongs to the same Company.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="transactionTypeId">Master transaction type identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the master exists for the company; otherwise false.</returns>
        Task<bool> MasterExistsAsync(
            int companyId,
            int transactionTypeId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new mapping.
        /// </summary>
        /// <param name="entity">Mapping entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created mapping entity.</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct);

        /// <summary>
        /// Updates the Active flag only (patch style).
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="isActive">New active state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if updated; otherwise false.</returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Deletes a mapping physically.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if deleted; otherwise false.</returns>
        Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);
    }
}

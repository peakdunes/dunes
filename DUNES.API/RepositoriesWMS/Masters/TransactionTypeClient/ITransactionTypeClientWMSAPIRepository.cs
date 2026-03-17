using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Repository contract for managing TransactionTypeClient mappings.
    /// Handles persistence and read operations for the mapping between a client
    /// and the master TransactionType catalog, scoped by CompanyId and CompanyClientId.
    /// </summary>
    public interface ITransactionTypeClientWMSAPIRepository
    {
        /// <summary>
        /// Gets all mapping records for the specified tenant scope (company + client),
        /// including both active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A list of mapped transaction types (active and inactive) with master display name.
        /// </returns>
        Task<List<WMSTransactionTypeClientReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a single mapping record by mapping Id within the specified tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// The mapped transaction type DTO if found; otherwise <c>null</c>.
        /// </returns>
        Task<WMSTransactionTypeClientReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a mapping already exists for the same tenant scope and TransactionTypeId.
        /// Useful to prevent duplicates on create/update.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="transactionTypeId">Master TransactionType identifier.</param>
        /// <param name="excludeId">
        /// Optional mapping Id to exclude from the duplicate check (used in updates).
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if a duplicate mapping exists; otherwise <c>false</c>.</returns>
        Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the referenced TransactionType exists in the master catalog.
        /// </summary>
        /// <param name="transactionTypeId">Master TransactionType identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if the master record exists; otherwise <c>false</c>.</returns>
        Task<bool> MasterExistsAsync(
            int transactionTypeId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the referenced TransactionType exists and is active in the master catalog.
        /// This is used when enabling a mapping (<c>Active = true</c>).
        /// </summary>
        /// <param name="transactionTypeId">Master TransactionType identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if the master record exists and is active; otherwise <c>false</c>.
        /// </returns>
        Task<bool> MasterIsActiveAsync(
            int transactionTypeId,
            CancellationToken ct);

        /// <summary>
        /// Gets the mapping entity by Id within the specified tenant scope.
        /// This method returns the entity (not DTO) for update/delete operations.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// The entity if found within the tenant scope; otherwise <c>null</c>.
        /// </returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new TransactionTypeClient mapping.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created entity including generated Id.</returns>
        Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing TransactionTypeClient mapping.
        /// </summary>
        /// <param name="entity">Entity with modified values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct);

        /// <summary>
        /// Deletes an existing TransactionTypeClient mapping.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct);

        /// <summary>
        /// Sets the active status for an existing TransactionTypeClient mapping
        /// within the tenant scope.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="isActive">New mapping active status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Returns the enabled transaction types for the current client.
        /// Only returns rows where:
        /// - Mapping Active = true AND
        /// - Master TransactionType Active = true
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        Task<List<WMSTransactionTypeClientReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes the transaction type relation by mapping Id
        /// (does not delete the master transaction type).
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);
    }
}
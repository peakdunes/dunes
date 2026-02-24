using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Repository contract for managing CompanyClientInventoryType mappings.
    /// Handles persistence and read operations for the mapping between a client
    /// and the master InventoryTypes catalog, scoped by CompanyId and CompanyClientId.
    /// </summary>
    public interface ICompanyClientInventoryTypeWMSAPIRepository
    {
        /// <summary>
        /// Gets all mapping records for the specified tenant scope (company + client),
        /// including both active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A list of mapped inventory types (active and inactive) with master type display name.
        /// </returns>
        Task<List<WMSCompanyClientInventoryTypeReadDTO>> GetAllAsync(
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
        /// The mapped inventory type DTO if found; otherwise <c>null</c>.
        /// </returns>
        Task<WMSCompanyClientInventoryTypeReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a mapping already exists for the same tenant scope and InventoryTypeId.
        /// Useful to prevent duplicates on create/update.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="inventoryTypeId">Master InventoryType identifier.</param>
        /// <param name="excludeId">
        /// Optional mapping Id to exclude from the duplicate check (used in updates).
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if a duplicate mapping exists; otherwise <c>false</c>.</returns>
        Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int inventoryTypeId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the referenced InventoryType exists in the master catalog.
        /// </summary>
        /// <param name="inventoryTypeId">Master InventoryType identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if the master record exists; otherwise <c>false</c>.</returns>
        Task<bool> MasterExistsAsync(
            int inventoryTypeId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the referenced InventoryType exists and is active in the master catalog.
        /// This is used when enabling a mapping (<c>IsActive = true</c>).
        /// </summary>
        /// <param name="inventoryTypeId">Master InventoryType identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if the master record exists and is active; otherwise <c>false</c>.
        /// </returns>
        Task<bool> MasterIsActiveAsync(
            int inventoryTypeId,
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
        Task<DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new CompanyClientInventoryType mapping.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created entity including generated Id.</returns>
        Task<DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType> CreateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing CompanyClientInventoryType mapping.
        /// </summary>
        /// <param name="entity">Entity with modified values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType entity,
            CancellationToken ct);

        /// <summary>
        /// Deletes an existing CompanyClientInventoryType mapping.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType entity,
            CancellationToken ct);

        /// <summary>
        /// Sets the active status for an existing CompanyClientInventoryType mapping
        /// within the tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="isActive">New mapping active status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        Task<bool> SetActiveAsync(
            int id,
            int companyId,
            int companyClientId,
            bool isActive,
            CancellationToken ct);
    }
}
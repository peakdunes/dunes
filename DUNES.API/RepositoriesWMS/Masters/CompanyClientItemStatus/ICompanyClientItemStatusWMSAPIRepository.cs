using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository contract for managing CompanyClientItemStatus mappings.
    /// Handles persistence and tenant-scoped read operations for the relation
    /// between a client and the master Itemstatus catalog.
    /// </summary>
    public interface ICompanyClientItemStatusWMSAPIRepository
    {
        /// <summary>
        /// Gets all ItemStatus mappings for the specified tenant scope (company + client),
        /// including both active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of ItemStatus mappings with master display name.</returns>
        Task<List<WMSCompanyClientItemStatusReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a single ItemStatus mapping by mapping Id within the tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The mapping DTO if found; otherwise <c>null</c>.</returns>
        Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a mapping already exists for the same tenant scope and ItemStatusId.
        /// Used to prevent duplicates in create/update operations.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="itemStatusId">Master Itemstatus identifier.</param>
        /// <param name="excludeId">
        /// Optional mapping Id to exclude from the duplicate check (used on update).
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if a duplicate mapping exists; otherwise <c>false</c>.</returns>
        Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the referenced master Itemstatus exists.
        /// </summary>
        /// <param name="itemStatusId">Master Itemstatus identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if exists; otherwise <c>false</c>.</returns>
        Task<bool> MasterExistsAsync(
            int itemStatusId,
            CancellationToken ct);

        /// <summary>
        /// Checks whether the referenced master Itemstatus exists and is active.
        /// Used when enabling a mapping.
        /// </summary>
        /// <param name="itemStatusId">Master Itemstatus identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if master exists and is active; otherwise <c>false</c>.</returns>
        Task<bool> MasterIsActiveAsync(
            int itemStatusId,
            CancellationToken ct);

        /// <summary>
        /// Gets the mapping entity by Id within the specified tenant scope.
        /// Returns entity (not DTO) for update/delete operations.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The entity if found; otherwise <c>null</c>.</returns>
        Task<DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new ItemStatus mapping.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created entity including generated Id.</returns>
        Task<DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus> CreateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing ItemStatus mapping.
        /// </summary>
        /// <param name="entity">Entity with modified values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if at least one row was affected; otherwise <c>false</c>.</returns>
        Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus entity,
            CancellationToken ct);

        /// <summary>
        /// Deletes an existing ItemStatus mapping (physical delete).
        /// Intended for wrong assignments.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if at least one row was affected; otherwise <c>false</c>.</returns>
        Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus entity,
            CancellationToken ct);

        /// <summary>
        /// Sets the active status for an existing ItemStatus mapping within tenant scope.
        /// Intended for temporary enable/disable without deleting the mapping.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="isActive">New mapping active status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if at least one row was affected; otherwise <c>false</c>.</returns>
        Task<bool> SetActiveAsync(
            int id,
            int companyId,
            int companyClientId,
            bool isActive,
            CancellationToken ct);
    }
}

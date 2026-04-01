using DUNES.API.ModelsWMS.Masters;
using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;

namespace DUNES.API.RepositoriesWMS.Masters.Items
{
    /// <summary>
    /// Repository contract for managing Items within WMS.
    /// Supports retrieval of master/company items, client-owned items, or both,
    /// depending on the ownership mode resolved by the service layer.
    /// </summary>
    public interface IItemsWMSAPIRepository
    {
        /// <summary>
        /// Returns all items available for the given company and client context,
        /// according to the ownership filters provided.
        /// </summary>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of items available under the resolved ownership scope.</returns>
        Task<List<WMSItemsReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            CancellationToken ct);

        /// <summary>
        /// Returns a single item by Id within the given company and client context,
        /// according to the ownership filters provided.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The requested item if found within the allowed scope; otherwise null.</returns>
        Task<WMSItemsReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            CancellationToken ct);

        /// <summary>
        /// Returns the entity instance for a specific item by Id within the given company and client context,
        /// according to the ownership filters provided.
        /// This method is intended for update/delete operations.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The entity if found within the allowed scope; otherwise null.</returns>
        Task<DUNES.API.ModelsWMS.Masters.Items?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            CancellationToken ct);

        /// <summary>
        /// Checks whether a Part Number already exists in the Items catalog.
        /// Business rule: Part Number must be unique globally.
        /// </summary>
        /// <param name="partNumber">Part Number to validate.</param>
        /// <param name="excludeId">
        /// Optional item Id to exclude from the validation.
        /// Used during update scenarios.
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the Part Number already exists; otherwise false.</returns>
        Task<bool> ExistsPartNumberAsync(
            string partNumber,
            int? excludeId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new item record.
        /// </summary>
        /// <param name="entity">Entity to persist.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created entity including generated Id.</returns>
        Task<DUNES.API.ModelsWMS.Masters.Items> CreateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing item record.
        /// </summary>
        /// <param name="entity">Entity with modified values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one database row was affected; otherwise false.</returns>
        Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct);

        /// <summary>
        /// Deletes an existing item record.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one database row was affected; otherwise false.</returns>
        Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.Items entity,
            CancellationToken ct);

        /// <summary>
        /// Updates the active status of an item within the allowed ownership scope.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="companyId">Company identifier from token scope.</param>
        /// <param name="companyClientId">Company client identifier from token scope.</param>
        /// <param name="includeMasterItems">
        /// Indicates whether company/master items (CompanyClientId = null) should be included.
        /// </param>
        /// <param name="includeClientItems">
        /// Indicates whether client-owned items (CompanyClientId = current client) should be included.
        /// </param>
        /// <param name="isActive">New active status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if at least one database row was affected; otherwise false.</returns>
        Task<bool> SetActiveAsync(
            int id,
            int companyId,
            int companyClientId,
            bool includeMasterItems,
            bool includeClientItems,
            bool isActive,
            CancellationToken ct);
    }
}
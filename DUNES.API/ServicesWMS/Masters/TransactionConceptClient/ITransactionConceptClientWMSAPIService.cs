using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Service contract for managing TransactionConceptClient mappings.
    /// Applies business rules on top of the repository layer for the mapping
    /// between a client and the master TransactionConcept catalog.
    /// </summary>
    public interface ITransactionConceptClientWMSAPIService
    {
        /// <summary>
        /// Gets all transaction concept mappings for the current tenant scope.
        /// Includes both active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the list of mapped transaction concepts.
        /// </returns>
        Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a transaction concept mapping by Id for the current tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the requested mapping if found.
        /// </returns>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new TransactionConceptClient mapping.
        /// Business rules:
        /// - The master TransactionConcept must exist.
        /// - The mapping must not already exist for the same CompanyId + CompanyClientId + TransactionConceptId.
        /// - If the mapping is created as active, the master TransactionConcept must also be active.
        /// </summary>
        /// <param name="dto">Create DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the created mapping.
        /// </returns>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            WMSTransactionConceptClientCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing TransactionConceptClient mapping.
        /// Business rules:
        /// - The mapping must belong to the current tenant scope.
        /// - The referenced master TransactionConcept must exist.
        /// - The mapping must remain unique by CompanyId + CompanyClientId + TransactionConceptId.
        /// - If the mapping is updated as active, the master TransactionConcept must also be active.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="dto">Update DTO.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the updated mapping.
        /// </returns>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionConceptClientUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes a TransactionConceptClient mapping by Id.
        /// Important:
        /// - This deletes only the client mapping.
        /// - It does not delete the master TransactionConcept.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response indicating whether the delete operation succeeded.
        /// </returns>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates the active status of a TransactionConceptClient mapping.
        /// Business rules:
        /// - The mapping must belong to the current tenant scope.
        /// - If the new state is active, the master TransactionConcept must also be active.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="isActive">New mapping active status.</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response indicating whether the active state was updated successfully.
        /// </returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets the enabled transaction concepts for the current tenant scope.
        /// Only returns mappings where:
        /// - Mapping Active = true
        /// - Master TransactionConcept IsActive = true
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// Api response containing the list of enabled transaction concepts.
        /// </returns>
        Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);
    }
}
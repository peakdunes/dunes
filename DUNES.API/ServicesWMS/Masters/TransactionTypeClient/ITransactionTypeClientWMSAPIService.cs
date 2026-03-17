using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Service contract for managing TransactionTypeClient mappings.
    /// Applies business rules on top of the repository layer for the mapping
    /// between a client and the master TransactionType catalog.
    /// </summary>
    public interface ITransactionTypeClientWMSAPIService
    {
        /// <summary>
        /// Gets all transaction type mappings for the current tenant scope.
        /// Includes both active and inactive mappings.
        /// </summary>
        Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets a transaction type mapping by Id for the current tenant scope.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new TransactionTypeClient mapping.
        /// Business rules:
        /// - The master TransactionType must exist.
        /// - The mapping must not already exist for the same CompanyId + CompanyClientId + TransactionTypeId.
        /// - If the mapping is created as active, the master TransactionType must also be active.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            WMSTransactionTypeClientCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing TransactionTypeClient mapping.
        /// Business rules:
        /// - The mapping must belong to the current tenant scope.
        /// - The referenced master TransactionType must exist.
        /// - The mapping must remain unique by CompanyId + CompanyClientId + TransactionTypeId.
        /// - If the mapping is updated as active, the master TransactionType must also be active.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionTypeClientUpdateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Deletes a TransactionTypeClient mapping by Id.
        /// Important:
        /// - This deletes only the client mapping.
        /// - It does not delete the master TransactionType.
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Updates the active status of a TransactionTypeClient mapping.
        /// Business rules:
        /// - The mapping must belong to the current tenant scope.
        /// - If the new state is active, the master TransactionType must also be active.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Gets the enabled transaction types for the current tenant scope.
        /// Only returns mappings where:
        /// - Mapping Active = true
        /// - Master TransactionType Active = true
        /// </summary>
        Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);
    }
}
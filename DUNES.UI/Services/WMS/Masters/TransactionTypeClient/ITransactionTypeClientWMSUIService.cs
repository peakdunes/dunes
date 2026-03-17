using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// UI service interface for client-specific Transaction Types configuration.
    /// </summary>
    public interface ITransactionTypeClientWMSUIService
    {
        /// <summary>
        /// Gets all transaction types mapped for a client.
        /// Includes both active and inactive mappings.
        /// </summary>
        Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets a specific client transaction type mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Creates a new transaction type mapping for the client.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            WMSTransactionTypeClientCreateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing transaction type mapping.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionTypeClientUpdateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets enabled transaction types for the current client.
        /// Returns only:
        /// - mapping Active=true AND
        /// - master catalog Active=true
        /// </summary>
        Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a client transaction type mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Deletes transaction type relation.
        /// Does not delete the master transaction type.
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            string token,
            CancellationToken ct);
    }
}
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.UI.Services.WMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// UI service interface for client-specific Transaction Concepts configuration.
    /// </summary>
    public interface ITransactionConceptClientWMSUIService
    {
        /// <summary>
        /// Gets all transaction concepts mapped for a client.
        /// Includes both active and inactive mappings.
        /// </summary>
        Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets a specific client transaction concept mapping by ID.
        /// </summary>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Creates a new transaction concept mapping for the client.
        /// </summary>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            WMSTransactionConceptClientCreateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing transaction concept mapping.
        /// </summary>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionConceptClientUpdateDTO dto,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Gets enabled transaction concepts for the current client.
        /// Returns only:
        /// - mapping Active=true AND
        /// - master catalog IsActive=true
        /// </summary>
        Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a client transaction concept mapping.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct);

        /// <summary>
        /// Deletes transaction concept relation.
        /// Does not delete the master transaction concept.
        /// </summary>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            string token,
            CancellationToken ct);
    }
}

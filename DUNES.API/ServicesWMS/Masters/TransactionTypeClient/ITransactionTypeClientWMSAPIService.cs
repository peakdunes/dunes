using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Transaction Type Client service interface.
    ///
    /// Manages the association between Transaction Types
    /// and Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller.
    /// - CompanyId comes from the authenticated token.
    /// - The service enforces business rules before repository calls.
    /// </summary>
    public interface ITransactionTypeClientWMSAPIService
    {
        /// <summary>
        /// Retrieves all Transaction Type mappings for a specific CompanyClient.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the list of mappings.</returns>
        Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new Transaction Type mapping for a CompanyClient.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="dto">Create DTO for the mapping.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the created mapping.</returns>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates an existing mapping.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="isActive">New active state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse with the updated mapping.</returns>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct);

        /// <summary>
        /// Deletes a mapping physically.
        /// </summary>
        /// <param name="companyId">Tenant company identifier (from token).</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>ApiResponse indicating whether the mapping was deleted.</returns>
        Task<ApiResponse<bool>> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct);
    }
}

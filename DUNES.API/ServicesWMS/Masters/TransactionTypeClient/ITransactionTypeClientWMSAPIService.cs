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
    /// - The service enforces all business rules.
    /// </summary>
    public interface ITransactionTypeClientWMSAPIService
    {
        /// <summary>
        /// Retrieves all Transaction Type associations
        /// for a specific CompanyClient.
        /// </summary>
        Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new Transaction Type association
        /// for a CompanyClient.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionTypeClientCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing Transaction Type association.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypeClientReadDTO>> UpdateAsync(
            int companyId,
            WMSTransactionTypeClientUpdateDTO dto,
            CancellationToken ct);
    }
}

using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Transaction Concept Client service interface.
    ///
    /// Manages the association between Transaction Concepts
    /// and Company Clients.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is always provided by the Controller.
    /// - The service enforces all business rules.
    /// </summary>
    public interface ITransactionConceptClientWMSAPIService
    {
        /// <summary>
        /// Retrieves all Transaction Concept associations
        /// for a specific CompanyClient.
        /// </summary>
        Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct);

        /// <summary>
        /// Creates a new Transaction Concept association
        /// for a CompanyClient.
        /// </summary>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            int companyId,
            WMSTransactionConceptClientCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing Transaction Concept association.
        /// </summary>
        Task<ApiResponse<WMSTransactionConceptClientReadDTO>> UpdateAsync(
            int companyId,
            WMSTransactionConceptClientUpdateDTO dto,
            CancellationToken ct);
    }
}

using DUNES.Shared.DTOs;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// UI service implementation for client-specific Transaction Concepts configuration.
    /// </summary>
    public class TransactionConceptClientWMSUIService
        : UIApiServiceBase, ITransactionConceptClientWMSUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public TransactionConceptClientWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Gets all transaction concepts mapped for a client.
        /// Includes both active and inactive mappings.
        /// </summary>
        public Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionConceptClientReadDTO>>(
                "/api/wms/masters/company-client/transaction-concept/GetAll",
                token,
                ct);

        /// <summary>
        /// Gets a specific client transaction concept mapping by ID.
        /// </summary>
        public Task<ApiResponse<WMSTransactionConceptClientReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSTransactionConceptClientReadDTO>(
                $"/api/wms/masters/company-client/transaction-concept/GetById/{id}",
                token,
                ct);

        /// <summary>
        /// Creates a new transaction concept mapping for the client.
        /// </summary>
        public Task<ApiResponse<WMSTransactionConceptClientReadDTO>> CreateAsync(
            WMSTransactionConceptClientCreateDTO dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<WMSTransactionConceptClientReadDTO, WMSTransactionConceptClientCreateDTO>(
                "/api/wms/masters/company-client/transaction-concept/Create",
                dto,
                token,
                ct);

        /// <summary>
        /// Updates an existing transaction concept mapping.
        /// </summary>
        public Task<ApiResponse<WMSTransactionConceptClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionConceptClientUpdateDTO dto,
            string token,
            CancellationToken ct)
            => PutApiAsync<WMSTransactionConceptClientReadDTO, WMSTransactionConceptClientUpdateDTO>(
                $"/api/wms/masters/company-client/transaction-concept/Update/{id}",
                dto,
                token,
                ct);

        /// <summary>
        /// Gets enabled transaction concepts for the current client.
        /// Returns only:
        /// - mapping Active=true AND
        /// - master catalog IsActive=true
        /// </summary>
        public Task<ApiResponse<List<WMSTransactionConceptClientReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionConceptClientReadDTO>>(
                "/api/wms/masters/company-client/transaction-concept/GetEnabled",
                token,
                ct);

        /// <summary>
        /// Activates or deactivates a client transaction concept mapping.
        /// </summary>
        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, WMSTransactionConceptClientSetActiveDTO>(
                $"/api/wms/masters/company-client/transaction-concept/SetActive/{id}",
                new WMSTransactionConceptClientSetActiveDTO
                {
                    Active = isActive
                },
                token,
                ct);

        /// <summary>
        /// Deletes transaction concept relation.
        /// Does not delete the master transaction concept.
        /// </summary>
        public Task<ApiResponse<bool>> DeleteAsync(
            int id,
            string token,
            CancellationToken ct)
            => DeleteApiAsync<bool>(
                $"/api/wms/masters/company-client/transaction-concept/Delete/{id}",
                token,
                ct);
    }
}

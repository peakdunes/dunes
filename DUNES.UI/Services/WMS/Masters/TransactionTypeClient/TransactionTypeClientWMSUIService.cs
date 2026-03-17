using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;
using DUNES.UI.Services.Common;

namespace DUNES.UI.Services.WMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// UI service implementation for client-specific Transaction Types configuration.
    /// </summary>
    public class TransactionTypeClientWMSUIService
        : UIApiServiceBase, ITransactionTypeClientWMSUIService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientWMSUIService"/> class.
        /// </summary>
        /// <param name="factory">HTTP client factory.</param>
        public TransactionTypeClientWMSUIService(IHttpClientFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Gets all transaction types mapped for a client.
        /// Includes both active and inactive mappings.
        /// </summary>
        public Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetAllAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypeClientReadDTO>>(
                "/api/wms/masters/company-client/transaction-type/GetAll",
                token,
                ct);

        /// <summary>
        /// Gets a specific client transaction type mapping by ID.
        /// </summary>
        public Task<ApiResponse<WMSTransactionTypeClientReadDTO>> GetByIdAsync(
            int id,
            string token,
            CancellationToken ct)
            => GetApiAsync<WMSTransactionTypeClientReadDTO>(
                $"/api/wms/masters/company-client/transaction-type/GetById/{id}",
                token,
                ct);

        /// <summary>
        /// Creates a new transaction type mapping for the client.
        /// </summary>
        public Task<ApiResponse<WMSTransactionTypeClientReadDTO>> CreateAsync(
            WMSTransactionTypeClientCreateDTO dto,
            string token,
            CancellationToken ct)
            => PostApiAsync<WMSTransactionTypeClientReadDTO, WMSTransactionTypeClientCreateDTO>(
                "/api/wms/masters/company-client/transaction-type/Create",
                dto,
                token,
                ct);

        /// <summary>
        /// Updates an existing transaction type mapping.
        /// </summary>
        public Task<ApiResponse<WMSTransactionTypeClientReadDTO>> UpdateAsync(
            int id,
            WMSTransactionTypeClientUpdateDTO dto,
            string token,
            CancellationToken ct)
            => PutApiAsync<WMSTransactionTypeClientReadDTO, WMSTransactionTypeClientUpdateDTO>(
                $"/api/wms/masters/company-client/transaction-type/Update/{id}",
                dto,
                token,
                ct);

        /// <summary>
        /// Gets enabled transaction types for the current client.
        /// Returns only:
        /// - mapping Active=true AND
        /// - master catalog Active=true
        /// </summary>
        public Task<ApiResponse<List<WMSTransactionTypeClientReadDTO>>> GetEnabledAsync(
            string token,
            CancellationToken ct)
            => GetApiAsync<List<WMSTransactionTypeClientReadDTO>>(
                "/api/wms/masters/company-client/transaction-type/GetEnabled",
                token,
                ct);

        /// <summary>
        /// Activates or deactivates a client transaction type mapping.
        /// </summary>
        public Task<ApiResponse<bool>> SetActiveAsync(
            int id,
            bool isActive,
            string token,
            CancellationToken ct)
            => PutApiAsync<bool, WMSTransactionTypeClientSetActiveDTO>(
                $"/api/wms/masters/company-client/transaction-type/SetActive/{id}",
                new WMSTransactionTypeClientSetActiveDTO
                {
                    Active = isActive
                },
                token,
                ct);

        /// <summary>
        /// Deletes transaction type relation.
        /// Does not delete the master transaction type.
        /// </summary>
        public Task<ApiResponse<bool>> DeleteAsync(
            int id,
            string token,
            CancellationToken ct)
            => DeleteApiAsync<bool>(
                $"/api/wms/masters/company-client/transaction-type/Delete/{id}",
                token,
                ct);
    }
}
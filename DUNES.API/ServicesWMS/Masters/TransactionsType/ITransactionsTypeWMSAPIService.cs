using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.TransactionsType
{
    /// <summary>
    /// Transaction Types Service Interface
    /// 
    /// Responsibilities:
    /// - Business rules validation
    /// - CompanyId ownership enforcement
    /// - DTO ↔ Entity mapping
    /// - Unified ApiResponse handling
    /// </summary>
    public interface ITransactionsTypeWMSAPIService
    {
        /// <summary>
        /// Get all transaction types for a company.
        /// </summary>
        Task<ApiResponse<List<WMSTransactiontypesReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get all active transaction types for a company.
        /// </summary>
        Task<ApiResponse<List<WMSTransactiontypesReadDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Get a transaction type by id validating company ownership.
        /// </summary>
        Task<ApiResponse<WMSTransactiontypesReadDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Create a new transaction type.
        /// </summary>
        Task<ApiResponse<WMSTransactiontypesCreateDTO>> CreateAsync(
            int companyId,
            WMSTransactiontypesCreateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Update an existing transaction type.
        /// </summary>
        Task<ApiResponse<WMSTransactionTypesUpdateDTO>> UpdateAsync(
            int companyId,
            int id,
            WMSTransactionTypesUpdateDTO dto,
            CancellationToken ct);

        /// <summary>
        /// Activate or deactivate a transaction type.
        /// </summary>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

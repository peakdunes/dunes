using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Models;

namespace DUNES.API.ServicesWMS.Masters.TransactionConcepts
{
    /// <summary>
    /// Transaction Concepts service contract.
    /// 
    /// This service contains all business rules and validations
    /// related to Transaction Concepts within the WMS.
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - CompanyId is ALWAYS provided by the Controller
    ///   using the value extracted from the authentication token.
    /// - The service NEVER reads claims or request headers directly.
    /// - The service is responsible for enforcing business rules
    ///   before calling the repository layer.
    /// </summary>
    public interface ITransactionConceptsWMSAPIService
    {
        /// <summary>
        /// Retrieves all transaction concepts for the specified company.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing a list of transaction concepts (read DTO).
        /// </returns>
        Task<ApiResponse<List<WMSTransactionconceptsReadDTO>>> GetAllAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves all active transaction concepts for the specified company.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing a list of active transaction concepts (read DTO).
        /// </returns>
        Task<ApiResponse<List<WMSTransactionconceptsReadDTO>>> GetActiveAsync(
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Retrieves a transaction concept by its identifier,
        /// validating ownership and visibility.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="id">
        /// Internal identifier of the transaction concept.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing the transaction concept (read DTO) if found;
        /// otherwise, a NotFound response.
        /// </returns>
        Task<ApiResponse<WMSTransactionconceptsReadDTO>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new transaction concept.
        /// 
        /// BUSINESS RULES:
        /// - Name must be unique per CompanyId.
        /// - CompanyId must be assigned from the authenticated context.
        /// - The transaction concept can be created active/inactive
        ///   according to the request DTO.
        /// </summary>
        /// <param name="request">
        /// DTO containing the data required to create the transaction concept.
        /// </param>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing the newly created transaction concept (read DTO).
        /// </returns>
        Task<ApiResponse<WMSTransactionconceptsReadDTO>> CreateAsync(
            WMSTransactionconceptsCreateDTO request,
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing transaction concept.
        /// 
        /// BUSINESS RULES:
        /// - The record must exist and belong to the specified CompanyId.
        /// - CompanyId cannot be modified.
        /// - Name uniqueness must be preserved per CompanyId.
        /// </summary>
        /// <param name="id">
        /// Internal identifier of the transaction concept.
        /// </param>
        /// <param name="request">
        /// DTO containing updated values for the transaction concept.
        /// </param>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing the updated transaction concept (read DTO).
        /// </returns>
        Task<ApiResponse<WMSTransactionconceptsReadDTO>> UpdateAsync(
            int id,
            WMSTransactionconceptsUpdateDTO request,
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// 
        /// This operation performs a soft state change and does not
        /// remove the record from the database.
        /// </summary>
        /// <param name="request">
        /// DTO containing the transaction concept identifier and target active state.
        /// </param>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing the updated transaction concept (read DTO).
        /// </returns>
        Task<ApiResponse<WMSTransactionconceptsReadDTO>> SetActiveAsync(
            WMSTransactionconceptsSetActiveDTO request,
            int companyId,
            CancellationToken ct);

        /// <summary>
        /// Deletes a transaction concept from the master catalog.
        /// 
        /// BUSINESS RULES:
        /// - The record must exist and belong to the specified CompanyId.
        /// - Physical deletion is blocked if dependencies exist
        ///   (for example, mappings in client configuration tables).
        /// </summary>
        /// <param name="id">
        /// Internal identifier of the transaction concept.
        /// </param>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse indicating whether the deletion was completed successfully.
        /// </returns>
        Task<ApiResponse<bool>> DeleteAsync(
            int id,
            int companyId,
            CancellationToken ct);
    }
}
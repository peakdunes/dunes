using DUNES.API.ModelsWMS.Masters;
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
        /// ApiResponse containing a list of transaction concepts.
        /// </returns>
        Task<ApiResponse<List<Transactionconcepts>>> GetAllAsync(
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
        /// ApiResponse containing a list of active transaction concepts.
        /// </returns>
        Task<ApiResponse<List<Transactionconcepts>>> GetActiveAsync(
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
        /// ApiResponse containing the transaction concept if found;
        /// otherwise, a NotFound or Forbidden response.
        /// </returns>
        Task<ApiResponse<Transactionconcepts>> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct);

        /// <summary>
        /// Creates a new transaction concept.
        /// 
        /// BUSINESS RULES:
        /// - Name must be unique per CompanyId.
        /// - CompanyId must be assigned from the authenticated context.
        /// - The transaction concept is created as Active by default
        ///   unless specified otherwise.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="entity">
        /// Transaction concept entity to be created.
        /// The CompanyId property will be validated and enforced.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing the newly created transaction concept.
        /// </returns>
        Task<ApiResponse<Transactionconcepts>> CreateAsync(
            int companyId,
            Transactionconcepts entity,
            CancellationToken ct);

        /// <summary>
        /// Updates an existing transaction concept.
        /// 
        /// BUSINESS RULES:
        /// - The record must exist and belong to the specified CompanyId.
        /// - CompanyId cannot be modified.
        /// - Name uniqueness must be preserved per CompanyId.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="id">
        /// Internal identifier of the transaction concept.
        /// </param>
        /// <param name="entity">
        /// Transaction concept entity containing updated values.
        /// CompanyId will be validated and must not change.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse containing the updated transaction concept.
        /// </returns>
        Task<ApiResponse<Transactionconcepts>> UpdateAsync(
            int companyId,
            int id,
            Transactionconcepts entity,
            CancellationToken ct);

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// 
        /// This operation performs a soft state change and does not
        /// remove the record from the database.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier provided by the Controller.
        /// </param>
        /// <param name="id">
        /// Internal identifier of the transaction concept.
        /// </param>
        /// <param name="isActive">
        /// Indicates whether the transaction concept should be active.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// ApiResponse indicating success or failure of the operation.
        /// </returns>
        Task<ApiResponse<bool>> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct);
    }
}

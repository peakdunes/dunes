using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionConcepts
{
    /// <summary>
    /// Transaction Concepts repository implementation.
    /// 
    /// Scoped by:
    /// Company (tenant).
    /// 
    /// IMPORTANT (STANDARD COMPANYID):
    /// - This repository is the last line of defense for multi-tenant security.
    /// - All read and write operations MUST be explicitly filtered by CompanyId.
    /// - The repository must NEVER infer, override, or modify CompanyId.
    /// </summary>
    public class TransactionConceptsWMSAPIRepository
        : ITransactionConceptsWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptsWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="db">
        /// Application WMS database context injected via dependency injection.
        /// </param>
        public TransactionConceptsWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves all transaction concepts for the specified company.
        /// 
        /// IMPORTANT:
        /// - Results are strictly scoped by CompanyId.
        /// - This is a read-only operation (AsNoTracking).
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier used to scope the query.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A list of transaction concepts belonging to the specified company.
        /// </returns>
        public async Task<List<Transactionconcepts>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .Where(x => x.companyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves all active transaction concepts for the specified company.
        /// 
        /// IMPORTANT:
        /// - Only records marked as Active are returned.
        /// - Results are strictly scoped by CompanyId.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier used to scope the query.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A list of active transaction concepts belonging to the specified company.
        /// </returns>
        public async Task<List<Transactionconcepts>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .Where(x => x.companyId == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a transaction concept by its identifier, validating ownership.
        /// 
        /// IMPORTANT:
        /// - The record must belong to the specified CompanyId.
        /// - If the record does not exist or does not belong to the company,
        ///   the method returns null.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier used to validate ownership.
        /// </param>
        /// <param name="id">
        /// Internal identifier of the transaction concept.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// The transaction concept if found and owned by the company;
        /// otherwise, null.
        /// </returns>
        public async Task<Transactionconcepts?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id && x.companyId == companyId,
                    ct);
        }

        /// <summary>
        /// Determines whether a transaction concept with the specified name
        /// already exists for the given company.
        /// 
        /// This method is typically used to enforce name uniqueness
        /// during create and update operations.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier used to scope the validation.
        /// </param>
        /// <param name="name">
        /// Transaction concept name to validate.
        /// </param>
        /// <param name="excludeId">
        /// Optional transaction concept identifier to exclude from the check.
        /// Used primarily during update operations.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// True if a transaction concept with the same name already exists
        /// for the company; otherwise, false.
        /// </returns>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.Transactionconcepts
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.Name != null &&
                    x.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Creates a new transaction concept record.
        /// 
        /// IMPORTANT:
        /// - The entity must already contain a valid CompanyId.
        /// - Ownership must be assigned by the Service layer.
        /// - The repository must NOT infer or override CompanyId.
        /// </summary>
        /// <param name="entity">
        /// Transaction concept entity to be created.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// The newly created transaction concept entity.
        /// </returns>
        public async Task<Transactionconcepts> CreateAsync(
            Transactionconcepts entity,
            CancellationToken ct)
        {
            _db.Transactionconcepts.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing transaction concept record.
        /// 
        /// IMPORTANT:
        /// - Ownership (CompanyId) must remain unchanged.
        /// - Validation of ownership and business rules must occur
        ///   before calling this method.
        /// </summary>
        /// <param name="entity">
        /// Transaction concept entity with updated values.
        /// </param>
        /// <param name="ct">
        /// Cancellation token to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// The updated transaction concept entity.
        /// </returns>
        public async Task<Transactionconcepts> UpdateAsync(
            Transactionconcepts entity,
            CancellationToken ct)
        {
            _db.Transactionconcepts.Update(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Activates or deactivates a transaction concept.
        /// 
        /// This operation performs a soft state change without
        /// removing the record from the database.
        /// </summary>
        /// <param name="companyId">
        /// Company (tenant) identifier used to validate ownership.
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
        /// True if the record was found and updated;
        /// otherwise, false.
        /// </returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.Transactionconcepts
                .FirstOrDefaultAsync(
                    x => x.Id == id && x.companyId == companyId,
                    ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}

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
    public class TransactionConceptsWMSAPIRepository : ITransactionConceptsWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptsWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="db">Application WMS database context injected via dependency injection.</param>
        public TransactionConceptsWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Retrieves all transaction concepts for the specified company.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope the query.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A list of transaction concepts belonging to the specified company.</returns>
        public async Task<List<Transactionconcepts>> GetAllAsync(int companyId, CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .Where(x => x.companyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves all active transaction concepts for the specified company.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope the query.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A list of active transaction concepts belonging to the specified company.</returns>
        public async Task<List<Transactionconcepts>> GetActiveAsync(int companyId, CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .Where(x => x.companyId == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a transaction concept by its identifier, validating ownership.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Internal identifier of the transaction concept.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The transaction concept if found and owned by the company; otherwise, null.</returns>
        public async Task<Transactionconcepts?> GetByIdAsync(int companyId, int id, CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId, ct);
        }

        /// <summary>
        /// Determines whether a transaction concept with the specified name already exists for the given company.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to scope the validation.</param>
        /// <param name="name">Transaction concept name to validate.</param>
        /// <param name="excludeId">Optional identifier to exclude (update scenario).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if a duplicate exists for the same company; otherwise, false.</returns>
        public async Task<bool> ExistsByNameAsync(int companyId, string name, int? excludeId, CancellationToken ct)
        {
            var normalizedName = (name ?? string.Empty).Trim();

            var query = _db.Transactionconcepts
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.Name != null &&
                    x.Name.Trim() == normalizedName);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Creates a new transaction concept record.
        /// </summary>
        /// <param name="entity">Transaction concept entity to be created.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The newly created transaction concept entity.</returns>
        public async Task<Transactionconcepts> CreateAsync(Transactionconcepts entity, CancellationToken ct)
        {
            _db.Transactionconcepts.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing transaction concept record.
        /// </summary>
        /// <param name="entity">Transaction concept entity with updated values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The updated transaction concept entity.</returns>
        public async Task<Transactionconcepts> UpdateAsync(Transactionconcepts entity, CancellationToken ct)
        {
            _db.Transactionconcepts.Update(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Activates or deactivates a transaction concept (soft state change).
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="isActive">Target active state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the record was found and updated; otherwise, false.</returns>
        public async Task<bool> SetActiveAsync(int companyId, int id, bool isActive, CancellationToken ct)
        {
            var entity = await _db.Transactionconcepts
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId, ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Checks whether the transaction concept has related records that prevent physical deletion.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used for context/ownership validation.</param>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if dependencies exist; otherwise, false.</returns>
        public async Task<bool> HasDependenciesAsync(int companyId, int id, CancellationToken ct)
        {
            // IMPORTANT:
            // This validation is intentionally transversal.
            // If any CompanyClient mapping exists for this TransactionConceptId,
            // master deletion must be blocked.
            return await _db.TransactionConceptClients
                .AsNoTracking()
                .AnyAsync(x => x.TransactionConceptId == id, ct);
        }

        /// <summary>
        /// Deletes a transaction concept permanently.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the record was found and deleted; otherwise, false.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the record does not exist in the tenant scope.</exception>
        public async Task<bool> DeleteAsync(int companyId, int id, CancellationToken ct)
        {
            var entity = await _db.Transactionconcepts
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId, ct);

            if (entity is null)
                throw new KeyNotFoundException("Transaction concept not found.");

            _db.Transactionconcepts.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}

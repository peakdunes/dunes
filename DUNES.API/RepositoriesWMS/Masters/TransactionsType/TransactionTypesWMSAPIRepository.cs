using DUNES.API.Data;
using DUNES.API.ModelsWMS.Masters;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionsType
{
    /// <summary>
    /// Transaction Types Repository Implementation
    /// 
    /// Scoped by:
    /// Company (STANDARD COMPANYID)
    /// 
    /// IMPORTANT:
    /// This repository is the last line of defense for multi-tenant security.
    /// ALL operations are filtered by CompanyId.
    /// </summary>
    public class TransactionTypesWMSAPIRepository : ITransactionTypesWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Constructor (Dependency Injection)
        /// </summary>
        /// <param name="db">WMS database context</param>
        public TransactionTypesWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get all transaction types for a company.
        /// </summary>
        public async Task<List<Transactiontypes>> GetAllAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _db.Transactiontypes
                .AsNoTracking()
                .Where(x => x.companyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get all active transaction types for a company.
        /// </summary>
        public async Task<List<Transactiontypes>> GetActiveAsync(
            int companyId,
            CancellationToken ct)
        {
            return await _db.Transactiontypes
                .AsNoTracking()
                .Where(x => x.companyId == companyId && x.Active)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Get transaction type by id validating company ownership.
        /// </summary>
        public async Task<Transactiontypes?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            return await _db.Transactiontypes
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id && x.companyId == companyId,
                    ct);
        }

        /// <summary>
        /// Check if a transaction type name already exists for a company.
        /// </summary>
        public async Task<bool> ExistsByNameAsync(
            int companyId,
            string name,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.Transactiontypes
                .AsNoTracking()
                .Where(x =>
                    x.companyId == companyId &&
                    x.Name != null &&
                    x.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Create a new transaction type.
        /// 
        /// IMPORTANT:
        /// Entity MUST already contain CompanyId.
        /// </summary>
        public async Task<Transactiontypes> CreateAsync(
            Transactiontypes entity,
            CancellationToken ct)
        {
            _db.Transactiontypes.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Update an existing transaction type.
        /// 
        /// IMPORTANT:
        /// Ownership (CompanyId) MUST NOT be changed here.
        /// </summary>
        public async Task<Transactiontypes> UpdateAsync(
            Transactiontypes entity,
            CancellationToken ct)
        {
            _db.Transactiontypes.Update(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Activate or deactivate a transaction type.
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.Transactiontypes
                .FirstOrDefaultAsync(
                    x => x.Id == id && x.companyId == companyId,
                    ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Checks whether the transaction type has related records that prevent physical deletion.
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
            return await _db.TransactionTypeClients
                .AsNoTracking()
                .AnyAsync(x => x.TransactionTypeId == id, ct);
        }

        /// <summary>
        /// Deletes a transaction type permanently.
        /// </summary>
        /// <param name="companyId">Company (tenant) identifier used to validate ownership.</param>
        /// <param name="id">Transaction concept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the record was found and deleted; otherwise, false.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the record does not exist in the tenant scope.</exception>
        public async Task<bool> DeleteAsync(int companyId, int id, CancellationToken ct)
        {
            var entity = await _db.Transactiontypes
                .FirstOrDefaultAsync(x => x.Id == id && x.companyId == companyId, ct);

            if (entity is null)
                throw new KeyNotFoundException("Transaction concept not found.");

            _db.Transactiontypes.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}

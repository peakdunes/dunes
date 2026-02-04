using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Transaction Concept Client repository implementation.
    ///
    /// This repository manages persistence and retrieval of
    /// TransactionConceptClient mappings.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - ALL queries are scoped by CompanyId.
    /// - Full composite key validation is enforced
    ///   (CompanyId + CompanyClientId).
    /// - This is the last line of defense for tenant isolation.
    /// </summary>
    public class TransactionConceptClientWMSAPIRepository
        : ITransactionConceptClientWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TransactionConceptClientWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="context">WMS database context</param>
        public TransactionConceptClientWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all Transaction Concept mappings
        /// for a specific CompanyClient.
        /// </summary>
        public async Task<List<DUNES.API.ModelsWMS.Masters.TransactionConceptClient>> GetAllByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a Transaction Concept mapping
        /// by its identifier, validating full ownership.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);
        }

        /// <summary>
        /// Checks whether a Transaction Concept
        /// is already assigned to a CompanyClient.
        /// </summary>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionConceptId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .AsNoTracking()
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionConceptId == transactionConceptId,
                    ct);
        }

        /// <summary>
        /// Creates a new Transaction Concept mapping.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct)
        {
            _context.TransactionConceptClients.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing Transaction Concept mapping.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct)
        {
            _context.TransactionConceptClients.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }
    }
}

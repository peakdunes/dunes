using DUNES.API.Data;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Transaction Type Client repository implementation.
    ///
    /// This repository manages persistence and retrieval of
    /// TransactionTypeClient mappings.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - ALL queries are scoped by CompanyId.
    /// - This is the last line of defense for multi-tenant isolation.
    /// </summary>
    public class TransactionTypeClientWMSAPIRepository
        : ITransactionTypeClientWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="TransactionTypeClientWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="context">WMS database context</param>
        public TransactionTypeClientWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all Transaction Type mappings
        /// for a specific CompanyClient.
        /// </summary>
        public async Task<List<DUNES.API.ModelsWMS.Masters.TransactionTypeClient>> GetAllByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves all active Transaction Type mappings
        /// for a specific CompanyClient.
        /// </summary>
        public async Task<List<DUNES.API.ModelsWMS.Masters.TransactionTypeClient>> GetActiveByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.Active)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Retrieves a Transaction Type mapping by its identifier,
        /// validating Company ownership.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetByIdAsync(
            int companyId,
            int id,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CompanyId == companyId &&
                    x.Id == id,
                    ct);
        }

        /// <summary>
        /// Retrieves a Transaction Type mapping by
        /// CompanyClient and TransactionType identifiers.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetByClientAndTypeAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionTypeId == transactionTypeId,
                    ct);
        }

        /// <summary>
        /// Checks whether a Transaction Type is already
        /// mapped to a specific CompanyClient.
        /// </summary>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            CancellationToken ct)
        {
            return await _context.TransactionTypeClients
                .AsNoTracking()
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionTypeId == transactionTypeId,
                    ct);
        }

        /// <summary>
        /// Creates a new Transaction Type mapping.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct)
        {
            _context.TransactionTypeClients.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing Transaction Type mapping.
        /// </summary>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct)
        {
            _context.TransactionTypeClients.Update(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Activates or deactivates a Transaction Type
        /// for a specific CompanyClient.
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            bool isActive,
            CancellationToken ct)
        {
            var affected = await _context.TransactionTypeClients
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionTypeId == transactionTypeId)
                .ExecuteUpdateAsync(
                    setters => setters
                        .SetProperty(x => x.Active, isActive),
                    ct);

            return affected > 0;
        }
    }
}

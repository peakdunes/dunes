using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionTypeClient
{
    /// <summary>
    /// Repository implementation for TransactionTypeClient mappings.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - All queries MUST be scoped by CompanyId.
    /// - CompanyClientId is also enforced in mapping operations.
    /// - Repository never infers tenant scope.
    /// </summary>
    public class TransactionTypeClientWMSAPIRepository : ITransactionTypeClientWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionTypeClientWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="db">Application WMS database context.</param>
        public TransactionTypeClientWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all mappings for a specific company client.
        /// Includes master transaction type info for display.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>List of mappings for the client.</returns>
        public async Task<List<WMSTransactionTypeClientReadDTO>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.TransactionTypeClients
                .AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .Join(
                    _db.Transactiontypes.AsNoTracking(),
                    map => map.TransactionTypeId,
                    master => master.Id,
                    (map, master) => new WMSTransactionTypeClientReadDTO
                    {
                        Id = map.Id,
                        CompanyClientId = map.CompanyClientId,
                        TransactionTypeId = map.TransactionTypeId,
                        TransactionTypeName = master.Name,
                        Active = map.Active
                    })
                .OrderBy(x => x.TransactionTypeName)
                .ToListAsync(ct);
        }

        /// <summary>
        /// Gets one mapping entity by Id, scoped by Company and CompanyClient.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The mapping entity if found; otherwise null.</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient?> GetEntityByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _db.TransactionTypeClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);
        }

        /// <summary>
        /// Checks whether a mapping already exists for the combination
        /// (CompanyId, CompanyClientId, TransactionTypeId).
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="transactionTypeId">Master transaction type identifier.</param>
        /// <param name="excludeId">Optional mapping Id to exclude.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if a duplicate mapping exists; otherwise false.</returns>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionTypeId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.TransactionTypeClients
                .AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionTypeId == transactionTypeId);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Validates that the master Transaction Type exists
        /// and belongs to the same Company.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="transactionTypeId">Master transaction type identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if the master exists for the company; otherwise false.</returns>
        public async Task<bool> MasterExistsAsync(
            int companyId,
            int transactionTypeId,
            CancellationToken ct)
        {
            return await _db.Transactiontypes
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Id == transactionTypeId &&
                    x.companyId == companyId, ct);
        }

        /// <summary>
        /// Creates a new mapping.
        /// </summary>
        /// <param name="entity">Mapping entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created mapping entity.</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionTypeClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionTypeClient entity,
            CancellationToken ct)
        {
            _db.TransactionTypeClients.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates the Active flag only (patch style).
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="isActive">New active state.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if updated; otherwise false.</returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.TransactionTypeClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);

            if (entity is null)
                return false;

            entity.Active = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Deletes a mapping physically.
        /// </summary>
        /// <param name="companyId">Tenant company identifier.</param>
        /// <param name="companyClientId">Company client identifier.</param>
        /// <param name="id">Mapping identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>True if deleted; otherwise false.</returns>
        public async Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var entity = await _db.TransactionTypeClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);

            if (entity is null)
                return false;

            _db.TransactionTypeClients.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}

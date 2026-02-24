using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Repository for TransactionConceptClient mappings.
    ///
    /// IMPORTANT (STANDARD COMPANYID):
    /// - All queries MUST be scoped by CompanyId.
    /// - CompanyClientId must also be validated in every mapping operation.
    /// - Repository never infers tenant scope.
    /// </summary>
    public class TransactionConceptClientWMSAPIRepository : ITransactionConceptClientWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="db">WMS DbContext.</param>
        public TransactionConceptClientWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Gets all mappings for a specific company client.
        /// Includes master concept info for display.
        /// </summary>
        public async Task<List<WMSTransactionConceptClientReadDTO>> GetByClientAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.TransactionConceptClients
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId && m.CompanyClientId == companyClientId)
                .Join(
                    _db.Transactionconcepts.AsNoTracking(),
                    m => m.TransactionConceptId,
                    tc => tc.Id,
                    (m, tc) => new { m, tc })
                .Where(x => x.tc.companyId == companyId) // defensa extra multi-tenant
                .OrderBy(x => x.tc.Name)
                .Select(x => new WMSTransactionConceptClientReadDTO
                {
                    Id = x.m.Id,
                    CompanyId = x.m.CompanyId,
                    CompanyClientId = x.m.CompanyClientId,
                    TransactionConceptId = x.m.TransactionConceptId,
                    TransactionConceptName = x.tc.Name,
                    Active = x.m.Active
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Gets one mapping entity by Id, scoped by Company and CompanyClient.
        /// </summary>
        public async Task<ModelsWMS.Masters.TransactionConceptClient?> GetEntityByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            return await _db.TransactionConceptClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);
        }

        /// <summary>
        /// Validates that the CompanyClient exists and belongs to the same Company.
        /// </summary>
        public async Task<bool> CompanyClientExistsAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClient
                .AsNoTracking()
                .AnyAsync(x => x.Id == companyClientId && x.CompanyId == companyId.ToString(), ct);
            // OJO:
            // Si tu tabla companyClient maneja CompanyId como int (no string), cambia a:
            // AnyAsync(x => x.Id == companyClientId && x.CompanyId == companyId, ct);
        }

        /// <summary>
        /// Checks if the mapping already exists for the combination
        /// (CompanyId, CompanyClientId, TransactionConceptId).
        /// </summary>
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int transactionConceptId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.TransactionConceptClients
                .AsNoTracking()
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionConceptId == transactionConceptId);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <summary>
        /// Validates that the master Transaction Concept exists
        /// and belongs to the same Company.
        /// </summary>
        public async Task<bool> MasterExistsAsync(
            int companyId,
            int transactionConceptId,
            CancellationToken ct)
        {
            return await _db.Transactionconcepts
                .AsNoTracking()
                .AnyAsync(x => x.Id == transactionConceptId && x.companyId == companyId, ct);
        }

        /// <summary>
        /// Creates a new mapping.
        /// </summary>
        public async Task<ModelsWMS.Masters.TransactionConceptClient> CreateAsync(
            ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct)
        {
            _db.TransactionConceptClients.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates Active flag only (patch style).
        /// </summary>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.TransactionConceptClients
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
        /// Deletes a mapping physically (optional, if business allows).
        /// </summary>
        public async Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var entity = await _db.TransactionConceptClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);

            if (entity is null)
                return false;

            _db.TransactionConceptClients.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}

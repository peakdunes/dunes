using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.TransactionConceptClient
{
    /// <summary>
    /// Repository implementation for managing TransactionConceptClient mappings.
    /// Handles persistence and read operations for the mapping between a client
    /// and the master TransactionConcept catalog, scoped by CompanyId and CompanyClientId.
    /// </summary>
    public class TransactionConceptClientWMSAPIRepository : ITransactionConceptClientWMSAPIRepository
    {
        private readonly appWmsDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionConceptClientWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public TransactionConceptClientWMSAPIRepository(appWmsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all mapping records for the specified tenant scope (company + client),
        /// including both active and inactive mappings.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// A list of mapped transaction concepts (active and inactive) with master display name.
        /// </returns>
        public async Task<List<WMSTransactionConceptClientReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .Select(x => new WMSTransactionConceptClientReadDTO
                {
                    Id = x.Id,
                   
                    TransactionConceptId = x.TransactionConceptId,
                    TransactionConceptName = x.TransactionConcept.Name,
                    Active = x.Active
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Gets a single mapping record by mapping Id within the specified tenant scope.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// The mapped transaction concept DTO if found; otherwise <c>null</c>.
        /// </returns>
        public async Task<WMSTransactionConceptClientReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .Where(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId)
                .Select(x => new WMSTransactionConceptClientReadDTO
                {
                    Id = x.Id,
                   
                    TransactionConceptId = x.TransactionConceptId,
                    TransactionConceptName = x.TransactionConcept.Name,
                    Active = x.Active
                })
                .FirstOrDefaultAsync(ct);
        }

        /// <summary>
        /// Checks whether a mapping already exists for the same tenant scope and TransactionConceptId.
        /// Useful to prevent duplicates on create/update.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="transactionConceptId">Master TransactionConcept identifier.</param>
        /// <param name="excludeId">
        /// Optional mapping Id to exclude from the duplicate check (used in updates).
        /// </param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if a duplicate mapping exists; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int transactionConceptId,
            int? excludeId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .AnyAsync(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.TransactionConceptId == transactionConceptId &&
                    (!excludeId.HasValue || x.Id != excludeId.Value),
                    ct);
        }

        /// <summary>
        /// Checks whether the referenced TransactionConcept exists in the master catalog.
        /// </summary>
        /// <param name="transactionConceptId">Master TransactionConcept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns><c>true</c> if the master record exists; otherwise <c>false</c>.</returns>
        public async Task<bool> MasterExistsAsync(
            int transactionConceptId,
            CancellationToken ct)
        {
            return await _context.Transactionconcepts
                .AnyAsync(x => x.Id == transactionConceptId, ct);
        }

        /// <summary>
        /// Checks whether the referenced TransactionConcept exists and is active in the master catalog.
        /// This is used when enabling a mapping (<c>Active = true</c>).
        /// </summary>
        /// <param name="transactionConceptId">Master TransactionConcept identifier.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if the master record exists and is active; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> MasterIsActiveAsync(
            int transactionConceptId,
            CancellationToken ct)
        {
            return await _context.Transactionconcepts
                .AnyAsync(x =>
                    x.Id == transactionConceptId &&
                    x.Active,
                    ct);
        }

        /// <summary>
        /// Gets the mapping entity by Id within the specified tenant scope.
        /// This method returns the entity (not DTO) for update/delete operations.
        /// </summary>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// The entity if found within the tenant scope; otherwise <c>null</c>.
        /// </returns>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);
        }

        /// <summary>
        /// Creates a new TransactionConceptClient mapping.
        /// </summary>
        /// <param name="entity">Entity to create.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>The created entity including generated Id.</returns>
        public async Task<DUNES.API.ModelsWMS.Masters.TransactionConceptClient> CreateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct)
        {
            _context.TransactionConceptClients.Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity;
        }

        /// <summary>
        /// Updates an existing TransactionConceptClient mapping.
        /// </summary>
        /// <param name="entity">Entity with modified values.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct)
        {
            _context.TransactionConceptClients.Update(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Deletes an existing TransactionConceptClient mapping.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.TransactionConceptClient entity,
            CancellationToken ct)
        {
            _context.TransactionConceptClients.Remove(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Sets the active status for an existing TransactionConceptClient mapping
        /// within the tenant scope.
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="isActive">New mapping active status.</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _context.TransactionConceptClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity == null)
                return false;

            entity.Active = isActive;

            return await _context.SaveChangesAsync(ct) > 0;
        }

        /// <summary>
        /// Returns the enabled transaction concepts for the current client.
        /// Only returns rows where:
        /// - Mapping Active = true AND
        /// - Master TransactionConcept IsActive = true
        /// </summary>
        /// <param name="companyId">Company scope (from token).</param>
        /// <param name="companyClientId">Client scope (from token).</param>
        /// <param name="ct">Cancellation token.</param>
        public async Task<List<WMSTransactionConceptClientReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _context.TransactionConceptClients
                .Where(x =>
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId &&
                    x.Active &&
                    x.TransactionConcept.Active)
                .Select(x => new WMSTransactionConceptClientReadDTO
                {
                    Id = x.Id,
                   
                    TransactionConceptId = x.TransactionConceptId,
                    TransactionConceptName = x.TransactionConcept.Name,
                    Active = x.Active
                })
                .ToListAsync(ct);
        }

        /// <summary>
        /// Deletes the transaction concept relation by mapping Id
        /// (does not delete the master transaction concept).
        /// </summary>
        /// <param name="companyId">Tenant company identifier from token.</param>
        /// <param name="companyClientId">Tenant client identifier from token.</param>
        /// <param name="id">Mapping identifier (surrogate key).</param>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>
        /// <c>true</c> if at least one database row was affected; otherwise <c>false</c>.
        /// </returns>
        public async Task<bool> DeleteAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            var entity = await _context.TransactionConceptClients
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity == null)
                return false;

            _context.TransactionConceptClients.Remove(entity);
            return await _context.SaveChangesAsync(ct) > 0;
        }
    }
}
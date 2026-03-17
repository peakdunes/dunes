using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryType
{

    /// <summary>
    /// Repository implementation for CompanyClientInventoryType mappings.
    /// Provides tenant-scoped CRUD and validation helpers for the relation
    /// between a client and the master InventoryTypes catalog.
    /// </summary>
    public class CompanyClientInventoryTypeWMSAPIRepository : ICompanyClientInventoryTypeWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientInventoryTypeRepository"/> class.
        /// </summary>
        /// <param name="db">Application database context.</param>
        public CompanyClientInventoryTypeWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public async Task<List<WMSCompanyClientInventoryTypeReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // NOTE:
            // - Includes both active and inactive mappings from CompanyClientInventoryType.
            // - Pulls display name from master InventoryTypes catalog.
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId)
                .Join(
                    _db.InventoryTypes.AsNoTracking(), // Adjust DbSet name if your context uses a different name
                    m => m.InventoryTypeId,
                    t => t.Id,
                    (m, t) => new WMSCompanyClientInventoryTypeReadDTO
                    {
                        Id = m.Id,
                        InventoryTypeId = t.Id,
                        InventoryTypeName = t.Name, // Adjust property name if master uses Description/TypeName/etc.
                        IsActive = m.IsActive
                    })
                .OrderBy(x => x.InventoryTypeName)
                .ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSCompanyClientInventoryTypeReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(m => m.Id == id
                         && m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId)
                .Join(
                    _db.InventoryTypes.AsNoTracking(),
                    m => m.InventoryTypeId,
                    t => t.Id,
                    (m, t) => new WMSCompanyClientInventoryTypeReadDTO
                    {
                        Id = m.Id,
                        InventoryTypeId = t.Id,
                        InventoryTypeName = t.Name,
                        IsActive = m.IsActive
                    })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int inventoryTypeId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId
                         && x.CompanyClientId == companyClientId
                         && x.InventoryTypeId == inventoryTypeId);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc />
        public async Task<bool> MasterExistsAsync(
            int inventoryTypeId,
            CancellationToken ct)
        {
            return await _db.InventoryTypes
                .AsNoTracking()
                .AnyAsync(x => x.Id == inventoryTypeId, ct);
        }

        /// <inheritdoc />
        public async Task<bool> MasterIsActiveAsync(
            int inventoryTypeId,
            CancellationToken ct)
        {
            return await _db.InventoryTypes
                .AsNoTracking()
                .AnyAsync(x => x.Id == inventoryTypeId && x.Active, ct);
            // If your master entity uses IsActive instead of Active, replace x.Active with x.IsActive
        }




        /// <inheritdoc />
        public async Task<DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientInventoryTypes
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);
        }

        /// <inheritdoc />
        public async Task<DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType> CreateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType entity,
            CancellationToken ct)
        {
            _db.CompanyClientInventoryTypes.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType entity,
            CancellationToken ct)
        {
            _db.CompanyClientInventoryTypes.Update(entity);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientInventoryType entity,
            CancellationToken ct)
        {
            _db.CompanyClientInventoryTypes.Remove(entity);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryTypes
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);

            if (entity is null)
                return false;

            entity.IsActive = isActive;

            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <summary>
        /// Returns the enabled inventory types for the current client.
        /// Only returns rows where:
        /// - Mapping IsActive=true AND
        /// - Master InventoryCategory IsActive=true
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyClientId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<WMSCompanyClientInventoryTypeReadDTO>> GetEnabledAsync(
           int companyId,
           int companyClientId,
           CancellationToken ct)
        {
            // Enabled means: mapping IsActive=true AND master IsActive=true
            return await _db.CompanyClientInventoryTypes
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId
                         && m.IsActive)
                .Join(
                    _db.InventoryTypes.AsNoTracking()
                        .Where(c => c.Active),
                    m => m.InventoryTypeId,
                    c => c.Id,
                    (m, c) => new WMSCompanyClientInventoryTypeReadDTO
                    {
                        Id = m.Id,
                        InventoryTypeId = c.Id,
                        InventoryTypeName = c.Name,
                        IsActive = m.IsActive
                    })
                .OrderBy(x => x.InventoryTypeName)
                .ToListAsync(ct);
        }

        /// <summary>
        /// delete inventory category relation (don't delete category master)
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="companyClientId"></param>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(int companyId, int companyClientId, int id, CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryTypes
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId,
                    ct);

            if (entity is null)
                return false;

            _db.CompanyClientInventoryTypes.Remove(entity);
            await _db.SaveChangesAsync(ct);

            return true;
        }
    }
}

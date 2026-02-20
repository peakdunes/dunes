using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientInventoryCategory
{
    /// <summary>
    /// Repository implementation for client-level inventory category enablement.
    /// Strictly scoped by CompanyId + CompanyClientId (from token).
    /// </summary>
    public class CompanyClientInventoryCategoryWMSAPIRepository : ICompanyClientInventoryCategoryWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>Constructor (DI).</summary>
        public CompanyClientInventoryCategoryWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public async Task<List<WMSCompanyClientInventoryCategoryReadDTO>> GetEnabledAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Enabled means: mapping IsActive=true AND master IsActive=true
            return await _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId
                         && m.IsActive)
                .Join(
                    _db.Inventorycategories.AsNoTracking()
                        .Where(c => c.Active),
                    m => m.InventoryCategoryId,
                    c => c.Id,
                    (m, c) => new WMSCompanyClientInventoryCategoryReadDTO
                    {
                        Id = m.Id,
                        InventoryCategoryId = c.Id,
                        InventoryCategoryName = c.Name,
                        IsActive = m.IsActive
                    })
                .OrderBy(x => x.InventoryCategoryName)
                .ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSCompanyClientInventoryCategoryReadDTO?> GetByIdAsync(
            int companyId,
            int companyClientId,
            int id,
            CancellationToken ct)
        {
            // Return the mapping by Id (scoped). For consistency with "enabled" rules,
            // we also require master IsActive=true here.
            return await _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .Where(m => m.Id == id
                         && m.CompanyId == companyId
                         && m.CompanyClientId == companyClientId)
                .Join(
                    _db.Inventorycategories.AsNoTracking().Where(c => c.Active),
                    m => m.InventoryCategoryId,
                    c => c.Id,
                    (m, c) => new WMSCompanyClientInventoryCategoryReadDTO
                    {
                        Id = m.Id,
                        InventoryCategoryId = c.Id,
                        InventoryCategoryName = c.Name,
                        IsActive = m.IsActive
                    })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSCompanyClientInventoryCategoryReadDTO> CreateAsync(
            WMSCompanyClientInventoryCategoryCreateDTO dto,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            // Anti-error: never create/enable if master is inactive.
            // (Service should validate too, but we protect the DB here as well.)
            var master = await _db.Inventorycategories
                .AsNoTracking()
                .Where(c => c.Id == dto.InventoryCategoryId && c.Active)
                .Select(c => new { c.Id, c.Name })
                .FirstOrDefaultAsync(ct);

            if (master is null)
                throw new InvalidOperationException("Cannot create mapping: master category is invalid or inactive.");

            // Avoid duplicates (unique index should exist too).
            var exists = await _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .AnyAsync(m =>
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId &&
                    m.InventoryCategoryId == dto.InventoryCategoryId,
                    ct);

            if (exists)
                throw new InvalidOperationException("Mapping already exists for this client and category.");

            var entity = new ModelsWMS.Masters.CompanyClientInventoryCategory
            {
                CompanyId = companyId,
                CompanyClientId = companyClientId,
                InventoryCategoryId = dto.InventoryCategoryId,
                IsActive = dto.IsActive
            };

            _db.CompanyClientInventoryCategories.Add(entity);

           

            await _db.SaveChangesAsync(ct);

            return new WMSCompanyClientInventoryCategoryReadDTO
            {
                Id = entity.Id,
                InventoryCategoryId = master.Id,
                InventoryCategoryName = master.Name,
                IsActive = entity.IsActive
            };
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(
            int companyId,
            int companyClientId,
            int id,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientInventoryCategories
                .FirstOrDefaultAsync(m =>
                    m.Id == id &&
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId,
                    ct);

            if (entity is null)
                return false;

            // Anti-error: do not allow activation if master is inactive.
            if (isActive)
            {
                var masterActive = await IsMasterActiveAsync(companyId, entity.InventoryCategoryId, ct);
                if (!masterActive)
                    throw new InvalidOperationException("Cannot activate mapping: master category is inactive.");
            }

            entity.IsActive = isActive;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> SetEnabledSetAsync(
            int companyId,
            int companyClientId,
            List<int> inventoryCategoryIds,
            CancellationToken ct)
        {
            inventoryCategoryIds ??= new List<int>();
            inventoryCategoryIds = inventoryCategoryIds.Distinct().ToList();

            // Anti-error: validate all provided IDs are master-active
            // (If your master catalog is tenant-scoped, also filter by CompanyId here.)
            if (inventoryCategoryIds.Count > 0)
            {
                var activeIds = await _db.Inventorycategories
                    .AsNoTracking()
                    .Where(c => c.Active && inventoryCategoryIds.Contains(c.Id))
                    .Select(c => c.Id)
                    .ToListAsync(ct);

                var missingOrInactive = inventoryCategoryIds.Except(activeIds).ToList();
                if (missingOrInactive.Count > 0)
                    throw new InvalidOperationException($"Invalid/inactive master categories: {string.Join(",", missingOrInactive)}");
            }

            // Load current mappings for the client
            var current = await _db.CompanyClientInventoryCategories
                .Where(m => m.CompanyId == companyId && m.CompanyClientId == companyClientId)
                .ToListAsync(ct);

            var currentByCategoryId = current.ToDictionary(x => x.InventoryCategoryId, x => x);

            // 1) Ensure all provided IDs exist and are active in mapping
            foreach (var catId in inventoryCategoryIds)
            {
                if (currentByCategoryId.TryGetValue(catId, out var existing))
                {
                    if (!existing.IsActive)
                        existing.IsActive = true;
                }
                else
                {
                    _db.CompanyClientInventoryCategories.Add(new ModelsWMS.Masters.CompanyClientInventoryCategory
                    {
                        CompanyId = companyId,
                        CompanyClientId = companyClientId,
                        InventoryCategoryId = catId,
                        IsActive = true
                    });
                }
            }

            // 2) Deactivate anything not in the final set
            var finalSet = inventoryCategoryIds.ToHashSet();
            foreach (var m in current)
            {
                if (!finalSet.Contains(m.InventoryCategoryId) && m.IsActive)
                    m.IsActive = false;
            }

            await _db.SaveChangesAsync(ct);
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(
            int companyId,
            int companyClientId,
            int inventoryCategoryId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientInventoryCategories
                .AsNoTracking()
                .Where(m =>
                    m.CompanyId == companyId &&
                    m.CompanyClientId == companyClientId &&
                    m.InventoryCategoryId == inventoryCategoryId);

            if (excludeId.HasValue)
                query = query.Where(m => m.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc />
        public async Task<bool> IsMasterActiveAsync(
            int companyId,
            int inventoryCategoryId,
            CancellationToken ct)
        {
            // If your master catalog is tenant-scoped, add: && c.CompanyId == companyId
            return await _db.Inventorycategories
                .AsNoTracking()
                .AnyAsync(c => c.Id == inventoryCategoryId && c.Active, ct);
        }
    }
}

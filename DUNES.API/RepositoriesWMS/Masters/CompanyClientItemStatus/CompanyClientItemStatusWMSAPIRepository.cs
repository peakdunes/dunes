using DUNES.API.Data;
using DUNES.Shared.DTOs.WMS;
using Microsoft.EntityFrameworkCore;

namespace DUNES.API.RepositoriesWMS.Masters.CompanyClientItemStatus
{
    /// <summary>
    /// Repository implementation for CompanyClientItemStatus mappings.
    /// Provides tenant-scoped CRUD operations and validation helpers for
    /// the relation between a client and the master Itemstatus catalog.
    /// </summary>
    public class CompanyClientItemStatusWMSAPIRepository : ICompanyClientItemStatusWMSAPIRepository
    {
        private readonly appWmsDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyClientItemStatusWMSAPIRepository"/> class.
        /// </summary>
        /// <param name="db">Application database context.</param>
        public CompanyClientItemStatusWMSAPIRepository(appWmsDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc />
        public async Task<List<WMSCompanyClientItemStatusReadDTO>> GetAllAsync(
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId &&
                            m.CompanyClientId == companyClientId)
                .Join(
                    _db.Itemstatus.AsNoTracking(), // <-- Ajusta DbSet master si el nombre real difiere
                    m => m.ItemStatusId,
                    s => s.Id,
                    (m, s) => new WMSCompanyClientItemStatusReadDTO
                    {
                        Id = m.Id,
                        ItemStatusId = s.Id,
                        ItemStatusName = s.Name, // <-- Ajusta propiedad de nombre si no es Name
                        IsActive = m.IsActive
                    })
                .OrderBy(x => x.ItemStatusName)
                .ToListAsync(ct);
        }

        /// <inheritdoc />
        public async Task<WMSCompanyClientItemStatusReadDTO?> GetByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {
            return await _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(m => m.Id == id &&
                            m.CompanyId == companyId &&
                            m.CompanyClientId == companyClientId)
                .Join(
                    _db.Itemstatus.AsNoTracking(), // <-- Ajusta DbSet master si el nombre real difiere
                    m => m.ItemStatusId,
                    s => s.Id,
                    (m, s) => new WMSCompanyClientItemStatusReadDTO
                    {
                        Id = m.Id,
                        ItemStatusId = s.Id,
                        ItemStatusName = s.Name, // <-- Ajusta propiedad de nombre si no es Name
                        IsActive = m.IsActive
                    })
                .FirstOrDefaultAsync(ct);
        }

        /// <inheritdoc />
        public async Task<bool> ExistsMappingAsync(
            int companyId,
            int companyClientId,
            int itemStatusId,
            int? excludeId,
            CancellationToken ct)
        {
            var query = _db.CompanyClientItemStatuses
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId &&
                            x.CompanyClientId == companyClientId &&
                            x.ItemStatusId == itemStatusId);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync(ct);
        }

        /// <inheritdoc />
        public async Task<bool> MasterExistsAsync(
            int itemStatusId,
            CancellationToken ct)
        {
            return await _db.Itemstatus
                .AsNoTracking()
                .AnyAsync(x => x.Id == itemStatusId, ct);
        }

        /// <inheritdoc />
        public async Task<bool> MasterIsActiveAsync(
            int itemStatusId,
            CancellationToken ct)
        {
            return await _db.Itemstatus
                .AsNoTracking()
                .AnyAsync(x => x.Id == itemStatusId && x.Active, ct);
            // <-- Cambia x.Active por x.IsActive si tu entidad master usa IsActive
        }

        /// <inheritdoc />
        public async Task<DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus?> GetEntityByIdAsync(
            int id,
            int companyId,
            int companyClientId,
            CancellationToken ct)
        {

     


            return await _db.CompanyClientItemStatuses
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);
        }

        /// <inheritdoc />
        public async Task<DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus> CreateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus entity,
            CancellationToken ct)
        {
            _db.CompanyClientItemStatuses.Add(entity);
            await _db.SaveChangesAsync(ct);
            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus entity,
            CancellationToken ct)
        {
            _db.CompanyClientItemStatuses.Update(entity);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(
            DUNES.API.ModelsWMS.Masters.CompanyClientItemStatus entity,
            CancellationToken ct)
        {
            _db.CompanyClientItemStatuses.Remove(entity);
            return await _db.SaveChangesAsync(ct) > 0;
        }

        /// <inheritdoc />
        public async Task<bool> SetActiveAsync(
            int id,
            int companyId,
            int companyClientId,
            bool isActive,
            CancellationToken ct)
        {
            var entity = await _db.CompanyClientItemStatuses
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.CompanyId == companyId &&
                    x.CompanyClientId == companyClientId, ct);

            if (entity is null)
                return false;

            entity.IsActive = isActive;

            return await _db.SaveChangesAsync(ct) > 0;
        }
    }
}
